using System.Collections.ObjectModel;
using Microsoft.EntityFrameworkCore;
using paperhands.Command;
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


    private long _availableAmountInFromStore;
    public long AvailableAmountInFromStore
    {
        get => _availableAmountInFromStore;
        set
        {
            _availableAmountInFromStore = value;
            RaisePropertyChanged();

        }
    }

    private Book? _selectedBook;
    public Book? SelectedBook
    {
        get => _selectedBook;
        set
        {
            _selectedBook = value;
            try
            {
                AvailableAmountInFromStore = value.Inventories.Where(i => i.Isbn13 == SelectedBook.Isbn13).First().Amount;
            }
            catch
            {
                AvailableAmountInFromStore = (long)0;
            }
            SelectedAmountOfBooks = 1;
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
            try
            {
                AvailableAmountInFromStore = value.Inventories.Where(i => i.Isbn13 == SelectedBook.Isbn13).First().Amount;
            }
            catch
            {
                AvailableAmountInFromStore = (long)0;
            }
            SelectedAmountOfBooks = 1;
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

        if (_mainWindowViewModel.IsDBConnected)
        {
            Books = new ObservableCollection<Book>(_dbContext.Books
            .Include(b => b.Inventories)
            .Include(b => b.Authors)
            .Include(b => b.Publisher)
            .ToList());

            Stores = new ObservableCollection<Store>(_dbContext.Stores.ToList());

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

                Books = new ObservableCollection<Book>(_dbContext.Books
                .Include(b => b.Inventories)
                .Include(b => b.Authors)
                .Include(b => b.Publisher)
                .ToList());

                Stores = new ObservableCollection<Store>(_dbContext.Stores.ToList());

                SelectedBook.Inventories.First(i => i.Isbn13 == SelectedBook.Isbn13 && i.StoreId == SelectedFromStore.Id).Amount -= SelectedAmountOfBooks;
                SelectedBook.Inventories.First(i => i.Isbn13 == SelectedBook.Isbn13 && i.StoreId == SelectedToStore.Id).Amount += SelectedAmountOfBooks;

                _mainWindowViewModel.ShowSuccessSnackbarMessage("Success!", $"Moved {SelectedAmountOfBooks} books!");


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