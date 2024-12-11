using System.Collections.ObjectModel;
using System.Windows.Threading;
using paperhands.Command;
using paperhands.Model;

namespace paperhands.ViewModel;

public class PlayerViewModel : ViewModelBase
{
    private readonly MainWindowViewModel? _mainWindowViewModel;
    private Question _activeQuestion;
    private int _amountOfCorrectAnswers;


    private ObservableCollection<string> _buttonBackgroundColors;

    private ObservableCollection<string> _buttonMouseOverBackgroundColors;
    private int _CorrectQuestions;
    private int _indexOfActiveQuestion;

    private string _playerBackground;

    private ObservableCollection<string> _shuffeledAnswers;

    private int _timeLeft;
    private bool hasAnswered;

    public PlayerViewModel(MainWindowViewModel? mainWindowViewModel)
    {
        ButtonMouseOverBackgroundColors = ["DarkOrange", "DarkOrange", "DarkOrange", "DarkOrange"];
        PlayerBackground = "#202937";
        ButtonBackgroundColors = ["WhiteSmoke", "WhiteSmoke", "WhiteSmoke", "WhiteSmoke"];
        ShuffeledAnswers = [];
        _mainWindowViewModel = mainWindowViewModel;

        AnswerButtonCommand = new DelegateCommand(AnswerButton);

        Timer = new DispatcherTimer();
        Timer.Interval = TimeSpan.FromSeconds(1);
        Timer.Tick += Timer_Tick;
        Timer.Start();
    }

    public QuestionPackViewModel? ActivePack => _mainWindowViewModel?.ActivePack;

    public ObservableCollection<string> ButtonMouseOverBackgroundColors
    {
        get => _buttonMouseOverBackgroundColors;
        set
        {
            _buttonMouseOverBackgroundColors = value;
            RaisePropertyChanged();
        }
    }

    public ObservableCollection<string> ButtonBackgroundColors
    {
        get => _buttonBackgroundColors;
        set
        {
            _buttonBackgroundColors = value;
            RaisePropertyChanged();
        }
    }

    public ObservableCollection<string> ShuffeledAnswers
    {
        get => _shuffeledAnswers;
        set
        {
            _shuffeledAnswers = value;
            RaisePropertyChanged();
        }
    }

    public string PlayerBackground
    {
        get => _playerBackground;
        set
        {
            _playerBackground = value;
            RaisePropertyChanged();
        }
    }

    public DispatcherTimer Timer { get; }

    public DelegateCommand AnswerButtonCommand { get; }

    public int AmountOfCorrectAnswers
    {
        get => _amountOfCorrectAnswers;
        set
        {
            _amountOfCorrectAnswers = value;
            RaisePropertyChanged();
        }
    }


    public int IndexOfActiveQuestion
    {
        get => _indexOfActiveQuestion;
        set
        {
            _indexOfActiveQuestion = value;
            RaisePropertyChanged();
        }
    }

    public int TimeLeft
    {
        get => _timeLeft;
        set
        {
            _timeLeft = value;
            RaisePropertyChanged();
        }
    }

    public Question ActiveQuestion
    {
        get => _activeQuestion;
        set
        {
            _activeQuestion = value;
            RaisePropertyChanged();
        }
    }

    private async void AnswerButton(object obj)
    {
        if (!hasAnswered)
        {
            hasAnswered = true;

            var index = Convert.ToInt32(obj);
            if (ShuffeledAnswers[index] == ActiveQuestion.CorrectAnswer) AmountOfCorrectAnswers++;

            await SetButtonBackgroundColors();
            LoadNextQuestion();
            hasAnswered = false;
        }
    }

    public void Timer_Tick(object sender, EventArgs e)
    {
        if (TimeLeft <= 0) LoadNextQuestion();
        TimeLeft--;
    }

    private async Task SetButtonBackgroundColors()
    {
        var indexOfCorrectAnswer = ShuffeledAnswers.IndexOf(ActiveQuestion.CorrectAnswer);

        for (var i = 0; i < ShuffeledAnswers.Count; i++)
        {
            ButtonMouseOverBackgroundColors[i] = "Red";
            ButtonBackgroundColors[i] = "Red";
        }

        ButtonMouseOverBackgroundColors[indexOfCorrectAnswer] = "Green";
        ButtonBackgroundColors[indexOfCorrectAnswer] = "Green";
        await Task.Delay(2000);

        for (var i = 0; i < ShuffeledAnswers.Count; i++)
        {
            ButtonMouseOverBackgroundColors[i] = "DarkOrange";
            ButtonBackgroundColors[i] = "WhiteSmoke";
        }

        await Task.Delay(50);
    }

    public void ShuffleAnswersForActiveQuestion(Question question)
    {
        Random random = new();
        var allAnswers = question.IncorrectAnswers.ToList();
        allAnswers.Add(question.CorrectAnswer);
        var randomizedList = allAnswers.OrderBy(x => random.Next()).ToList();
        ShuffeledAnswers = new ObservableCollection<string>(randomizedList);
    }

    public void LoadNextQuestion()
    {
        if (ActivePack != null && IndexOfActiveQuestion < ActivePack.Questions.Count)
        {
            TimeLeft = ActivePack.TimeLimitSeconds;
            ActiveQuestion = ActivePack.Questions[IndexOfActiveQuestion];
            ShuffleAnswersForActiveQuestion(ActiveQuestion);
            IndexOfActiveQuestion++;
        }
        else
        {
            Timer.Stop();
            TimeLeft = 30;
            _mainWindowViewModel.ShowResultsView();
        }
    }
}