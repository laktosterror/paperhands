using System.Collections.ObjectModel;
using paperhands.Command;
using paperhands.Model;

namespace paperhands.ViewModel;

public class ConfigurationViewModel : ViewModelBase
{
    private readonly MainWindowViewModel? _mainWindowViewModel;

    private Question? _selectedQuestion;

    public ConfigurationViewModel(MainWindowViewModel? mainWindowViewModel)
    {
        AddQuestionCommand = new DelegateCommand(AddQuestion);
        RemoveQuestionCommand = new DelegateCommand(RemoveQuestion);

        _mainWindowViewModel = mainWindowViewModel;
        Difficulties = new ObservableCollection<Difficulty>(Enum.GetValues(typeof(Difficulty)).Cast<Difficulty>());
    }

    public DelegateCommand AddQuestionCommand { get; }
    public DelegateCommand RemoveQuestionCommand { get; }
    public ObservableCollection<QuestionPackViewModel>? Packs => _mainWindowViewModel?.Packs;
    public ObservableCollection<Difficulty> Difficulties { get; }
    public QuestionPackViewModel? ActivePack => _mainWindowViewModel?.ActivePack;

    public Question? SelectedQuestion
    {
        get => _selectedQuestion;
        set
        {
            _selectedQuestion = value;
            RaisePropertyChanged();
        }
    }

    public void AutoSelectFirstQuestion()
    {
        if (ActivePack != null) SelectedQuestion = ActivePack.Questions.FirstOrDefault();
    }


    public void AddQuestion(object obj)
    {
        ActivePack.Questions.Insert(0, new Question("New Question", "", "", "", ""));
        SelectedQuestion = ActivePack.Questions.FirstOrDefault();
        _mainWindowViewModel.ReloadCurrentView();
    }

    public void RemoveQuestion(object obj)
    {
        ActivePack.Questions.Remove(SelectedQuestion);
        SelectedQuestion = ActivePack.Questions.FirstOrDefault();
    }
}