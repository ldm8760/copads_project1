using System.Diagnostics;

class CLI_Handler
{
    // TODO: include a check for user input

    public CLI_Handler(string mode, string path)
    {
        HandleCLI(mode, path);
    }

    private static void HandleCLI(string mode, string path)
    {
        // FileHandler fileHandler = new FileHandler(path);
        if (mode == "-s")
        {
            VisualHandler.HandleVisuals(new SequentialMode(), path);
        }
        else if (mode == "-b")
        {
            ParallelMode par = new ParallelMode(path);
        }
        else if (mode == "-d")
        {
            VisualHandler.HandleVisuals(new SequentialMode(), path);
            ParallelMode par = new ParallelMode(path);
        }
        else
        {
            Console.WriteLine("Unknown Input given");
        }
    }
}

class VisualHandler
{
    public static void HandleVisuals(IThreadHandler threadHandler, string path)
    {
        Stopwatch stopwatch = new();
        threadHandler.RunThreadMode(path);
        stopwatch.Stop();
        Console.WriteLine($"Sequential Calculated in: {stopwatch.Elapsed.TotalSeconds}s");
        Console.WriteLine($"{threadHandler.FolderCount:N0} folders, {threadHandler.FileCount:N0} files, {threadHandler.ByteCount:N0} bytes");
    }

}

interface IThreadHandler
{
    public int FolderCount { get; set; }
    public int FileCount { get; set; }
    public int ByteCount { get; set; }
    void RunThreadMode(string path);

}
class SequentialMode : IThreadHandler
{
    public int FolderCount { get; set; } = 0;
    public int FileCount { get; set; } = 0;
    public int ByteCount { get; set; } = 0;

    public void RunThreadMode(string path)
    {
        foreach (string d in Directory.GetDirectories(path))
        {
            if (FileHandler.HasReadAccess(d))
            {
                FolderCount += 1;
                RunThreadMode(d);
            }
            Console.WriteLine($"{d}");
        }
        foreach (string f in Directory.GetFiles(path))
        {
            FileCount += 1;
            ByteCount += FileHandler.GetByteCount(f);
        }
    }
}

class ParallelMode
{
    public ParallelMode(string path)
    {
        RunParallel(path);
    }

    private static void RunParallel(string path)
    {
        // Parallel.ForEach
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

    public static int GetByteCount(string path)
    {
        FileInfo fileInfo = new(path);
        try
        {
            int byteCount = Convert.ToInt32(fileInfo.Length);
            return byteCount;
        }
        catch (OverflowException)
        {
            Console.WriteLine("Overflow error");
            throw;
        }
        catch (FileNotFoundException)
        {
            Console.WriteLine("File not found somehow?");
            throw;
        }
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
        string main = "C:\\Users\\extra\\Fall2025";
        SequentialMode sequentialMode = new();
        Stopwatch watch = Stopwatch.StartNew();
        sequentialMode.RunThreadMode(main);
        watch.Stop();
        Console.WriteLine($"Sequential Calculated in: {watch.Elapsed.TotalSeconds}s");
        Console.WriteLine($"{sequentialMode.FolderCount:N0} folders, {sequentialMode.FileCount:N0} files, {sequentialMode.ByteCount:N0} bytes");


        // Console.WriteLine(fileHandler.GetFileCount());
        // Console.WriteLine(fileHandler.GetFolderCount());
        // bool x = FileHandler.HasReadAccess(main);
        // Console.WriteLine(x);
    }
}
