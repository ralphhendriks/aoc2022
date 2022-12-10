using Day09;

var lines = File.ReadAllLines("input.txt");

static (int, int) MoveHead((int, int) h, char move)
{
    var (x, y) = h;
    return move switch {
        'R' => (x + 1, y),
        'L' => (x - 1, y),
        'U' => (x, y + 1),
        'D' => (x, y - 1),
        _ => throw new ArgumentException("Unknown move")
    };
}

static (int, int) MoveTail((int, int) t, (int, int) h)
{
    var (hx, hy) = h;
    var (tx, ty) = t;
    return (hx - tx, hy - ty) switch {
        var (dx, dy) when (Math.Abs(dx) > 1 || Math.Abs(dy) > 1)=> (tx + Math.Sign(dx), ty + Math.Sign(dy)),
        _ => (tx, ty)
    };
}
var headPositions =
    lines
    .SelectMany(l => Enumerable.Repeat(l[0], int.Parse(l[2..])))
    .Scan((0, 0), MoveHead);

var answer1 = headPositions.Scan((0, 0), MoveTail).GroupBy(p => p).Count();
Console.WriteLine($"Answer part 1: {answer1}");

var answer2 = Enumerable.Range(1, 9).Aggregate(headPositions, (p, _) => p.Scan((0, 0), MoveTail)).GroupBy(p => p).Count();
Console.WriteLine($"Answer part 2: {answer2}");