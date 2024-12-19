using System.Collections.ObjectModel;
using System.Net;
using System.Net.Http;
using System.Windows.Threading;
using Microsoft.EntityFrameworkCore;
using paperhands.Command;
using paperhands.Model;
using paperhands.Model.Context;
using paperhands.Model.Entities;

namespace paperhands.ViewModel;

public class ImporterViewModel : ViewModelBase
{
    private readonly MainWindowViewModel? _mainWindowViewModel;
    private BookstoreDbContext _dbContext => _mainWindowViewModel.dbContext;
    public ObservableCollection<Book> Books { get; set; }
    public ObservableCollection<Inventory> Inventories { get; set; }

    public ObservableCollection<Store> Stores { get; set; }
    public DelegateCommand MoveBookCommand { get; }

    private Book? _selectedBook;
    public Book? SelectedBook
    {
        get => _selectedBook;
        set
        {
            _selectedBook = value;
            RaisePropertyChanged();
        }
    }

    private Store _selectedFromStore;

    public Store SelectedFromStore
    {
        get => _selectedFromStore;
        set
        { 
            _selectedFromStore = value;
            RaisePropertyChanged();
        }
    }

    private Store _selectedToStore;

    public Store SelectedToStore
    {
        get => _selectedToStore;
        set 
        { 
            _selectedToStore = value;
            RaisePropertyChanged();
        }
    }

    private int _selectedAmountOfBooks;
    public int SelectedAmountOfBooks
    {
        get => _selectedAmountOfBooks;
        set
        {
            _selectedAmountOfBooks = value;
            RaisePropertyChanged();
        }
    }



    public ImporterViewModel(MainWindowViewModel? mainWindowViewModel)
    {
        _mainWindowViewModel = mainWindowViewModel;

        MoveBookCommand = new DelegateCommand(MoveBook);

        Books = new ObservableCollection<Book>(_dbContext.Books
            .Include(b => b.Inventories)
            .Include(b => b.Authors)
            .Include(b => b.Publisher)
            .ToList());

        Stores = new ObservableCollection<Store>(_dbContext.Stores.ToList());

        if (_dbContext.Database.CanConnect())
        {
            SelectedBook = Books.FirstOrDefault();
            SelectedFromStore = Stores.FirstOrDefault();
            SelectedToStore = Stores.FirstOrDefault();
            SelectedAmountOfBooks = 1;
        }
    }

    private async void MoveBook(object obj)
    {
        if (_mainWindowViewModel.IsDBConnected)
        {
            try
            {
                await _dbContext.Database.ExecuteSqlRawAsync
                    ("EXEC bookstore.dbo.MoveBook @ISBN13 = {0}, @FromStoreID = {1}, @ToStoreID = {2}, @Amount = {3};"
                    , SelectedBook.Isbn13, SelectedFromStore.Id, SelectedToStore.Id, SelectedAmountOfBooks);

                _mainWindowViewModel.ShowSuccessSnackbarMessage("Success!", $"You moved {SelectedAmountOfBooks} books!");


            }
            catch (Exception e)
            {
                _mainWindowViewModel.ShowErrorSnackbarMessage("Error", e.Message);
            }
        }
        else
        {
            _mainWindowViewModel.ShowErrorSnackbarMessage("Failure",
                "You are not connected to database!");
        }
    }
}