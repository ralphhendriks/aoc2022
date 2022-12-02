var lines = File.ReadAllLines("input.txt");

var answer1 =
    lines.Select(l => l switch
    {
        "A X" => 3 + 1, // rock - rock => draw
        "A Y" => 6 + 2, // rock - paper => win
        "A Z" => 0 + 3, // rock - scissors => loss
        "B X" => 0 + 1, // paper - rock => loss
        "B Y" => 3 + 2, // paper - paper => draw
        "B Z" => 6 + 3, // paper - scissors => win
        "C X" => 6 + 1, // scissors - rock => win
        "C Y" => 0 + 2, // scissors - paper => loss
        "C Z" => 3 + 3, // scissors - scissors => draw
        _ => throw new ArgumentException("Unexpected input")
    }).Sum();
Console.WriteLine($"Answer part 1: {answer1}");

var answer2 =
    lines.Select(l => l switch
    {
        "A X" => 0 + 3, // rock - loss => scissors
        "A Y" => 3 + 1, // rock - draw => rock
        "A Z" => 6 + 2, // rock - win => paper
        "B X" => 0 + 1, // paper - loss => rock
        "B Y" => 3 + 2, // paper - draw => paper
        "B Z" => 6 + 3, // paper -  win => scissors
        "C X" => 0 + 2, // scissors - loss => paper
        "C Y" => 3 + 3, // scissors - draw => scissors
        "C Z" => 6 + 1, // scissors - win => rock
        _ => throw new ArgumentException("Unexpected input")
    }).Sum();
Console.WriteLine($"Answer part 2: {answer2}");