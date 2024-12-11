using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using paperhands.Command;
using paperhands.Model;
using paperhands.View;
using Wpf.Ui;
using Wpf.Ui.Controls;

namespace paperhands.ViewModel;

public class MainWindowViewModel : ViewModelBase
{
    private QuestionPackViewModel? _activePack;
    private Visibility _collapsibleMenuVisibility;
    private UserControl? _currentView;

    public MainWindowViewModel(ISnackbarService snackbarService)
    {
        this.snackbarService = snackbarService;

        FileReader = new FileReader(@"./data.json", this);
        var loadedPacks = FileReader.ReadFromFileAsync();


        ShowImporterViewCommand = new DelegateCommand(ShowImporterView);
        ShowPlayViewCommand = new DelegateCommand(ShowPlayView);
        ShowConfigurationViewCommand = new DelegateCommand(ShowConfigurationView);
        ExitApplicationCommand = new DelegateCommand(ExitApplication);
        ShowMenuCommand = new DelegateCommand(ToggleMenu);
        AddPackCommand = new DelegateCommand(AddPack);
        RemovePackCommand = new DelegateCommand(RemovePack);
        SetActivePackCommand = new DelegateCommand(SetActivePack);

        Packs = loadedPacks.GetAwaiter().GetResult();
        ActivePack = Packs.FirstOrDefault();

        ImporterViewModel = new ImporterViewModel(this);
        ImporterView = new ImporterView(ImporterViewModel);

        ConfigurationViewModel = new ConfigurationViewModel(this);
        ConfigurationView = new ConfigurationView(ConfigurationViewModel);

        PlayerViewModel = new PlayerViewModel(this);
        PlayerView = new PlayerView(PlayerViewModel);

        CollapsibleMenuVisibility = Visibility.Collapsed;
        CurrentView = PlayerView;
    }

    public ISnackbarService snackbarService { get; set; }

    public DelegateCommand ShowImporterViewCommand { get; }
    public DelegateCommand ShowPlayViewCommand { get; }
    public DelegateCommand ShowConfigurationViewCommand { get; }
    public DelegateCommand ExitApplicationCommand { get; }
    public DelegateCommand ShowMenuCommand { get; }
    public DelegateCommand AddPackCommand { get; }
    public DelegateCommand RemovePackCommand { get; }
    public DelegateCommand SetActivePackCommand { get; }
    public ObservableCollection<QuestionPackViewModel> Packs { get; set; }
    public FileReader FileReader { get; set; }
    public OpenTriviaClient? OpenTriviaClient { get; set; }

    public ImporterViewModel ImporterViewModel { get; set; }
    public ImporterView ImporterView { get; }
    public ConfigurationViewModel ConfigurationViewModel { get; set; }
    public ConfigurationView ConfigurationView { get; }
    public PlayerViewModel? PlayerViewModel { get; set; }
    public PlayerView PlayerView { get; }

    public UserControl CurrentView
    {
        get => _currentView;
        set
        {
            _currentView = value;
            RaisePropertyChanged();
        }
    }

    public Visibility CollapsibleMenuVisibility
    {
        get => _collapsibleMenuVisibility;
        set
        {
            _collapsibleMenuVisibility = value;
            RaisePropertyChanged();
        }
    }

    public QuestionPackViewModel? ActivePack
    {
        get => _activePack;
        set
        {
            _activePack = value;
            RaisePropertyChanged();

            ReloadCurrentView();
        }
    }


    private void SetActivePack(object obj)
    {
        if (obj is QuestionPackViewModel selectedPack) ActivePack = selectedPack;
    }

    private void ShowImporterView(object obj)
    {
        PlayerViewModel.Timer.Stop();

        CurrentView = new ImporterView(ImporterViewModel);
    }

    private void ShowPlayView(object obj)
    {
        PlayerViewModel = new PlayerViewModel(this);
        CurrentView = new PlayerView(PlayerViewModel);
    }

    public void ShowResultsView()
    {
        PlayerViewModel.Timer.Stop();

        CurrentView = new ResultsView(PlayerViewModel);
    }

    private void ShowConfigurationView(object obj)
    {
        PlayerViewModel.Timer.Stop();

        ConfigurationViewModel = new ConfigurationViewModel(this);
        CurrentView = new ConfigurationView(ConfigurationViewModel);
        ConfigurationViewModel.AutoSelectFirstQuestion();
    }

    public void ReloadCurrentView()
    {
        switch (CurrentView)
        {
            case PlayerView playerView:
                ShowPlayView(null);
                break;
            case ConfigurationView configurationView:
                ShowConfigurationView(null);
                break;
            case ImporterView importerView:
                ShowImporterView(null);
                break;
            default:
                ShowPlayView(null);
                break;
        }
    }

    private void ToggleMenu(object obj)
    {
        if (CollapsibleMenuVisibility == Visibility.Visible)
            CollapsibleMenuVisibility = Visibility.Collapsed;
        else
            CollapsibleMenuVisibility = Visibility.Visible;
    }

    private void ExitApplication(object obj)
    {
        FileReader.WriteToFileAsync(Packs).GetAwaiter();
        Application.Current.Shutdown();
    }

    private void RemovePack(object obj)
    {
        Packs.Remove(ActivePack);
        ActivePack = Packs.FirstOrDefault();
    }

    private void AddPack(object obj)
    {
        var newPackModel = new QuestionPack("New Question Pack");
        var newPack = new QuestionPackViewModel(newPackModel);
        newPack.Questions.Add(new Question("Why is the sky so blue?", "Dont worry about it!", "Blue is not a color!",
            "What about the colorblind?", "Something with light."));
        Packs.Insert(0, newPack);
        ActivePack = Packs.FirstOrDefault();
    }

    public void ShowSuccessSnackbarMessage(string title, string message)
    {
        snackbarService.Show(title,
            message,
            ControlAppearance.Success,
            new SymbolIcon(SymbolRegular.Checkmark24), TimeSpan.FromSeconds(3));
    }

    public void ShowWarningSnackbarMessage(string title, string message)
    {
        snackbarService.Show(title,
            message,
            ControlAppearance.Caution,
            new SymbolIcon(SymbolRegular.ErrorCircle24), TimeSpan.FromSeconds(4));
    }

    public void ShowErrorSnackbarMessage(string title, string message)
    {
        snackbarService.Show(title,
            message,
            ControlAppearance.Danger,
            new SymbolIcon(SymbolRegular.Warning24), TimeSpan.FromSeconds(5));
    }
}