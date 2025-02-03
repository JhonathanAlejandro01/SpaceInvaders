using Microsoft.UI.Xaml.Input;

namespace SpaceInvadersJA.Presentation;

public sealed partial class SecondPage : Page
{
    private readonly SecondViewModel _viewModel;

    public SecondPage()
    {
        this.InitializeComponent();
        _viewModel = new SecondViewModel();
        DataContext = _viewModel;
        this.Loaded += SecondPage_Loaded;
    }

    private void SecondPage_Loaded(object sender, RoutedEventArgs e)
    {
        MainGrid.KeyDown += OnKeyDown;
        MainGrid.Focus(FocusState.Programmatic);
        _viewModel.Initialize(GameCanvas, PlayerShip);
    }

    private void OnKeyDown(object sender, KeyRoutedEventArgs e)
    {
        System.Diagnostics.Debug.WriteLine($"Tecla presionada: {e.Key}");
        switch (e.Key)
        {
            case Windows.System.VirtualKey.Left:
                _viewModel.MovePlayer("Left");
                break;
            case Windows.System.VirtualKey.Right:
                _viewModel.MovePlayer("Right");
                break;
            case Windows.System.VirtualKey.Space:
                _viewModel.FireBullet();
                break;
        }
    }
}

