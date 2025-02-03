using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Shapes;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.UI.Dispatching;

namespace SpaceInvadersJA.Presentation
{
    public partial class SecondViewModel : ObservableObject
    {
        private const int PlayerSpeed = 10;  // Velocidad del jugador
        private const int BulletSpeed = 5;   // Velocidad del disparo
        private DispatcherTimer _gameLoopTimer;
        private Canvas _gameCanvas;
        private Rectangle _playerShip;
        private double _playerX = 200;

        [ObservableProperty]
        private int score;

        public SecondViewModel()
        {
            Score = 0;
            DispatcherQueue.GetForCurrentThread()?.TryEnqueue(() =>
            {
                _gameLoopTimer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(16) };
                _gameLoopTimer.Tick += GameLoop;
                _gameLoopTimer.Start();
            });
        }

        public void Initialize(Canvas gameCanvas, Rectangle playerShip)
        {
            _gameCanvas = gameCanvas;
            _playerShip = playerShip;
            Canvas.SetLeft(_playerShip, _playerX);
            Canvas.SetTop(_playerShip, 400);
        }

        public void MovePlayer(string direction)
        {
            if (_gameCanvas == null || _playerShip == null)
                return;

            double newX = Canvas.GetLeft(_playerShip);

            if (direction == "Left" && newX > 0)
                newX -= PlayerSpeed;
            if (direction == "Right" && newX < _gameCanvas.ActualWidth - _playerShip.Width)
                newX += PlayerSpeed;

            System.Diagnostics.Debug.WriteLine($"Moviendo nave a: {newX}");

            Canvas.SetLeft(_playerShip, newX);
        }


        public void FireBullet()
        {
            Rectangle bullet = new Rectangle
            {
                Width = 5,
                Height = 15,
                Fill = new SolidColorBrush(Microsoft.UI.Colors.Red)
            };

            _gameCanvas.Children.Add(bullet);
            Canvas.SetLeft(bullet, _playerX + 17);
            Canvas.SetTop(bullet, 385);

            Task.Run(async () =>
            {
                while (Canvas.GetTop(bullet) > 0)
                {
                    await Task.Delay(30);
                    _gameCanvas.DispatcherQueue.TryEnqueue(() =>
                    {
                        Canvas.SetTop(bullet, Canvas.GetTop(bullet) - BulletSpeed);
                    });
                }
                _gameCanvas.DispatcherQueue.TryEnqueue(() =>
                {
                    _gameCanvas.Children.Remove(bullet);
                });
            });
        }


        private void GameLoop(object sender, object e)
        {
            // añadir la lógica de colisiones
        }
    }
}
