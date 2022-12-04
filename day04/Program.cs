var lines = File.ReadAllLines("input.txt");

static int[] ParseInput(string l) =>
    l.Split(new char[]{',','-'}).Select(int.Parse).ToArray();

static bool FullyContains(int[] s) =>
    (s[2] >= s[0] && s[3] <= s[1]) ||
    (s[0] >= s[2] && s[1] <= s[3]);

static bool Overlaps(int[] s) =>
    Enumerable.Range(s[0], s[1]-s[0]+1).Intersect(Enumerable.Range(s[2], s[3]-s[2]+1)).Any();

var answer1 = lines.Select(ParseInput).Where(FullyContains).Count();
Console.WriteLine($"Answer part 1: {answer1}");

var answer2 = lines.Select(ParseInput).Where(Overlaps).Count();
Console.WriteLine($"Answer part 2: {answer2}");