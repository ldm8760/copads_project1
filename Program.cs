using System.Diagnostics;

class CLI_Handler
{
    // TODO: include a check for user input

    public static void MainLoop()
    {
        Console.WriteLine("Disk usage (dh) project");
        Console.WriteLine("Usage: du [-s] [-d] [-b] <path>");
        while (true)
        {
            var input = Console.ReadLine();
            if (input != null)
            {

                try
                {
                    if (input == "-h")
                    {
                        Console.WriteLine("Usage: du [-s] [-d] [-b] <path>");
                        Console.WriteLine("Summarize disk usage of the set of FILES, recursively for directories.");
                        Console.WriteLine("You MUST specify one of the parameters, -s, -d, or -b");
                        Console.WriteLine("-s Run in single threaded mode");
                        Console.WriteLine("-d Run in parallel mode (uses all available processors)");
                        Console.WriteLine("-b Run in both parallel and single threaded mode.");
                        Console.WriteLine("Runs parallel followed by sequential mode");
                    }
                    else
                    {
                        string[] prompts = input.Split(" ");
                        HandleCLI(prompts[1], prompts[2]);
                    }
                }
                catch (IndexOutOfRangeException)
                {
                    Console.WriteLine("Incorrect usage. Use \"-h\" for help");
                }

            }
        }
    }

    public static void HandleCLI(string mode, string path)
    {
        if (mode == "-s")
        {
            VisualHandler.HandleVisuals(new SequentialMode(), "Sequential", path);
        }
        else if (mode == "-b")
        {
            VisualHandler.HandleVisuals(new ParallelMode(), "Parallel", path);
        }
        else if (mode == "-d")
        {
            VisualHandler.HandleVisuals(new SequentialMode(), "Sequential", path);
            VisualHandler.HandleVisuals(new ParallelMode(), "Parallel", path);
        }
        else
        {
            Console.WriteLine("Use \"-h\" for help");
        }
    }
}

class VisualHandler
{
    public static void HandleVisuals(IThreadHandler threadHandler, string mode, string path)
    {
        Stopwatch stopwatch = Stopwatch.StartNew();
        threadHandler.RunThreadMode(path);
        stopwatch.Stop();
        Console.WriteLine($"{mode} Calculated in: {stopwatch.Elapsed.TotalSeconds}s");
        Console.WriteLine($"{threadHandler.FolderCount:N0} folders, {threadHandler.FileCount:N0} files, {threadHandler.ByteCount:N0} bytes");
    }

}

interface IThreadHandler
{
    public int FolderCount { get; set; }
    public int FileCount { get; set; }
    public long ByteCount { get; set; }
    void RunThreadMode(string path);

}

class SequentialMode : IThreadHandler
{
    public int FolderCount { get; set; } = 0;
    public int FileCount { get; set; } = 0;
    public long ByteCount { get; set; } = 0;

    public void RunThreadMode(string path)
    {
        foreach (string d in Directory.GetDirectories(path))
        {
            if (FileHandler.HasReadAccess(d))
            {
                FolderCount += 1;
                RunThreadMode(d);
            }
        }
        foreach (string f in Directory.GetFiles(path))
        {
            FileCount += 1;
            ByteCount += FileHandler.GetByteCount(f);
        }
    }
}

class ParallelMode: IThreadHandler
{
    List<string> Directories { get; set; } = [];
    public int FolderCount { get; set; } = 0;
    public int FileCount { get; set; } = 0;
    public long ByteCount { get; set; } = 0;

    private void FindPaths(string path)
    {
        Directories.Add(path);

        foreach (string d in Directory.GetDirectories(path))
        {
            if (FileHandler.HasReadAccess(d))
            {
                FolderCount += 1;
                FindPaths(d);
            }
        }
    }

    public void TraversePaths()
    {
        int localFileCount = 0;
        long localByteCount = 0;
        Parallel.ForEach(Directories, d =>
        {
            foreach (string f in Directory.GetFiles(d))
            {
                Interlocked.Increment(ref localFileCount);
                Interlocked.Add(ref localByteCount, FileHandler.GetByteCount(f));
            }
        });

        FileCount = localFileCount;
        ByteCount = localByteCount;
    }

    public void RunThreadMode(string path)
    {
        FindPaths(path);
        TraversePaths();
    }
}

class FileHandler
{
    public static bool HasReadAccess(string path)
    {
        try
        {
            string[] files = Directory.GetFiles(path);
            return true;
        }
        catch (UnauthorizedAccessException)
        {
            return false;
        }
        catch (FileNotFoundException)
        {
            return false;
        }
        catch (Exception e)
        {
            Console.WriteLine($"An unexpected error {e} occured");
            return false;
        }
    }

    public static long GetByteCount(string path)
    {
        FileInfo fileInfo = new(path);
        return fileInfo.Length;
        // try
        // {
        //     return fileInfo.Length;
        // }
        // catch (OverflowException)
        // {
        //     Console.WriteLine("Overflow error");
        //     throw;
        // }
        // catch (FileNotFoundException)
        // {
        //     Console.WriteLine("File not found somehow?");
        //     throw;
        // }
    }

    public static bool IsImageFile(string path)
    {
        string extension = Path.GetExtension(path);
        string[] validExtensions = [".jpg", ".png", ".gif", ".jpeg"];

        if (validExtensions.Contains(extension, StringComparer.OrdinalIgnoreCase))
        {
            return true;
        }
        else
        {
            return false;
        }

    }
}

class Program
{
    static void Main(string[] args)
    {
        // string main = "C:\\Users\\extra\\Fall2025";
        CLI_Handler.MainLoop();
    }
}
