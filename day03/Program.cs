var lines = File.ReadAllLines("input.txt");

const string allItemTypes = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";

static IEnumerable<string> Compartmentize(string rucksack) =>
    new string[] {rucksack[..(rucksack.Length / 2)], rucksack[(rucksack.Length / 2)..]};

static IEnumerable<char> CommonItems(IEnumerable<string> groups) =>
    groups.Aggregate((IEnumerable<char>)allItemTypes, (a, e) => a.Intersect(e));

static int Priority(char itemType) =>
    (int)itemType switch {
        var p when 65 < p && p <= 90 => p - 38,
        var p when 97 < p && p <= 122 => p - 96,
        _ => throw new ArgumentException("Unknown item type")
    };

var answer1 = lines.Select(Compartmentize).SelectMany(CommonItems).Select(Priority).Sum();
Console.WriteLine($"Answer part 1: {answer1}");

var answer2 = lines.Chunk(3).SelectMany(CommonItems).Select(Priority).Sum();
Console.WriteLine($"Answer part 2: {answer2}");