using System.Collections.ObjectModel;
using System.Net;
using System.Net.Http;
using System.Windows.Threading;
using Microsoft.EntityFrameworkCore;
using paperhands.Command;
using paperhands.Model;
using paperhands.Model.Context;
using paperhands.Model.Entities;
using Wpf.Ui.Controls;

namespace paperhands.ViewModel;

public class AuthorViewModel : ViewModelBase
{
    private readonly MainWindowViewModel? _mainWindowViewModel;
    private BookstoreDbContext _dbContext => _mainWindowViewModel.dbContext;
    public ObservableCollection<Book> Books { get; set; }
    public ObservableCollection<Author> Authors { get; set; }
    public ObservableCollection<Store> Stores { get; set; }
    public DelegateCommand NewAuthorCommand { get; }
    public DelegateCommand RemoveAuthorCommand { get; }


    private Author? _selectedAuthor;
    public Author? SelectedAuthor
    {
        get => _selectedAuthor;
        set
        {
            _selectedAuthor = value;
            RaisePropertyChanged();
        }
    }

    public AuthorViewModel(MainWindowViewModel? mainWindowViewModel)
    {
        _mainWindowViewModel = mainWindowViewModel;

        NewAuthorCommand = new DelegateCommand(NewAuthor);
        RemoveAuthorCommand = new DelegateCommand(RemoveAuthor);

        if (_mainWindowViewModel.IsDBConnected)
        {
            Books = new ObservableCollection<Book>(_dbContext.Books
                .Include(b => b.Inventories)
                .Include(b => b.Authors)
                .Include(b => b.Publisher)
                .ToList());

            Stores = new ObservableCollection<Store>(_dbContext.Stores.ToList());

            Authors = new ObservableCollection<Author>(_dbContext.Authors
                .Include(b => b.BookIsbn13s)
                .ToList());
        }
    }

    private void NewAuthor(object obj)
    {
        if (_mainWindowViewModel.IsDBConnected)
        {
            try
            {
                var author = new Author
                {
                    FirstName = "New Author",
                    BirthDate = DateTime.Now
                };

                _dbContext.Authors.Add(author);
                _dbContext.SaveChanges();
                Authors.Add(author);

                _mainWindowViewModel.ShowSuccessSnackbarMessage("Success!", $"Created new author, change name and birthdate");
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

    private void RemoveAuthor(object obj)
    {
        if (_mainWindowViewModel.IsDBConnected)
        {
            try
            {
                _dbContext.Authors.Remove(SelectedAuthor);
                _dbContext.SaveChanges();
                Authors.Remove(SelectedAuthor);


                _mainWindowViewModel.ShowSuccessSnackbarMessage($"Success!", $"Removed {SelectedAuthor}");
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