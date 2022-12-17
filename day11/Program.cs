var lines = File.ReadAllText("input.txt");

var monkeys =
    lines
        .Split(Environment.NewLine + Environment.NewLine)
        .Select(Monkey.Create)
        .ToList();

static void PlayRound(List<Monkey> monkeys)
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

static void PrintInventory(List<Monkey> monkeys)
{
    for (int i = 0; i < monkeys.Count; i++)
    {
        Console.WriteLine($"monkey {i}: " + string.Join(", ", monkeys[i].Items));
    }
}

// System.Console.WriteLine("Start:");
// PrintInventory(monkeys);
// PlayRound(monkeys);
// Console.WriteLine("After round 1:");
// PrintInventory(monkeys);

// Play 20 rounds
for (int i = 0; i < 20; i++)
{
    PlayRound(monkeys);
}

PrintInventory(monkeys);

var answer1 = monkeys
    .Select(m => m.ItemsInspected)
    .OrderByDescending(x => x)
    .Take(2)
    .Aggregate((x, y) => x * y);
Console.WriteLine($"Answer part 1: {answer1}");

public class Monkey
{
    private readonly List<long> _items;
    private readonly char _operator;
    private readonly int? _operand2;
    private readonly int _divisor;
    private readonly int _monkeyIfTrue;
    private readonly int _monkeyIfFalse;
    private List<(int, long)> _outbox;

    private Monkey(IEnumerable<long> items, char op, int? operand2, int divisor, int monkeyIfTrue, int monkeyIfFalse)
    {
        _items = new List<long>(items);
        _operator = op;
        _operand2 = operand2;
        _divisor = divisor;
        _monkeyIfTrue = monkeyIfTrue;
        _monkeyIfFalse = monkeyIfFalse;
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

    public int ItemsInspected { get; private set; }

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
        } / 3;
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