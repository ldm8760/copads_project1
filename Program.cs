class CLI_Handler
{
    // public string Mode { get; set; }
    // public string Path { get; set; }

    public CLI_Handler(string mode, string path)
    {
        // Mode = mode;
        // Path = path;
        handleCLI(mode, path);
    }

    private void handleCLI(string mode, string path)
    {
        // FileHandler fileHandler = new FileHandler(path);
        if (mode == "-s")
        {
            SequentialMode seq = new SequentialMode(path);
        }
        else if (mode == "-b")
        {
            ParallelMode par = new ParallelMode(path);
        }
        else if (mode == "-d")
        {
            SequentialMode seq = new SequentialMode(path);
            ParallelMode par = new ParallelMode(path);
        }
        else
        {
            Console.WriteLine("Unknown Input given");
        }
    }
}

class SequentialMode
{
    // public string Path { get; set; }

    public SequentialMode(string path)
    {
        // Path = path;
        runSequential(path);
    }

    private void runSequential(string path)
    {
        // Foreach
    }

}

class ParallelMode
{
    public ParallelMode(string path)
    {
        runParallel(path);
    }

    private void runParallel(string path)
    {
        // Parallel.ForEach
    }
}

class FileHandler
{
    public HashSet<string> files = new HashSet<string>();
    public HashSet<string> directories = new HashSet<string>();
    public string Path { get; set; }
    private int folderCount = 0;
    private int fileCount = 0;
    private int byteCount = 0;

    // this must be recursive

    public FileHandler(string path)
    {
        Path = path;
    }

    public void findAllPaths(string path)
    {
        // base case
        if (File.Exists(path))
        {
            files.Add(path);
            fileCount += 1;
        }
        else if (Directory.Exists(path))
        {
            // Parallel.foreach for parallel
            foreach (string d in Directory.GetDirectories(path))
            {
                directories.Add(d);
                folderCount += 1;
                findAllPaths(d);
            }
            foreach (string f in Directory.GetFiles(path))
            {
                files.Add(f);
                fileCount += 1;
            }
        }
        Console.WriteLine(files);
        Console.WriteLine(directories);
        Console.WriteLine(fileCount);
        Console.WriteLine(folderCount);
    }
}

class Program
{
    static void Main(string[] args)
    {
        string main = "C:\\Users\\extra";
        FileHandler fileHandler = new FileHandler(main);
        fileHandler.findAllPaths(main);
    }
}
