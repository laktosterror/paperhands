﻿using System.Windows;
using System.Windows.Input;
using paperhands.ViewModel;
using Wpf.Ui;

namespace paperhands;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        snackbarService = new SnackbarService();
        snackbarService.SetSnackbarPresenter(SnackbarPresenter);

        DataContext = new MainWindowViewModel(snackbarService);

        ContentGrid.MouseDown += ContentGrid_MouseDown;
    }

    public ISnackbarService snackbarService { get; set; }

    private void ContentGrid_MouseDown(object sender, MouseButtonEventArgs e)
    {
        try
        {
            DragMove();
        }
        catch
        {
            Console.WriteLine("only action on left click!");
        }
    }

    //private void Window_MouseDown(object sender, MouseButtonEventArgs e)
    //{
    //    var viewModel = (MainWindowViewModel)DataContext;
    //}
}