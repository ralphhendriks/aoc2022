var lines = File.ReadAllLines("input.txt");

static IEnumerable<char> ItemTypesInBothCompartments(string rucksack) {
    var s1 = new HashSet<char>(rucksack.Substring(0, rucksack.Length/2).ToCharArray());
    var s2 = new HashSet<char>(rucksack.Substring(rucksack.Length/2).ToCharArray());
    s2.IntersectWith(s1);
    return s2;
}

static int Priority(char itemType) =>
    (int)itemType switch {
        var p when 65 < p && p <= 90 => p - 38,
        var p when 97 < p && p <= 122 => p - 96,
        _ => throw new ArgumentException("Unknown item type")
    };

var answer1 = lines.SelectMany(ItemTypesInBothCompartments).Select(Priority).Sum();
Console.WriteLine($"Answer part 1: {answer1}");

static IEnumerable<char> CommonItems(IEnumerable<string> groups)
{
    var s = new HashSet<char>(groups.First().ToCharArray());
    foreach (var g in groups.Skip(1))
    {
        s.IntersectWith(new HashSet<char>(g.ToCharArray()));
    }
    return s;
}

var answer2 = lines.Chunk(3).SelectMany(CommonItems).Select(Priority).Sum();
Console.WriteLine($"Answer part 2: {answer2}");