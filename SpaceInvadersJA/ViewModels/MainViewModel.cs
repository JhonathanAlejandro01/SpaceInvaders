using SpaceInvadersJA.ViewModels;

namespace SpaceInvadersJA.Presentation;

public partial class MainViewModel : ObservableObject
{
    private INavigator _navigator;

    [ObservableProperty]
    private string? name;

    public MainViewModel(INavigator navigator)
    {
        _navigator = navigator;
        Title = "Space Invaders";
        StartGameCommand = new AsyncRelayCommand(StartGame);
        ViewControlsCommand = new AsyncRelayCommand(ViewControls);
    }

    public string Title { get; }
    public ICommand StartGameCommand { get; }
    public ICommand ViewControlsCommand { get; }

    private async Task StartGame()
    {
        await _navigator.NavigateViewModelAsync<GameViewModel>(this);
    }

    private async Task ViewControls()
    {
        await _navigator.NavigateViewModelAsync<ControlsViewModel>(this);
    }

}
