using System.Windows.Controls;
using paperhands.ViewModel;

namespace paperhands.View;

/// <summary>
///     Interaction logic for ImporterView.xaml
/// </summary>
public partial class ImporterView : UserControl
{
    public ImporterView(ImporterViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }
}