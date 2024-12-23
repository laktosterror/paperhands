using System.Collections.ObjectModel;
using Microsoft.EntityFrameworkCore;
using paperhands.Command;
using paperhands.Model.Context;
using paperhands.Model.Entities;
using Wpf.Ui.Controls;

namespace paperhands.ViewModel;

public class BookViewModel : ViewModelBase
{
    private readonly MainWindowViewModel? _mainWindowViewModel;
    private BookstoreDbContext _dbContext => _mainWindowViewModel.dbContext;
    public ObservableCollection<Book> Books { get; set; }
    public ObservableCollection<Inventory> Inventories { get; set; }
    public ObservableCollection<Store> Stores { get; set; }
    public ObservableCollection<Author> Authors { get; set; }
    public ObservableCollection<LanguagesLookup> Languages { get; set; }
    public ObservableCollection<Genre> Genres { get; set; }
    public ObservableCollection<Review> Reviews { get; set; }

    public DelegateCommand AddBookCommand { get; }
    public DelegateCommand RemoveBookCommand { get; }


    private int _selectedLanguage;

    public int SelectedLanguage
    {
        get => _selectedLanguage;
        set
        {
            _selectedLanguage = value;
            RaisePropertyChanged();
        }
    }


    private int _selectedGenre;
    public int SelectedGenre
    {
        get => _selectedGenre;
        set
        {
            _selectedGenre = value;
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
            RaisePropertyChanged();
        }
    }

    private string _selectedIsbn13;

    public string SelectedIsbn13
    {
        get => _selectedIsbn13;
        set
        {
            _selectedIsbn13 = value;
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

    public BookViewModel(MainWindowViewModel? mainWindowViewModel)
    {
        _mainWindowViewModel = mainWindowViewModel;

        AddBookCommand = new DelegateCommand(AddBook);
        RemoveBookCommand = new DelegateCommand(RemoveBook);

        if (_mainWindowViewModel.IsDBConnected)
        {
            Books = new ObservableCollection<Book>(_dbContext.Books
            .Include(b => b.Inventories)
            .Include(b => b.Authors)
            .Include(b => b.Publisher)
            .ToList());

            Authors = new ObservableCollection<Author>(_dbContext.Authors
            .Include(b => b.BookIsbn13s)
            .ToList());

            Stores = new ObservableCollection<Store>(_dbContext.Stores.ToList());
            Inventories = new ObservableCollection<Inventory>(_dbContext.Inventories.ToList());
            Languages = new ObservableCollection<LanguagesLookup>(_dbContext.LanguagesLookups.ToList());
            Genres = new ObservableCollection<Genre>(_dbContext.Genres.ToList());
            Reviews = new ObservableCollection<Review>(_dbContext.Reviews.ToList());



            SelectedBook = Books.FirstOrDefault();
            SelectedAmountOfBooks = 1;
        }
    }

    private void AddBook(object obj)
    {
        if (_mainWindowViewModel.IsDBConnected)
        {
            try
            {
                var newBook = new Book
                {
                    Isbn13 = SelectedIsbn13,
                    Title = "New Book",
                    PublishDate = DateTime.Now,
                    Price = 0,
                    GenreId = 1,
                    LanguageId = 1,
                    PublisherId = 1
                };

                Books.Add(newBook);
                _dbContext.Books.Add(newBook);
                _dbContext.SaveChanges();

                var newInventory = new Inventory
                {
                    Isbn13 = newBook.Isbn13,
                    StoreId = SelectedToStore.Id,
                    Amount = SelectedAmountOfBooks
                };

                Inventories.Add(newInventory);
                _dbContext.Inventories.Add(newInventory);
                _dbContext.SaveChanges();


                SelectedBook = newBook;

                _mainWindowViewModel.ShowSuccessSnackbarMessage("Success!", $"Added new book, edit information!");


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
    
    private void RemoveBook(object obj)
    {
        if (_mainWindowViewModel.IsDBConnected)
        {
            try
            {
                var itemsToRemove = _dbContext.Inventories.Where(i => i.Isbn13 == SelectedBook.Isbn13).ToList();
                var reviewsToRemove = _dbContext.Reviews.Where(r => r.BookIsbn13 == SelectedBook.Isbn13).ToList();

                if (itemsToRemove.Any())
                {
                    _dbContext.Authors.Where(a => a.BookIsbn13s.Any(b => b.Isbn13 == SelectedBook.Isbn13)).ToList().ForEach(a => a.BookIsbn13s.Remove(a.BookIsbn13s.First(b => b.Isbn13 == SelectedBook.Isbn13)));
                    _dbContext.Inventories.RemoveRange(itemsToRemove);
                    _dbContext.Reviews.RemoveRange(reviewsToRemove);
                }

                _dbContext.Books.Remove(SelectedBook);

                _dbContext.SaveChanges();

                Authors.Where(a => a.BookIsbn13s.Any(b => b.Isbn13 == SelectedBook.Isbn13)).ToList().ForEach(a => a.BookIsbn13s.Remove(a.BookIsbn13s.First(b => b.Isbn13 == SelectedBook.Isbn13)));
                Reviews.Where(r => r.BookIsbn13 == SelectedBook.Isbn13).ToList().ForEach(r => Reviews.Remove(r));
                Books.Remove(SelectedBook);


                _mainWindowViewModel.ShowSuccessSnackbarMessage("Success!", $"Removed {SelectedBook}");
                SelectedBook = Books.FirstOrDefault();


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