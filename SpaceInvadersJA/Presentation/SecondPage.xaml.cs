using Windows.System;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Controls;
using System.Diagnostics;

namespace SpaceInvadersJA.Presentation
{
    public sealed partial class SecondPage : Page
    {
        private double _canvasWidth;
        private double _canvasHeight;
        private PlayerShip player;
        private const double INITIAL_SHIP_X = 120;
        private const double INITIAL_SHIP_Y = 120;


        public SecondPage()
        {
            this.InitializeComponent();
            this.Loaded += OnPageLoaded;
        }

        private void OnPageLoaded(object sender, RoutedEventArgs e)
        {
            _canvasWidth = GameCanvas.ActualWidth;
            _canvasHeight = GameCanvas.ActualHeight;
            player = new PlayerShip(INITIAL_SHIP_X, INITIAL_SHIP_Y, GameCanvas);
            GameCanvas.Children.Add(player.Sprite);
            MainGrid.Focus(FocusState.Programmatic);
        }

        private void Page_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            Debug.WriteLine($"esta escuchando el teclado e.Key = {e.Key}");
            e.Handled = true;
            Debug.WriteLine($"esta escuchando el teclado e.OriginalKey = {e.OriginalKey}");

            switch (e.OriginalKey)
            {
                case VirtualKey.Left:
                    Debug.WriteLine($"entro en left  VirtualKey.Left = {VirtualKey.Left}");
                    player.MoveLeft();
                    break;

                case VirtualKey.Right:
                    Debug.WriteLine($"entro en left  VirtualKey.Right = {VirtualKey.Right}");

                    player?.MoveRight(_canvasWidth);
                    break;
                default:
                    Debug.WriteLine($"La tecla {e.OriginalKey} no está mapeada.");
                    break;
            }
        }
    }
}
