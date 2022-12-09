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

static (int, int) MoveTail((int, int) h, (int, int) t)
{
    var (hx, hy) = h;
    var (tx, ty) = t;
    return (hx - tx, hy - ty) switch {
        ( 2,  1) => (tx + 1, ty + 1),
        ( 2,  0) => (tx + 1, ty    ),
        ( 2, -1) => (tx + 1, ty - 1),
        (-2,  1) => (tx - 1, ty + 1),
        (-2,  0) => (tx - 1, ty    ),
        (-2, -1) => (tx - 1, ty - 1),
        ( 1,  2) => (tx + 1, ty + 1),
        ( 0,  2) => (tx    , ty + 1),
        (-1,  2) => (tx - 1, ty + 1),
        ( 1, -2) => (tx + 1, ty - 1),
        ( 0, -2) => (tx    , ty - 1),
        (-1, -2) => (tx - 1, ty - 1),
        _ => (tx, ty)
    };
}

var positions =
    lines
    .SelectMany(l =>
        Enumerable.Range(1, int.Parse(l[2..]))
        .Select(_ => l[0]))
    .Aggregate(new List<((int, int), (int, int))> { ((0, 0), (0, 0)) }, (List<((int, int), (int, int))> l, char m) =>
    {
        var (h, t) = l.Last();
        var nh = MoveHead(h, m);
        var nt = MoveTail(nh, t);
        l.Add((nh, nt));
        return l;
    });

var answer1 =
    positions
    .Select(p => p.Item2)
    .GroupBy(p => p)
    .Count();
Console.WriteLine($"Answer part 1: {answer1}");