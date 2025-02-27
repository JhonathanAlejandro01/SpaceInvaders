
using Microsoft.UI.Xaml.Shapes;
using Microsoft.UI.Dispatching;
using Microsoft.UI.Xaml.Input;
using Windows.System;
using System.Diagnostics;




namespace SpaceInvadersJA.Presentation;

public partial class GameViewModel : ObservableObject
{
    private PlayerShip player;
    private List<Enemy> enemyShips = new();
    private DeployBlocks _deployBlocks;

    private const int EnemyMoveStepX = 10;
    private const int EnemyMoveStepY = 20;
    private const int MoveDelay = 500;
    private bool bulletInScreen = false;

    private DispatcherTimer _gameLoopTimer;
    private Canvas _gameCanvas;
    private bool movingRight = true;
    private bool gameRunning = true;
    private double _canvasWidth;
    private double _canvasHeight;


    [ObservableProperty]
    private int score;

    public GameViewModel()
    {
        Score = 0;
        var dispatcherQueue = Microsoft.UI.Dispatching.DispatcherQueue.GetForCurrentThread();
        if (dispatcherQueue != null)
        {
            dispatcherQueue.TryEnqueue(() =>
            {
                _gameLoopTimer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(16) };
                _gameLoopTimer.Tick += GameLoop;
                _gameLoopTimer.Start();
            });
        }
    }

    public void Initialize(Canvas gameCanvas)
    {
        try
        {
            _gameCanvas = gameCanvas;
            _canvasWidth = _gameCanvas.ActualWidth;
            _canvasHeight = _gameCanvas.ActualHeight;

            if (_canvasWidth == 0 || _canvasHeight == 0)
            {
                Debug.WriteLine("⚠️ Advertencia: El Canvas aún no está listo. Posponiendo inicialización.");
                _gameCanvas.SizeChanged += (s, e) => Initialize(gameCanvas);
                return;
            }

            InitializePlayer();
            GenerateEnemyFormation();
            MoveEnemiesAsync();
            _deployBlocks = new DeployBlocks(_gameCanvas);
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"❌ Error en Initialize: {ex.Message}");
        }
    }

    public void StopGame()
    {
        gameRunning = false;
    }

    private void InitializePlayer()
    {
        double initialShipX = (_canvasWidth / 2) - (PlayerShip.ShipWidth / 2);
        double initialShipY = _canvasHeight - (PlayerShip.ShipHeight + 20);

        player = new PlayerShip(initialShipX, initialShipY, _gameCanvas);
        _gameCanvas.Children.Add(player.Sprite);
    }

    public void HandleKeyPress(KeyRoutedEventArgs e)
    {
        e.Handled = true;

        if (e.OriginalKey == VirtualKey.Left || e.OriginalKey == VirtualKey.A || e.OriginalKey == VirtualKey.J)
        {
            player?.MoveLeft();
        }
        else if (e.OriginalKey == VirtualKey.Right || e.OriginalKey == VirtualKey.D || e.OriginalKey == VirtualKey.L)
        {
            player?.MoveRight(_canvasWidth);
        }
        else if (e.OriginalKey == VirtualKey.Space)
        {
            FireBullet();
        }
    }

    private void FireBullet()
    {
        if (bulletInScreen) return;

        bulletInScreen = true;

        double bulletX = player.PositionX + (player.Width / 2)- 30;
        double bulletY = player.PositionY - 30;

        Bullet bullet = new Bullet(_gameCanvas, bulletX, bulletY);
        _ = bullet.MoveUp().ContinueWith(_ =>
        {
            _gameCanvas.DispatcherQueue.TryEnqueue(() =>
            {
                bulletInScreen = false; // Cuando la bala desaparezca, permitir disparar otra
            });
        });
    }

    private void GenerateEnemyFormation()
    {
        int rows = 6;
        int cols = 11;
        double spacingX = 60;
        double spacingY = 50;
        double startX = (_canvasWidth - (cols * spacingX)) / 2;
        double startY = 40;

        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < cols; col++)
            {
                double x = startX + col * spacingX;
                double y = startY + row * spacingY;

                Enemy enemy = row switch
                {
                    0 => new MysteryEnemy(x, y),
                    1 => new AdvancedEnemy(x, y),
                    2 or 3 => new MediumEnemy(x, y),
                    _ => new BasicEnemy(x, y)
                };

                enemyShips.Add(enemy);
                _gameCanvas.Children.Add(enemy.Sprite);
            }
        }
    }

    private async void MoveEnemiesAsync()
    {
        try
        {
            while (gameRunning)
            {
                await Task.Delay(MoveDelay);
                bool reachedBorder = false;

                foreach (var enemy in enemyShips)
                {
                    double currentX = Canvas.GetLeft(enemy.Sprite);
                    double newX = movingRight ? currentX + EnemyMoveStepX : currentX - EnemyMoveStepX;

                    if (newX <= 0 || newX >= _canvasWidth - enemy.Sprite.Width)
                    {
                        reachedBorder = true;
                    }
                }

                _gameCanvas.DispatcherQueue.TryEnqueue(() =>
                {
                    if (reachedBorder)
                    {
                        movingRight = !movingRight;
                        foreach (var enemy in enemyShips)
                        {
                            double currentY = Canvas.GetTop(enemy.Sprite);
                            Canvas.SetTop(enemy.Sprite, currentY + EnemyMoveStepY);
                        }
                    }
                    else
                    {
                        foreach (var enemy in enemyShips)
                        {
                            double currentX = Canvas.GetLeft(enemy.Sprite);
                            double newX = movingRight ? currentX + EnemyMoveStepX : currentX - EnemyMoveStepX;
                            Canvas.SetLeft(enemy.Sprite, newX);
                        }
                    }
                });

            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"❌ Error en MoveEnemiesAsync: {ex.Message}");
        }
    }


    private void GameLoop(object sender, object e)
    {
        // añadir la lógica de colisiones
    }

    public string ScoreText => $"Score: {score}";
}
