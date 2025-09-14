using System.Text;

public class FileEntry
{
    public string name;
    public string date;
    public bool is_directory;
    public long real_size;
    public string size;

    public FileEntry(string name, string date, bool is_directory, long real_size, string size)
    {
        this.name = name;
        this.date = date;
        this.is_directory = is_directory;
        this.real_size = real_size;
        this.size = size;
    }
}

public class Reader
{
    public string path;
    public Reader(string p)
    {
        this.path = p;
    }

    public Reader()
    {
        this.path = ".";
    }

    public static long DirSize(DirectoryInfo d)
    {
        long size = 0;
        // Add file sizes.
        FileInfo[] fis = d.GetFiles();
        foreach (FileInfo fi in fis)
        {
            size += fi.Length;
        }
        // Add subdirectory sizes.
        DirectoryInfo[] dis = d.GetDirectories();
        foreach (DirectoryInfo di in dis)
        {
            size += DirSize(di);
        }
        return size;
    }

    public List<FileEntry> GetFilesDir()
    {
        if (Directory.Exists(path) == false)
        {
            return [];
        }

        var dir = new DirectoryInfo(path);
        FileInfo[] entries = dir.GetFiles();

        List<FileEntry> files = new List<FileEntry> { };
        foreach ( FileInfo fi in entries)
        {
            string filename = fi.Name;
            long size = fi.Length;
            bool is_dir = fi.Attributes.HasFlag(FileAttributes.Directory);
            string hsize = human_readable_size(size);
            string date = fi.LastWriteTime.Date.ToString("yyyy-MM-dd");
            FileEntry f = new FileEntry(filename, date, is_dir, size, hsize);
            files.Add(f);
        }

        DirectoryInfo[] dirs = dir.GetDirectories();
        foreach ( DirectoryInfo di in dirs)
        {
            string filename = di.Name;
            long size =  DirSize(di);
            bool is_dir = di.Attributes.HasFlag(FileAttributes.Directory);
            string hsize = human_readable_size(size);
            string date = di.LastWriteTime.Date.ToString("yyyy-MM-dd");
            FileEntry f = new FileEntry(filename, date, is_dir, size, hsize);
            files.Add(f);
        }

        return files;
    }

    public static int maxlen(List<FileEntry> files)
    {
        int max = 0;

        foreach (var file in files)
        {
            if (file.name.Length > max)
            {
                max = file.name.Length;
            }
        }

        return max;
    }

    public static void PrintFiles(List<FileEntry> files)
    {
        int max = maxlen(files);
        string col1 = "Date Mod";
        string col2 = "Size";
        string col3 = "Filename";

        Console.WriteLine(col1 + "\t" + col2 + "\t" + col3);
        foreach (var file in files)
        {
            string folder_icon = "📁";
            string file_icon = "📑";

            string icon = file.is_directory == true ? folder_icon : file_icon;
            Console.Write(file.date + "\t");
            Console.Write(file.size + "\t ");
            Console.WriteLine(file.name + " " + icon);
        }
    }


    public static string human_readable_size(long bytes)
    {
        long gb = 1024 * 1024 * 1024;
        long mb = 1024 * 1024;
        long kb = 1024;
        if (bytes >= gb) return Math.Round(Convert.ToDouble(bytes) / gb, 1) + "GB";
        if (bytes >= mb) return Math.Round((Convert.ToDouble(bytes) / mb), 1) + "MB";
        if (bytes >= kb) return Math.Round((Convert.ToDouble(bytes) / kb), 1) + "KB";

        return bytes + " B ";
    }


}

public abstract class BaseAction
{
    public abstract void add_args(string[] args);
    public abstract void run();
}

public class Action : BaseAction
{
    public string[] arguments = [];
    public override void add_args(string[] args)
    {
        arguments = args;
    }

    public override void run()
    {
        Console.WriteLine("Default action class");
    }
}

public class HelpAction : Action
{
    public string name = "help";
    public string[] flags = ["h", "help"];

    public override void run()
    {
        Console.WriteLine("Usage: rd <directory>");
        Console.WriteLine("Options:");
        Console.WriteLine("  -h, --help: Show this help");
        Console.WriteLine("  -o, --order: list files in alphabetic order");
        Console.WriteLine("  -s, --size: list files in size order");

    }
}


public class ListAction : Action {

    public string name = "list";


    public override void run()
    {
        Reader r;

        if (arguments.Length == 0)
        {
            r = new Reader();
        } else {
            r = new Reader(arguments[0]);
        }

        List<FileEntry> files = r.GetFilesDir();

        if (files.Count == 0)
        {
            Console.WriteLine("Error: No hay elementos en " + r.path);
            return;
        }

        Reader.PrintFiles(files);
    }
}

public class OrderAction : Action {

    public string name = "order";
    public string[] flags = ["o", "order"];

    public override void run()
    {
        Reader r;

        if (arguments.Length == 0)
        {
            r = new Reader();
        } else {
            r = new Reader(arguments[0]);
        }

        List<FileEntry> files = r.GetFilesDir();

        if (files.Count == 0)
        {
            Console.WriteLine("Error: No hay elementos en " + r.path);
            return;
        }

        files.Sort((f1, f2) => (f1.name.CompareTo(f2.name)));

        Reader.PrintFiles(files);
    }
}
public class SizeAction : Action {

    public string name = "size";
    public string[] flags = ["s", "size"];

    public override void run()
    {
        Reader r;

        if (arguments.Length == 0)
        {
            r = new Reader();
        } else {
            r = new Reader(arguments[0]);
        }

        List<FileEntry> files = r.GetFilesDir();

        if (files.Count == 0)
        {
            Console.WriteLine("Error: No hay elementos en " + r.path);
            return;
        }

        files.Sort((f1, f2) => (Convert.ToInt32(f2.real_size - f1.real_size)));

        Reader.PrintFiles(files);
    }
}



class CliApp


{
    public string[] args;

    public CliApp(string[] argv)
    {
        args = argv;
        Console.OutputEncoding = Encoding.UTF8;
    }
    public static bool CheckFlag(string[] flags, string query)
    {
        return Array.Exists(flags, flag => flag == query);
    }

    public static bool ExistsFlags(string[] collection)
    {
        return Array.Exists(collection, arg => arg.StartsWith("-") == true);
    }

    public void Eval()
    {

        ListAction list = new ListAction();
        if (args.Length == 0)
        {

            list.run();
            return;
        }
        
        HelpAction help = new HelpAction();
        OrderAction order = new OrderAction();
        SizeAction size = new SizeAction();

        string[] filtered = Array.FindAll(args, arg => arg.StartsWith("-") == false);
        if (ExistsFlags(args))
        {
            string? check = Array.Find(args, arg => arg.StartsWith("-") == true);

            if (check == null)
            {
                Console.Write("No se encontraron flags");
            }

            string flag = check.Replace("-", "");

            if (CheckFlag(help.flags, flag))
            {
                help.add_args(filtered);
                help.run();
            }
            else if (CheckFlag(order.flags, flag))
            {

                order.add_args(filtered);
                order.run();
            }
            else if (CheckFlag(size.flags, flag))
            {

                size.add_args(filtered);
                size.run();
            }


        } else
        {
            list.add_args(filtered);
            list.run();
        }


    }



}


class MainProgram
{
    static void Main(string[] args)
    {

        CliApp app = new CliApp(args);

        app.Eval();
    }
}

