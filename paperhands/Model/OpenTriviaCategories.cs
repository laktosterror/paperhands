namespace paperhands.Model;

public class OpenTriviaCategories
{
    public Trivia_Categories[] trivia_categories { get; set; }
}

public class Trivia_Categories
{
    public int id { get; set; }
    public string name { get; set; }
}