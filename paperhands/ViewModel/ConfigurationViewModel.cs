using System.Collections.ObjectModel;
using Microsoft.EntityFrameworkCore;
using paperhands.Command;
using paperhands.Model.Context;
using paperhands.Model.Entities;

namespace paperhands.ViewModel;

public class ConfigurationViewModel : ViewModelBase
{
    private readonly MainWindowViewModel? _mainWindowViewModel;

    private Book? _selectedBook;


    public ConfigurationViewModel(MainWindowViewModel? mainWindowViewModel)
    {
        AddBookCommand = new DelegateCommand(AddBook);
        RemoveBookCommand = new DelegateCommand(RemoveBook);

        _mainWindowViewModel = mainWindowViewModel;

        if (_mainWindowViewModel.IsDBConnected)
        {
            //TODO: add control if is db connected
            Books = new ObservableCollection<Book>(_dbContext.Books
                .Include(b => b.Inventories)
                .Include(b => b.Authors)
                .Include(b => b.Publisher)
                .ToList());

            Inventories = new ObservableCollection<Inventory>(_dbContext.Inventories
                .Include(b => b.Store)
                .ToList());

            //Authors = new ObservableCollection<Author>(_dbContext.Authors.ToList());
            //Stores = new ObservableCollection<Store>(_dbContext.Stores.ToList());
            Genres = new ObservableCollection<Genre>(_dbContext.Genres.ToList());
            //Publishers = new ObservableCollection<Publisher>(_dbContext.Publishers.ToList());
            Languages = new ObservableCollection<LanguagesLookup>(_dbContext.LanguagesLookups.ToList());
            Reviews = new ObservableCollection<Review>(_dbContext.Reviews.ToList());
        }
    }

    private BookstoreDbContext _dbContext => _mainWindowViewModel.dbContext;

    public ObservableCollection<Book> Books { get; set; }
    public ObservableCollection<Inventory> Inventories { get; set; }
    public ObservableCollection<Publisher> Publishers { get; set; }
    public ObservableCollection<Author> Authors { get; set; }
    public ObservableCollection<LanguagesLookup> Languages { get; set; }
    public ObservableCollection<Review> Reviews { get; set; }
    public ObservableCollection<Store> Stores { get; set; }
    public ObservableCollection<Genre> Genres { get; set; }

    public DelegateCommand AddBookCommand { get; }
    public DelegateCommand RemoveBookCommand { get; }

    public Book? SelectedBook
    {
        get => _selectedBook;
        set
        {
            _selectedBook = value;
            RaisePropertyChanged();
        }
    }

    public void AddBook(object obj)
    {
        //ActivePack.Questions.Insert(0, new Question("New Question", "", "", "", ""));
        //SelectedBook = ActivePack.Questions.FirstOrDefault();
        _mainWindowViewModel.ReloadCurrentView();
    }

    public void RemoveBook(object obj)
    {
        //ActivePack.Questions.Remove(SelectedBook);
        //SelectedBook = ActivePack.Questions.FirstOrDefault();
    }

    public void MoveBook(object obj)
    {
        //ActivePack.Questions.Remove(SelectedBook);
        //SelectedBook = ActivePack.Questions.FirstOrDefault();
    }
}