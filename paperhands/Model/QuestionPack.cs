using System.Collections.ObjectModel;

namespace paperhands.Model;

public enum Difficulty
{
    Easy,
    Medium,
    Hard
}

public class QuestionPack(
    string name = "My Question Pack",
    Difficulty difficulty = Difficulty.Easy,
    int timeLimitSeconds = 30)
{
    public string Name { get; set; } = name;
    public Difficulty Difficulty { get; set; } = difficulty;
    public int TimeLimitSeconds { get; set; } = timeLimitSeconds;
    public ObservableCollection<Question> Questions { get; set; }
}