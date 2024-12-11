using System.Windows.Controls;
using paperhands.ViewModel;

namespace paperhands.View;

/// <summary>
///     Interaction logic for ConfigurationView.xaml
/// </summary>
public partial class ConfigurationView : UserControl
{
    public ConfigurationView(ConfigurationViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }
}