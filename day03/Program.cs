var lines = File.ReadAllLines("input.txt");

const string allItemTypes = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";

static IEnumerable<string> Compartmentize(string rucksack) =>
    new string[] {rucksack[..(rucksack.Length / 2)], rucksack[(rucksack.Length / 2)..]};

static IEnumerable<char> CommonItems(IEnumerable<string> groups) =>
    groups.Aggregate((IEnumerable<char>)allItemTypes, (a, e) => a.Intersect(e));

static int Priority(char itemType) =>
    char.IsAsciiLetterUpper(itemType) ? itemType - 38 : itemType - 96;

var answer1 = lines.Select(Compartmentize).SelectMany(CommonItems).Select(Priority).Sum();
Console.WriteLine($"Answer part 1: {answer1}");

var answer2 = lines.Chunk(3).SelectMany(CommonItems).Select(Priority).Sum();
Console.WriteLine($"Answer part 2: {answer2}");