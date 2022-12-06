var input = File.ReadAllText("input.txt");

static int FindDistinctCharacters(string datastream, int n)
{
    HashSet<char> buffer = new();
    for (var i = n - 1; i < datastream.Length; i++)
    {
        buffer.Clear();
        for (var j = i - n + 1; j <= i; j++) buffer.Add(datastream[j]);
        if (buffer.Count == n) return i + 1;
    }
    return -1;
}

var answer1 = FindDistinctCharacters(input, 4);
Console.WriteLine($"Answer part 1: {answer1}");
var answer2 = FindDistinctCharacters(input, 14);
Console.WriteLine($"Answer part 2: {answer2}");