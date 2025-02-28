using Microsoft.UI.Xaml.Input;
using Windows.System;
using System.Diagnostics;
using Microsoft.UI.Xaml.Media.Imaging;

namespace SpaceInvadersJA.Presentation;

public partial class GameViewModel : ObservableObject
{
    private PlayerShip player;
    private List<Enemy> enemyShips = new();
    private DeployBlocks _deployBlocks;

    private const int EnemyMoveStepX = 10;
    private const int EnemyMoveStepY = 20;
    private const int MoveDelay = 500;
    private List<Bullet> playerBullets = new();
    private List<Bullet> enemyBullets = new();

    private DispatcherTimer _gameLoopTimer;
    private DispatcherTimer enemyShootingTimer;
    private Canvas _gameCanvas;
    private bool movingRight = true;
    private bool gameRunning = true;
    private double _canvasWidth;
    private double _canvasHeight;
    private Random random = new();

    // Cada 1000 puntos se otorga una vida extra hasta 6
    private int extraLifeThreshold = 1000;

    [ObservableProperty]
    private int score;

    public string ScoreText => $"Score: {Score}";

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

    // Se notifica el cambio de ScoreText cuando cambia el Score.
    partial void OnScoreChanged(int oldValue, int newValue)
    {
        OnPropertyChanged(nameof(ScoreText));
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
                Debug.WriteLine("⚠️ El Canvas aún no está listo. Posponiendo inicialización.");
                _gameCanvas.SizeChanged += (s, e) => Initialize(gameCanvas);
                return;
            }

            InitializePlayer();
            GenerateEnemyFormation();
            MoveEnemiesAsync();
            StartEnemyShooting();
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
        _gameLoopTimer?.Stop();
        enemyShootingTimer?.Stop();
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

    // Solo se dispara si no hay ya una bala del jugador en pantalla
    private void FireBullet()
    {
        if (playerBullets.Any())
            return;

        double bulletX = player.PositionX + (player.Width / 2) - 30;
        double bulletY = player.PositionY - 30;
        var bullet = new Bullet(_gameCanvas, bulletX, bulletY, BulletOwner.Player);
        playerBullets.Add(bullet);
        _ = bullet.MoveUp().ContinueWith(_ =>
        {
            _gameCanvas.DispatcherQueue.TryEnqueue(() =>
            {
                if (_gameCanvas.Children.Contains(bullet.Sprite))
                    _gameCanvas.Children.Remove(bullet.Sprite);
                playerBullets.Remove(bullet);
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

        foreach (var enemy in enemyShips)
        {
            _gameCanvas.Children.Remove(enemy.Sprite);
        }
        enemyShips.Clear();

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
                        break;
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

    private void StartEnemyShooting()
    {
        enemyShootingTimer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(4000) };
        enemyShootingTimer.Tick += EnemyShootingLoop;
        enemyShootingTimer.Start();
    }

    private void EnemyShootingLoop(object sender, object e)
    {
        var advancedEnemies = enemyShips.Where(enemy => enemy is AdvancedEnemy).ToList();
        if (advancedEnemies.Any())
        {
            var selectedEnemy = advancedEnemies[random.Next(advancedEnemies.Count)];
            FireEnemyBullet(selectedEnemy);
        }
    }

    private void FireEnemyBullet(Enemy enemy)
    {
        double bulletX = Canvas.GetLeft(enemy.Sprite) + (enemy.Sprite.Width / 2) - 30;
        double bulletY = Canvas.GetTop(enemy.Sprite) + enemy.Sprite.Height;
        var bullet = new Bullet(_gameCanvas, bulletX, bulletY, BulletOwner.Enemy);
        enemyBullets.Add(bullet);
        _ = bullet.MoveDown().ContinueWith(_ =>
        {
            _gameCanvas.DispatcherQueue.TryEnqueue(() =>
            {
                if (_gameCanvas.Children.Contains(bullet.Sprite))
                    _gameCanvas.Children.Remove(bullet.Sprite);
                enemyBullets.Remove(bullet);
            });
        });
    }

    private void GameLoop(object sender, object e)
    {
        // Colisiones de balas del jugador contra enemigos
        foreach (var bullet in playerBullets.ToList())
        {
            foreach (var enemy in enemyShips.ToList())
            {
                if (CheckCollision(bullet, enemy))
                {
                    _ = ExplodeEnemy(enemy, bullet);
                    break;
                }
            }
        }
        // Colisiones de balas enemigas contra el jugador
        foreach (var bullet in enemyBullets.ToList())
        {
            if (CheckCollision(bullet, player))
            {
                _gameCanvas.Children.Remove(bullet.Sprite);
                enemyBullets.Remove(bullet);
                player.Lives -= 1;
                break;
            }
        }
    }

    private bool CheckCollision(GameEntity a, GameEntity b)
    {
        double aLeft = Canvas.GetLeft(a.Sprite);
        double aTop = Canvas.GetTop(a.Sprite);
        double aRight = aLeft + a.Sprite.Width;
        double aBottom = aTop + a.Sprite.Height;

        double bLeft = Canvas.GetLeft(b.Sprite);
        double bTop = Canvas.GetTop(b.Sprite);
        double bRight = bLeft + b.Sprite.Width;
        double bBottom = bTop + b.Sprite.Height;

        return aLeft < bRight && aRight > bLeft && aTop < bBottom && aBottom > bTop;
    }

    private async Task ExplodeEnemy(Enemy enemy, Bullet bullet)
    {
        // enemy.Sprite.Source = new BitmapImage(new Uri("ms-appx:///Assets/Images/"));
        await Task.Delay(200);
        _gameCanvas.Children.Remove(enemy.Sprite);
        enemyShips.Remove(enemy);

        Score += enemy.ScoreValue;
        if (Score >= extraLifeThreshold && player.Lives < 6)
        {
            player.Lives += 1;
            extraLifeThreshold += 1000;
        }

        if (bullet.Owner == BulletOwner.Player)
        {
            _gameCanvas.Children.Remove(bullet.Sprite);
            playerBullets.Remove(bullet);
        }

        if (!enemyShips.Any())
        {
            GenerateEnemyFormation();
        }
    }
}
