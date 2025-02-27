using Microsoft.UI.Xaml.Controls;

namespace SpaceInvadersJA.Models;

public class Bullet : GameEntity
{
    private const double BulletSpeed = 5;
    private readonly Canvas _gameCanvas;
    private const double BULLET_WIDTH = 60;
    private const double BULLET_HEIGHT = 90;

    public Bullet(Canvas gameCanvas, double startX, double startY)
        : base("ms-appx:///Assets/Images/misil2.gif", startX, startY, BULLET_WIDTH, BULLET_HEIGHT)
    {
        _gameCanvas = gameCanvas;
        _gameCanvas.Children.Add(Sprite);
    }

    public async Task MoveUp()
    {
        while (PositionY > 0)
        {
            await Task.Delay(30);
            _gameCanvas.DispatcherQueue.TryEnqueue(() =>
            {
                Move(0, -BulletSpeed);
            });
        }


        _gameCanvas.DispatcherQueue.TryEnqueue(() =>
        {
            _gameCanvas.Children.Remove(Sprite);
        });
    }
}
