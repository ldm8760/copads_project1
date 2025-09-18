using System.Reflection;

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
        FileHandler fileHandler = new FileHandler(path);
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
    // this must be recursive

    public FileHandler(string path)
    {
        
    }
}

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Test");
        string[] dir = Directory.GetDirectories("C:\\Users\\extra");
        // if (Directory.Exists(directoryPath))
        foreach (string d in dir)
        {
            Console.WriteLine($"{d}");
        }
    }
}
