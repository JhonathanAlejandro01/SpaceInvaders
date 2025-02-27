using Windows.System;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Controls;
using System.Diagnostics;
using Newtonsoft.Json.Bson;



namespace SpaceInvadersJA.Presentation;

public sealed partial class GamePage : Page
{

    private readonly GameViewModel _viewModel;

    public GamePage()
    {
        this.InitializeComponent();
        _viewModel = new GameViewModel();
        this.DataContext = _viewModel;
    }

    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
        base.OnNavigatedTo(e);
        GameCanvas.Loaded += (s, args) =>
        {
            if (GameCanvas.ActualWidth > 0 && GameCanvas.ActualHeight > 0)
            {
                _viewModel.Initialize(GameCanvas);
            }
        };
    }

    protected override void OnNavigatedFrom(NavigationEventArgs e)
    {
        base.OnNavigatedFrom(e);
        _viewModel.StopGame();
    }
    private void Page_KeyDown(object sender, KeyRoutedEventArgs e)
    {
        _viewModel.HandleKeyPress(e);
    }

}
