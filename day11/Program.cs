var lines = File.ReadAllText("input.txt").Split(Environment.NewLine + Environment.NewLine);

static void PlayRounds(List<Monkey> monkeys, int rounds, Func<long, long> relief)
{
    for (int r = 0; r < rounds; r++)
    {
        foreach (var monkey in monkeys)
        {
            monkey.InspectItems(relief);
            foreach (var (i, worryLevel) in monkey.Outbox)
            {
                monkeys[i].AddItem(worryLevel);
            }
        }
    }
}

static void PrintInventory(List<Monkey> monkeys)
{
    for (int i = 0; i < monkeys.Count; i++)
    {
        Console.WriteLine($"monkey {i}: " + string.Join(", ", monkeys[i].Items));
    }
}

static void PrintInspections(List<Monkey> monkeys)
{
    for (int i = 0; i < monkeys.Count; i++)
    {
        Console.WriteLine($"Monkey {i} inspected items {monkeys[i].ItemsInspected} times.");
    }
}

static long MonkeyBusiness(List<Monkey> monkeys) =>
    monkeys.Select(m => m.ItemsInspected).OrderByDescending(x => x).Take(2).Aggregate(((x, y) => x * y));

var monkeysPart1 = lines.Select(Monkey.Create).ToList();
PlayRounds(monkeysPart1, 20, x => x / 3);
var answer1 = MonkeyBusiness(monkeysPart1);
Console.WriteLine($"Answer part 1: {answer1}");

var monkeysPart2 = lines.Select(Monkey.Create).ToList();
var lcd = monkeysPart2.Select(m => m.Divisor).Aggregate((x, y) => x * y);
PlayRounds(monkeysPart2, 10_000, x => x % lcd);
var answer2 = MonkeyBusiness(monkeysPart2);
Console.WriteLine($"Answer part 2: {answer2}");

public class Monkey
{
    private readonly List<long> _items;
    private readonly char _operator;
    private readonly long? _rhs;
    private readonly int _divisor;
    private readonly int _monkeyIfTrue;
    private readonly int _monkeyIfFalse;
    private List<(int, long)> _outbox;

    private Monkey(IEnumerable<long> items, char op, long? rhs, int divisor, int monkeyIfTrue, int monkeyIfFalse)
    {
        _items = new List<long>(items);
        _operator = op;
        _rhs = rhs;
        _divisor = divisor;
        _monkeyIfTrue = monkeyIfTrue;
        _monkeyIfFalse = monkeyIfFalse;
        _outbox = new();
    }

    public IEnumerable<long> Items => _items;
    public long Divisor => _divisor;

    public IEnumerable<(int, long)> Outbox => _outbox;

    public long ItemsInspected { get; private set; }

    public void AddItem(long worryLevel) => _items.Add(worryLevel);

    public void InspectItems(Func<long, long> relief)
    {
        _outbox = _items.Select(i => InspectItem(i, relief)).ToList();
        ItemsInspected += _items.Count;
        _items.Clear();
    }

    private (int, long) InspectItem(long worryLevel, Func<long, long> reliefFunc)
    {
        var newWorryLevel = _operator switch
        {
            '+' => worryLevel + (_rhs ?? worryLevel),
            '*' => worryLevel * (_rhs ?? worryLevel),
            _ => throw new InvalidOperationException("Unknown operator")
        };
        newWorryLevel = reliefFunc(newWorryLevel);
        return newWorryLevel % _divisor == 0 ? (_monkeyIfTrue, newWorryLevel) : (_monkeyIfFalse, newWorryLevel);
    }

    public static Monkey Create(string input)
    {
        var lines = input.Split(Environment.NewLine);
        var items = lines[1][18..].Split(',').Select(long.Parse);
        var op = lines[2][23];
        int? operand2 = lines[2][25..] == "old" ? null : int.Parse(lines[2][25..]);
        var divisor = int.Parse(lines[3][21..]);
        var monkeyIfTrue = int.Parse(lines[4][29..]);
        var monkeyIfFalse = int.Parse(lines[5][30..]);
        return new Monkey(items, op, operand2, divisor, monkeyIfTrue, monkeyIfFalse);
    }
}