#include <iostream>
#include <filesystem>
#include <vector>
#include <cmath>
#include <algorithm>
#include <chrono>

using namespace std;
namespace fs = std::filesystem;

struct HumanReadable
{
    std::uintmax_t size{};

    template<typename Os> friend Os& operator<<(Os& os, HumanReadable hr)
    {
        int i{};
        double mantissa = hr.size;
        for (; mantissa >= 1024.0; mantissa /= 1024.0, ++i)
        {}
        os << std::ceil(mantissa * 10.0) / 10.0 << i["BKMGTPE"];
        return os;
    }
};

class File {
public:
    HumanReadable size;
    string name;
    string date;
    bool is_directory;
    uintmax_t real_size;

    File(HumanReadable size, string name, string date, bool is_dir, uintmax_t rsize) {
        this->size = size;
        this->name = name;
        this->date = date;
        this->is_directory = is_dir;
        this->real_size = rsize;
    }
};

class Reader {

public:
    ~Reader() {

    };
    string path;
    Reader(const fs::path& path) {
        this->path = path.string();
    }

    Reader() {
        this->path = ".";
    }


    Reader(const char* path) {
        this->path = path;
    }

    static HumanReadable get_size_file(uintmax_t size) {
        return HumanReadable { size };
    }

    static uintmax_t get_rsize_file(const fs::directory_entry& entry, bool is_directory) {
        if (is_directory) {
            uintmax_t size = 0;
            for (const auto& fl : fs::recursive_directory_iterator(entry)) {
                if (fs::is_regular_file(fl)) {
                    size += fs::file_size(fl);
                }
            }
            return size;
        }

        return entry.file_size();
    }

    static HumanReadable get_size_dir(const fs::path& dir) {
        uintmax_t size = 0;
        for (const auto& entry : fs::recursive_directory_iterator(dir)) {
            if (fs::is_regular_file(entry)) {
                size += fs::file_size(entry);
            }
        }
        return HumanReadable { size };
    }

    vector<File> static get_formal_files(vector<filesystem::directory_entry> files) {
       vector<File> f_files;
       for (auto &entry: files) {
           string filename = entry.path().filename().string();
           HumanReadable size;
           auto rsize = Reader::get_rsize_file(entry, entry.is_directory());
           if (entry.is_regular_file())
               size = get_size_file(entry.file_size());
           else if (entry.is_directory())
               size = get_size_dir(entry.path());

           auto date = Reader::date_file(entry);
           auto f = File(size, filename, date, entry.is_directory(), rsize);
           f_files.emplace_back(f);
       }
       return f_files;
    }

    vector<filesystem::directory_entry> get_files() const {
        vector<filesystem::directory_entry> files;
        std::filesystem::directory_entry direction{path};

        if (!direction.exists()) {
            cout << "No se encontro la direccion " << path;
            return {};
        }
        for (const auto & entry : fs::directory_iterator(path)) {
            files.emplace_back(entry);
        }

        return files;
    }

    static int max_len(vector<File> files) {
        int max = 0;
        for (const auto & entry : files) {
            int chars = entry.name.length();
            if (chars > max)
                max = chars;
        }

        return max;
    }

    static string date_file(const filesystem::directory_entry& entry) {
        auto sctp = std::chrono::clock_cast<std::chrono::system_clock>(entry.last_write_time());
        auto date = std::format("{:%F}", sctp);

        return date;
    }

    static void print_files(vector<File> entries) {
        int maxlen = Reader::max_len(entries);
        string col3 = "Filename";
        string col1 = "Date Mod";
        string col2 = "Size";
        int margin = maxlen - col1.length();
        string marg_s(margin, ' ');
        cout << col1 << "\t" <<  col2 << "\t" << col3 << "\n";

        for (auto& file : entries) {
            int dif = maxlen - file.name.length();
            string space( dif, ' ');
            string folder_icon = "󰉋";
            string file_icon = "";
            string icon = file.is_directory == true ? folder_icon : file_icon;
            // if (file.name.length() < maxlen) {
                // cout << space;
            // }

            cout << file.date << "\t";
            cout << file.size << "\t";
            cout << file.name << " " << icon << "   ";
            cout << "\n";

        }
    }
};

class BaseAction {
public:
    virtual ~BaseAction() = default;

    virtual void add_args(vector<string> args) = 0;
    virtual void run() = 0;
};

class Action: public BaseAction {

    public:
        string name;
        vector<string> flags;
        vector<string> arguments;
    void add_args(vector<string> args) override {
        arguments = args;
    }

    void run() override {
        cout << "Running default action" << name << "\n";
    }
};

class ListAction: public Action {
    public:
        string name = "list";
    void run() override {
        Reader r;
        vector<filesystem::directory_entry> entries;
        if (arguments.empty()) {
            r = Reader();
            entries = r.get_files();
        } else {
            std::filesystem::directory_entry entry{arguments[0]};
            if (!entry.exists()) {
                cout << "Esa direccion del fs no existe" << endl;
                return;
            }
            r = Reader(arguments[0]);
            entries = r.get_files();
        }

        if (entries.empty()) {
            cout << "Error no hay elementos en " << r.path << "\n";
            return;
        }

        auto files = Reader::get_formal_files(entries);
        Reader::print_files(files);
    }

};

class HelpAction: public Action {
public:
    string name = "help";
    vector<string> flags = {"h", "help"};

    void run() override {
        cout << "Usage: rd <directory>\n";
        cout << "Options:\n";
        cout << "  -h, --help: Show this help\n";
        cout << "  -o, --order: list files in alphabetic order\n";
        cout << "  -s, --size: list files in size order\n";
    }
};

class OrderAction: public Action {
    public:
    string name = "order";
    vector<string> flags = {"o", "order"};

    void run() override {
        Reader r;
        if (arguments.empty()) {
            r = Reader();
        } else {
            cout << arguments[0] << endl;
            r = Reader(arguments[0]);
        }

        auto entries = r.get_files();

        if (entries.empty()) {
            cout << "Error no hay elementos en " << r.path << "\n";
            return;
        }

        typedef filesystem::directory_entry entry;
        ranges::sort(entries, [](const entry& f1, const entry& f2) {
            string name1 = f1.path().filename().string();
            ranges::transform(name1, name1.begin(), ::tolower);
            string name2 = f2.path().filename().string();
            ranges::transform(name2, name2.begin(), ::tolower);
            return name1 < name2;
        });

        auto files = Reader::get_formal_files(entries);
        Reader::print_files(files);
    }
};

class SizeAction: public Action {
    public:
    string name = "size";
    vector<string> flags = {"s", "size"};
    void run() override {
        Reader r;
        if (arguments.empty()) {
            r = Reader();
        } else {
            cout << arguments[0] << endl;
            r = Reader(arguments[0]);
        }

        auto fs_entries = r.get_files();

        if (fs_entries.empty()) {
            cout << "Error no hay elementos en " << r.path << "\n";
            return;
        }

        auto files = Reader::get_formal_files(fs_entries);
        std::sort(files.begin(), files.end(), [](const File& f1, const File& f2) {
            return f1.real_size > f2.real_size;
        });

        Reader::print_files(files);
    }
};

class CliApp {
    public:
        vector<string> args;

    CliApp(int argc, char* argv[]) {
        for (int i = 0; i < argc; i++) {
            args.emplace_back(argv[i]);
        }
    }

    static bool check_flag(vector<string> flags, string query) {
        return std::find(flags.begin(), flags.end(), query) != flags.end();
    }

    static bool exist_flag(vector<string> flags) {
        for (auto f: flags) {
            if (std::any_of(f.begin(), f.end(), [](char c){ return c == '-'; })) {
                return true;
            }
        }

        return false;
    }

    void eval() {
        auto list = ListAction();
        if (args.size() == 1) {
            list.run();
        }

        auto help = HelpAction();
        auto order = OrderAction();
        auto size = SizeAction();
        args.erase(args.begin());

        if (exist_flag(args)) {
            vector<string> filtered;
            ranges::copy_if(args, back_inserter(filtered),
                            [](const string &s) { return s.find('-'); });
            string flag;
            for (auto arg: args) {
                if (arg[0] != '-') continue;

                for (int i = 0; i < arg.length(); i++) {
                    if (arg[i] == '-')
                        continue;
                    flag += arg[i];
                }
            }

            if (CliApp::check_flag(help.flags, flag)) {
                help.add_args(filtered);
                help.run();
            } else if (CliApp::check_flag(order.flags, flag)) {
                order.add_args(filtered);
                order.run();
            } else if (CliApp::check_flag(size.flags, flag)) {
                size.add_args(filtered);
                size.run();
            }
        } else {
            // vector<string> filtered;
            // ranges::copy_if(args, back_inserter(filtered),
            // [](const string& s){ return !s.find('-'); });

            list.add_args(args);
            list.run();
        }

    }

};

int main(int argc, char* argv[]) {

    auto app = CliApp(argc, argv);

    app.eval();

    return 0;
}