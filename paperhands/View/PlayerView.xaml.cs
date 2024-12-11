using System.Windows.Controls;
using paperhands.ViewModel;

namespace paperhands.View;

public partial class PlayerView : UserControl
{
    public PlayerView(PlayerViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }
}