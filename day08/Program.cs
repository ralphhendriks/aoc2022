var lines = File.ReadAllLines("input.txt");

var forrest = new int[lines.Length, lines[0].Length];
for (int i = 0; i < lines.Length; i++)
{
    for (int j = 0; j < lines[0].Length; j++)
    {
        forrest[i, j] = lines[i][j] - 48;
    }
}

static IEnumerable<int> Northward(int x, int y, int[,] forrest)
{
    for (int i = x - 1; i >= 0 ; i--) yield return forrest[i, y];
}

static IEnumerable<int> Southward(int x, int y, int[,] forrest)
{
    for (int i = x + 1; i < forrest.GetLength(0) ; i++) yield return forrest[i, y];
}

static IEnumerable<int> Eastward(int x, int y, int[,] forrest)
{
    for (int j = y - 1; j >= 0 ; j--) yield return forrest[x, j];
}

static IEnumerable<int> Westward(int x, int y, int[,] forrest)
{
    for (int j = y + 1; j < forrest.GetLength(1) ; j++) yield return forrest[x, j];
}

static bool IsVisible(int x, int y, int[,] forrest) =>
    Northward(x, y, forrest).All(t => t < forrest[x,y]) ||
    Southward(x, y, forrest).All(t => t < forrest[x,y]) ||
    Eastward(x, y, forrest).All(t => t < forrest[x,y]) ||
    Westward(x, y, forrest).All(t => t < forrest[x,y]);


int answer1 = 0;
for (int i = 0; i < forrest.GetLength(0); i++)
{
    for (int j = 0; j < forrest.GetLength(1); j++)
    {
        if (IsVisible(i, j, forrest)) answer1 += 1;
    }
}
Console.WriteLine($"Answer part 1: {answer1}");

static int ViewingDistance(IEnumerable<int> trees, int currentTree)
{
    var rc = 0;
    foreach (var tree in trees)
    {
        rc += 1;
        if (tree >= currentTree) break;
    }
    return rc;
}

static int ScenicScore(int x, int y, int[,] forrest) =>
    ViewingDistance(Northward(x, y, forrest), forrest[x,y]) *
    ViewingDistance(Southward(x, y, forrest), forrest[x,y]) *
    ViewingDistance(Eastward(x, y, forrest), forrest[x,y]) *
    ViewingDistance(Westward(x, y, forrest), forrest[x,y]);

int answer2 = 0;
for (int i = 0; i < forrest.GetLength(0); i++)
{
    for (int j = 0; j < forrest.GetLength(1); j++)
    {
        var ss = ScenicScore(i, j, forrest);
        if (ss > answer2) answer2 = ss;
    }
}
Console.WriteLine($"Answer part 2: {answer2}");
