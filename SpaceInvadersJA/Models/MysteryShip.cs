

namespace SpaceInvadersJA.Models;
public class MysteryShip : Enemy
{
    private const double SHIP_WIDTH = 60;
    private const double SHIP_HEIGHT = 60;
    private static readonly Random _random = new Random();

    public MysteryShip(double x, double y)
        : base("ms-appx:///Assets/Images/mystery.gif", x, y, SHIP_WIDTH, SHIP_HEIGHT, _random.Next(50, 300))
    {
        
    }
}

