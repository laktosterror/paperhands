using System.Windows.Controls;
using paperhands.ViewModel;

namespace paperhands.View;

/// <summary>
///     Interaction logic for ImporterView.xaml
/// </summary>
public partial class BookView : UserControl
{
    public BookView(BookViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }
}