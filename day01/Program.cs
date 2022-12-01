var lines = File.ReadAllLines("input.txt");

List<int> totals = new();
int current = 0;
foreach (var line in lines)
{
    if (line == string.Empty)
    {
        totals.Add(current);
        current = 0;
    }
    else
    {
        current += int.Parse(line);
    }
}
totals.Add(current);

var answer1 = totals.Max();
Console.WriteLine($"Answer part 1: {answer1}");

var answer2 = totals.OrderByDescending(p => p).Take(3).Sum();
Console.WriteLine($"Answer part 2: {answer2}");