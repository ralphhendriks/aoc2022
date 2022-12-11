using Day10;

var lines = File.ReadAllLines("input.txt");

static IEnumerable<int> InstructionToDeltas(string instruction) =>
    instruction switch
    {
        "noop" => new int[] {0},
        var s when s.StartsWith("addx ") => new int[] {0, int.Parse(s[5..])},
        _ => throw new ArgumentException("unkown instruction")
    };

static int SignalStrength(IReadOnlyList<int> values, int cycle) => cycle * values[cycle - 2];

var values = lines.SelectMany(InstructionToDeltas).Scan(1, (p, n) => p + n).ToList();

var answer1 = new int[] {20, 60, 100, 140, 180, 220}.Select(c => SignalStrength(values, c)).Sum();
Console.WriteLine($"Answer part 1: {answer1}");

var answer2 =
    String.Join(
        Environment.NewLine,
            Enumerable.Range(1, 1).Concat(values)
        .Select((x, i) => (x >= (i % 40) - 1 && x <= (i % 40) + 1) ? '#' : '.')
        .Chunk(40)
        .Select(c => string.Join(string.Empty, c)));
Console.WriteLine("Answer part 2:");
Console.Write(answer2);