using System.Windows.Controls;
using paperhands.ViewModel;

namespace paperhands.View;

/// <summary>
///     Interaction logic for ImporterView.xaml
/// </summary>
public partial class AuthorView : UserControl
{
    public AuthorView(AuthorViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }
}