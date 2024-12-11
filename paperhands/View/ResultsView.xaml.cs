using System.Windows.Controls;
using paperhands.ViewModel;

namespace paperhands.View;

/// <summary>
///     Interaction logic for ResultsView.xaml
/// </summary>
public partial class ResultsView : UserControl
{
    public ResultsView(PlayerViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }
}