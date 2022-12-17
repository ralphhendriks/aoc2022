var lines = File.ReadAllText("test.txt");

static void PlayRounds(List<Monkey> monkeys, int rounds)
{
    for (int r = 0; r < rounds; r++)
    {
        foreach (var monkey in monkeys)
        {
            monkey.InspectItems();
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
    monkeys
        .Select(m => m.ItemsInspected)
        .OrderByDescending(x => x)
        .Take(2)
        .Aggregate((x, y) => x * y);

var monkeysPart1 =
    lines
        .Split(Environment.NewLine + Environment.NewLine)
        .Select(Monkey.Create)
        .ToList();
PlayRounds(monkeysPart1, 20);
var answer1 = MonkeyBusiness(monkeysPart1);
Console.WriteLine($"Answer part 1: {answer1}");

var monkeysPart2 =
    lines
        .Split(Environment.NewLine + Environment.NewLine)
        .Select(Monkey.CreateWithoutRelief)
        .ToList();
PlayRounds(monkeysPart2, 1000);
PrintInspections(monkeysPart2);
// PlayRounds(monkeysPart2, 10_000);
// var answer2 = MonkeyBusiness(monkeysPart2);
// Console.WriteLine($"Answer part 2: {answer2}");

public class Monkey
{
    private readonly List<long> _items;
    private readonly char _operator;
    private readonly long? _operand2;
    private readonly long _divisor;
    private readonly int _monkeyIfTrue;
    private readonly int _monkeyIfFalse;
    private readonly long _relief;
    private List<(int, long)> _outbox;

    private Monkey(
        IEnumerable<long> items,
        char op,
        long? operand2,
        long divisor,
        int monkeyIfTrue,
        int monkeyIfFalse,
        long relief)
    {
        _items = new List<long>(items);
        _operator = op;
        _operand2 = operand2;
        _divisor = divisor;
        _monkeyIfTrue = monkeyIfTrue;
        _monkeyIfFalse = monkeyIfFalse;
        _relief = relief;
        _outbox = new();
    }

    public void Print()
    {
        Console.WriteLine("Items: " + string.Join(',', _items));
        Console.WriteLine("Operator: " + _operator);
        Console.WriteLine("Operand 2: " + _operand2 ?? "old");
        Console.WriteLine("Divisor: " + _divisor);
        Console.WriteLine("If true throw to: " + _monkeyIfTrue);
        Console.WriteLine("If false throw to: " + _monkeyIfFalse);
    }

    public IEnumerable<long> Items => _items;

    public IEnumerable<(int, long)> Outbox => _outbox;

    public long ItemsInspected { get; private set; }

    public void AddItem(long worryLevel) => _items.Add(worryLevel);

    public void InspectItems()
    {
        _outbox = _items.Select(InspectItem).ToList();
        ItemsInspected += _items.Count;
        _items.Clear();
    }

    private (int, long) InspectItem(long worryLevel)
    {
        var op2 = _operand2 ?? worryLevel;
        var newWorryLevel = _operator switch
        {
            '+' => worryLevel + op2,
            '*' => worryLevel * op2,
            _ => throw new InvalidOperationException("Unknown operator")
        } / _relief;
        return newWorryLevel % _divisor == 0 ? (_monkeyIfTrue, newWorryLevel) : (_monkeyIfFalse, newWorryLevel);
    }

    public static Monkey Create(string input) => CreateInt(input, 3);

    public static Monkey CreateWithoutRelief(string input) => CreateInt(input, 1);

    private static Monkey CreateInt(string input, int relief)
    {
        var lines = input.Split(Environment.NewLine);
        var items = lines[1][18..].Split(',').Select(long.Parse);
        var op = lines[2][23];
        int? operand2 = lines[2][25..] == "old" ? null : int.Parse(lines[2][25..]);
        var divisor = int.Parse(lines[3][21..]);
        var monkeyIfTrue = int.Parse(lines[4][29..]);
        var monkeyIfFalse = int.Parse(lines[5][30..]);
        return new Monkey(items, op, operand2, divisor, monkeyIfTrue, monkeyIfFalse, relief);
    }
}