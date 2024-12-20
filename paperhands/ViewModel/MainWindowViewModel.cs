using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using Microsoft.EntityFrameworkCore;
using System.Windows.Threading;
using paperhands.Command;
using paperhands.Model;
using paperhands.Model.Context;
using paperhands.View;
using Wpf.Ui;
using Wpf.Ui.Controls;
using System.Windows.Documents;
using paperhands.Model.Entities;

namespace paperhands.ViewModel;

public class MainWindowViewModel : ViewModelBase
{
    private UserControl? _currentView;
    private bool PreviousIsDBConnectedState;
    public bool IsDBConnected;
    public BookstoreDbContext dbContext;

    public MainWindowViewModel(ISnackbarService snackbarService)
    {
        this.snackbarService = snackbarService;

        dbContext = new BookstoreDbContext();

        ShowImporterViewCommand = new DelegateCommand(ShowImporterView);
        ShowConfigurationViewCommand = new DelegateCommand(ShowConfigurationView);
        ShowAuthorViewCommand = new DelegateCommand(ShowAuthorView);
        ExitApplicationCommand = new DelegateCommand(ExitApplication);

        ImporterViewModel = new ImporterViewModel(this);
        ImporterView = new ImporterView(ImporterViewModel);

        AuthorViewModel = new AuthorViewModel(this);
        AuthorView = new AuthorView(AuthorViewModel);

        ConfigurationViewModel = new ConfigurationViewModel(this);
        ConfigurationView = new ConfigurationView(ConfigurationViewModel);

        CurrentView = ConfigurationView;

        ConnectionTimer = new DispatcherTimer();
        ConnectionTimer.Interval = TimeSpan.FromSeconds(3);
        ConnectionTimer.Tick += ConnectionTick;
        ConnectionTimer.Start();
    }

    public ISnackbarService snackbarService { get; set; }
    public DelegateCommand ShowImporterViewCommand { get; }
    public DelegateCommand ShowConfigurationViewCommand { get; }
    public DelegateCommand ShowAuthorViewCommand { get; }
    public DelegateCommand ExitApplicationCommand { get; }
    public ImporterViewModel ImporterViewModel { get; set; }
    public ImporterView ImporterView { get; }
    public AuthorViewModel AuthorViewModel { get; set; }
    public AuthorView AuthorView { get; }
    public ConfigurationViewModel ConfigurationViewModel { get; set; }
    public ConfigurationView ConfigurationView { get; }
    public DispatcherTimer ConnectionTimer { get; }

    public UserControl CurrentView
    {
        get => _currentView;
        set
        {
            _currentView = value;
            RaisePropertyChanged();
        }
    }

    private async void ConnectionTick(object sender, EventArgs e)
    {
        await CheckDBConnection();

        if (IsDBConnected && ConfigurationViewModel.Books == null)
        {
            ReloadCurrentView();
        }

        else if(IsDBConnected && !PreviousIsDBConnectedState)
            ShowSuccessSnackbarMessage("Connected", "You are connected to the database again!");

        else if(!IsDBConnected && PreviousIsDBConnectedState)
            ShowErrorSnackbarMessage("Disconnected", "You are not connected to the database!");
    }

    private async Task CheckDBConnection()
    {
        if (await dbContext.Database.CanConnectAsync())
        {
            PreviousIsDBConnectedState = IsDBConnected;
            IsDBConnected = true;
        }
        else
        {
            PreviousIsDBConnectedState = IsDBConnected;
            IsDBConnected = false;
        }
    }

    private void ShowImporterView(object obj)
    {
        ImporterViewModel = new ImporterViewModel(this);
        CurrentView = new ImporterView(ImporterViewModel);
    }

    private void ShowConfigurationView(object obj)
    {
        ConfigurationViewModel = new ConfigurationViewModel(this);
        CurrentView = new ConfigurationView(ConfigurationViewModel);
    }
    private void ShowAuthorView(object obj)
    {
        AuthorViewModel = new AuthorViewModel(this);
        CurrentView = new AuthorView(AuthorViewModel);
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
    public void ReloadCurrentView()
    {
        switch (CurrentView)
        {
            case ConfigurationView configurationView:
                ShowConfigurationView(null);
                break;
            case ImporterView importerView:
                ShowImporterView(null);
                break;
            case AuthorView authorView:
                ShowAuthorView(null);
                break;
            default:
                ShowConfigurationView(null);
                break;
        }
    }
    private void ExitApplication(object obj)
    {
        dbContext.SaveChanges();
        Application.Current.Shutdown();
    }
}