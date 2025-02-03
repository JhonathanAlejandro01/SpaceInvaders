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
        ViewScoreCommand = new AsyncRelayCommand(ViewScore);
    }

    public string Title { get; }
    public ICommand StartGameCommand { get; }
    public ICommand ViewScoreCommand { get; }

    private async Task StartGame()
    {
        await _navigator.NavigateViewModelAsync<SecondViewModel>(this);
    }

    private async Task ViewScore()
    {
        await _navigator.NavigateViewModelAsync<ScoreViewModel>(this);
    }

}
