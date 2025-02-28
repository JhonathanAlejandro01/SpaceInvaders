using Microsoft.UI.Xaml.Controls;
using System.Threading.Tasks;

namespace SpaceInvadersJA.Models;

public enum BulletOwner { Player, Enemy }

public class Bullet : GameEntity
{
    private const double BulletSpeed = 5;
    private readonly Canvas _gameCanvas;
    private const double BULLET_WIDTH = 60;
    private const double BULLET_HEIGHT = 90;
    public BulletOwner Owner { get; }

    public Bullet(Canvas gameCanvas, double startX, double startY, BulletOwner owner = BulletOwner.Player)
        : base("ms-appx:///Assets/Images/misil2.gif", startX, startY, BULLET_WIDTH, BULLET_HEIGHT)
    {
        _gameCanvas = gameCanvas;
        Owner = owner;
        _gameCanvas.Children.Add(Sprite);
    }

    public async Task MoveUp()
    {
        while (PositionY > -BULLET_HEIGHT)
        {
            await Task.Delay(30);
            _gameCanvas.DispatcherQueue.TryEnqueue(() => Move(0, -BulletSpeed));
        }
    }

    public async Task MoveDown()
    {
        while (PositionY < _gameCanvas.ActualHeight)
        {
            await Task.Delay(30);
            _gameCanvas.DispatcherQueue.TryEnqueue(() => Move(0, BulletSpeed));
        }
    }
}
