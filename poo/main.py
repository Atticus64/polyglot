import os
import sys
from functools import singledispatch
from pathlib import Path
from datetime import datetime
from typing import List
from abc import ABC, abstractmethod
from sys import argv


class FileEntry:
    name: str
    date: str
    is_directory: bool
    real_size: int
    size: str

    def __init__(self, name: str, dat: str, is_dir: bool, rsz: int, size: str):
        self.name = name
        self.date = dat
        self.is_directory = is_dir
        self.real_size = rsz
        self.size = size


class Reader:
    path: str

    @singledispatch
    def __init__(self, arg: str = '.'):
        self.path = arg

    @__init__.register
    def _(self, arg: Path):
        self.path = arg

    def get_size_dir(self, path='.'):
        total_size = 0
        for dirpath, dirnames, filenames in os.walk(path):
            for f in filenames:
                fp = os.path.join(dirpath, f)
                # skip if it is symbolic link
                if not os.path.islink(fp):
                    total_size += os.path.getsize(fp)

        return total_size

    def get_size_file(self, path):
        return os.path.getsize(path)

    def get_size(self, entry: os.DirEntry):
        if entry.is_dir():
            return self.get_size_dir(entry.path)
        elif entry.is_file():
            return self.get_size_file(entry.path)

    def human_read_size(self, quantity: int):
        gb = 1024 * 1024 * 1024
        mb = 1024 * 1024
        kb = 1024
        if quantity >= gb:
            return str(round(quantity / gb, 1)) + "GB"
        if quantity >= mb:
            return str(round(quantity / mb, 1)) + "MB"
        if quantity >= kb:
            return str(round(quantity / kb, 1)) + "KB"

        return str(quantity) + "B"

    def get_files(self) -> List[FileEntry]:
        files = []
        if not os.path.exists(self.path):
            return files

        for entry in os.scandir(self.path):
            name = entry.name
            timestamp = os.path.getctime(self.path)
            date_created = datetime.fromtimestamp(timestamp)
            str_DT = date_created.strftime('%Y-%m-%d')
            date = str_DT
            is_dir = entry.is_dir()
            rsz = self.get_size(entry)
            size = self.human_read_size(rsz)
            file = FileEntry(name, date, is_dir, rsz, size)
            files.append(file)

        return files

    def print_files(self, files: List[FileEntry]):
        col1 = "Date Mod"
        col2 = "Size"
        col3 = "Filename"

        print(f"{col1}\t{col2}\t{col3}")
        for file in files:
            folder_icon = "📁"
            file_icon = "📑"
            icon = folder_icon if file.is_directory else file_icon
            print(f"{file.date}\t{file.size}\t{file.name} {icon}")


def unwrap_files(arguments: List[str]):
    r: Reader
    if len(arguments) == 0:
        r = Reader()
    else:
        r = Reader(arguments[0])

    list_files = r.get_files()
    if len(list_files) == 0:
        print(f"No hay elementos en {r.path}")
        return [], r

    return list_files, r


class BaseAction(ABC):
    arguments = []

    @abstractmethod
    def add_args(self, args):
        pass

    @abstractmethod
    def run(self):
        pass


class Action(BaseAction):
    def __init__(self):
        self.arguments = []

    def add_args(self, args):
        self.arguments = args

    def run(self):
        print("default action running!")


class HelpAction(Action):
    name = "help"
    flags = ["h", "help"]

    def __init__(self):
        super().__init__()

    def run(self):
        print("Usage: rd <directory>")
        print("Options:")
        print("  -h, --help: Show this help")
        print("  -o, --order: list files in alphabetic order")
        print("  -s, --size: list files in size order")


class ListAction(Action):
    name = "list"

    def __init__(self):
        super().__init__()

    def run(self):
        list_files, r = unwrap_files(self.arguments)

        if len(list_files) == 0:
            return

        r.print_files(list_files)


class OrderAction(Action):
    name = "order"
    flags = ["o", "order"]

    def __init__(self):
        super().__init__()

    def run(self):
        list_files, r = unwrap_files(self.arguments)

        # alphabetic order
        list_files = sorted(list_files, key=lambda x: x.name)

        r.print_files(list_files)


class SizeAction(Action):
    name = "size"
    flags = ["s", "size"]

    def __init__(self):
        super().__init__()

    def run(self):
        list_files, r = unwrap_files(self.arguments)

        list_files = sorted(list_files, key=lambda x: x.real_size, reverse=True)

        r.print_files(list_files)


class CliApp:
    args: list[str]

    def __init__(self, arg: list[str]):
        self.args = arg

    def check_flag(self, flags: List[str], query: str):
        return query in flags

    def exists_flags(self, argv: list[str]):
        return any('-' in arg for arg in argv)

    def eval(self):
        listA = ListAction()
        if len(self.args) == 0:
            listA.run()
            return

        helpA = HelpAction()
        order = OrderAction()
        size = SizeAction()

        filtered = list(filter(lambda arg:  '-' not in arg, self.args))
        if self.exists_flags(self.args):
            flag: str = list(filter(lambda arg: '-' in arg, self.args)).pop()

            if flag is None:
                    print("No se encontraron flags")
                    return

            flag = flag.replace('-', '')
            if self.check_flag(helpA.flags, flag):
                helpA.add_args(filtered)
                helpA.run()
            elif self.check_flag(order.flags, flag):
                order.add_args(filtered)
                order.run()
            elif self.check_flag(size.flags, flag):
                size.add_args(filtered)
                size.run()

        else:
            listA.add_args(filtered)
            listA.run()


if __name__ == '__main__':
    argv = sys.argv[1:] or []
    app = CliApp(argv)
    app.eval()
