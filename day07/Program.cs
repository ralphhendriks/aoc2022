var lines = File.ReadAllLines("input.txt");

var root = Directory.Root;
var pwd = root;
foreach (var l in lines)
{
    if (l == "$ cd /")
    {
    }
    else if (l == "$ cd ..")
    {
        pwd = pwd.Parent;
    }
    else if (l.StartsWith("$ cd "))
    {
        pwd = pwd.GetDirectory(l[5..]);
    }
    else if (l == "$ ls")
    {
    }
    else if (l.StartsWith("dir "))
    {
        pwd.AddDirectory(l[4..]);
    }
    else
    {
        var parts = l.Split(' ');
        pwd.AddFile(int.Parse(parts[0]));
    }
}

var directorySizes = root.GetDirectorySizes().Concat(new int[] {root.Size()});

var answer1 = directorySizes.Where(s => s <= 100_000).Sum();
Console.WriteLine($"Answer part 1: {answer1}");

var answer2 = directorySizes.Order().First(s => s > root.Size() - 40_000_000);
Console.WriteLine($"Answer part 2: {answer2}");

public class Directory
{
    private readonly Directory? _parent;
    private int _totalFileSize;
    private readonly Dictionary<string, Directory> _directories = new();

    private Directory(Directory? parent) => _parent = parent;

    public static Directory Root => new Directory(null);

    public void AddFile(int size) => _totalFileSize += size;

    public void AddDirectory(string name) => _directories.Add(name, new Directory(this));

    public Directory GetDirectory(string name) => _directories[name];

    public Directory Parent => _parent ?? this;

    public int Size() => _totalFileSize + _directories.Values.Select(d => d.Size()).Sum();

    public IEnumerable<int> GetDirectorySizes() =>
        _directories
            .Select(kvp => kvp.Value.Size())
            .Concat(_directories.Values.SelectMany(d => d.GetDirectorySizes()));
}