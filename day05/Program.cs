var lines = File.ReadAllLines("input.txt");

(string[], string, string[]) SplitInputLines(string[] lines) =>
    (
        lines.TakeWhile(l => !l.StartsWith(" 1")).ToArray(),
        lines.Single(l => l.StartsWith(" 1")),
        lines.SkipWhile(l => !l.StartsWith("move")).ToArray()
    );

static Stack<char>[] BuildStacks(string[] stackLines, int highestStack)
{
    var stacks = Enumerable.Range(1, highestStack).Select(_ => new Stack<char>()).ToArray();
    for (var i = stackLines.Length - 1; i >= 0; i--)
    {
        stackLines[i]
            .Chunk(4)
            .Select(g => g.SingleOrDefault(c => char.IsAsciiLetterUpper(c)))
            .Select((crate, index) => (crate, index))
            .ToList()
            .ForEach(p =>{
                var (crate, index) = p;
                if (crate != default(char))
                {
                    stacks[index].Push(crate);
                }
            });
    }
    return stacks;
}

static Stack<char>[] ApplyStepCrateMover9000(Stack<char>[] stacks, string stepLine)
{
    var parts = stepLine.Split(' ');
    foreach (var _ in Enumerable.Range(1, int.Parse(parts[1])))
    {
        var crate = stacks[int.Parse(parts[3]) - 1].Pop();
        stacks[int.Parse(parts[5]) - 1].Push(crate);
    }
    return stacks;
}

static Stack<char>[] ApplyStepCrateMover9001(Stack<char>[] stacks, string stepLine)
{
    var parts = stepLine.Split(' ');
    var temp = new Stack<char>();
    foreach (var _ in Enumerable.Range(1, int.Parse(parts[1])))
    {
        temp.Push(stacks[int.Parse(parts[3]) - 1].Pop());
    }
    foreach (var _ in Enumerable.Range(1, int.Parse(parts[1])))
    {
        stacks[int.Parse(parts[5]) - 1].Push(temp.Pop());
    }
    return stacks;
}

static string TopCrates(Stack<char>[] stacks)
{
    var chars = stacks.Select(s => s.Peek()).ToArray();
    return string.Join(string.Empty, chars);
}

var (stackLines, numberLine, steps) = SplitInputLines(lines);
var highestStack = numberLine.Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).Last();
var answer1 = TopCrates(steps.Aggregate(BuildStacks(stackLines, highestStack), ApplyStepCrateMover9000));
Console.WriteLine($"Answer part 1: {answer1}");
var answer2 = TopCrates(steps.Aggregate(BuildStacks(stackLines, highestStack), ApplyStepCrateMover9001));
Console.WriteLine($"Answer part 2: {answer2}");