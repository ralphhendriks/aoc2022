var lines = File.ReadAllLines("input.txt");

var map = new char[lines[0].Length, lines.Length];
var start = (-1, -1);
var end = (-1, -1);
for (int j = 0; j < lines.Length; j++)
{
    for (int i = 0; i < lines[j].Length; i++)
    {
        if (lines[j][i] == 'S')
        {
            start = (i, j);
            map[i, j] = 'a';
        }
        else if (lines[j][i] == 'E')
        {
            end = (i, j);
            map[i, j] = 'z';
        }
        else
        {
            map[i, j] = lines[j][i];
        }
    }
}

static bool OnMap((int, int) point, char[,] map)
{
    var (x, y) = point;
    return x >= 0 && x <= map.GetLength(0) - 1 && y >= 0 && y <= map.GetLength(1) -1;
}

static bool CanClimb((int, int) from, (int, int) to, char[,] map)
{
    var (i ,j) = from;
    var (x, y) = to;
    return map[x, y] <= map[i, j] + 1;
}

static IEnumerable<(int, int)> PossibleMoves((int, int) point, char[,] map)
{
    var (x, y) = point;
    var moves =
        (new (int, int)[] {(x - 1, y), (x + 1, y), (x, y - 1), (x, y + 1)})
        .Where(p => OnMap(p, map))
        .Where(p => CanClimb(point, p, map))
        .ToList();
    return moves;
}

static int ShortestPath((int, int) start, (int, int) end, char[,] map)
{
    var visited = new HashSet<(int, int)>(new (int, int)[]{ start });
    var front = new HashSet<(int, int)>(new (int, int)[]{ start });
    var d = 0;

    while (!visited.Contains(end))
    {
        d += 1;
        front = new HashSet<(int, int)>(front.SelectMany(p => PossibleMoves(p, map)).Where(p => !visited.Contains(p)));
        if (front.Count == 0)
            return -1;
        visited.UnionWith(front);
    }

    return d;
}

var answer1 = ShortestPath(start, end, map);
Console.WriteLine($"Answer part 1: {answer1}");

static IEnumerable<(int, int)> PossibleStarts(char[,] map)
{
    for (int i = 0; i < map.GetLength(0); i++)
    {
        for (int j = 0; j < map.GetLength(1); j++)
        {
            if (map[i, j] == 'a') yield return (i, j);
        }
    }
}

var answer2 =
    PossibleStarts(map)
    .Select(p => ShortestPath(p, end, map))
    .Where(d => d >= 0)
    .Min();
Console.WriteLine($"Answer part 2: {answer2}");