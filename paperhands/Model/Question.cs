using System.Text.Json.Serialization;
using paperhands.ViewModel;

namespace paperhands.Model;

public class Question : ViewModelBase
{
    private string query;

    public Question(string query, string correctAnswer, string incorrectAnswer1, string incorrectAnswer2,
        string correctAnswer3)
    {
        Query = query;
        CorrectAnswer = correctAnswer;
        IncorrectAnswers = [incorrectAnswer1, incorrectAnswer2, correctAnswer3];
    }

    public Question(string query, string correctAnswer, string[] incorrectAnswers)
    {
        Query = query;
        CorrectAnswer = correctAnswer;
        IncorrectAnswers = incorrectAnswers;
    }

    [JsonConstructor]
    public Question()
    {
        Query = string.Empty;
        CorrectAnswer = string.Empty;
        IncorrectAnswers = [];
    }

    public string Query
    {
        get => query;
        set
        {
            query = value;
            RaisePropertyChanged();
        }
    }

    public string CorrectAnswer { get; set; }
    public string[] IncorrectAnswers { get; set; }
}