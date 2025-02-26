

namespace SpaceInvadersJA.Models;
public class BasicEnemy : Enemy
{
    private const double SHIP_WIDTH = 60;
    private const double SHIP_HEIGHT = 60;
    private const int SCORE = 10;
    public BasicEnemy(double x, double y)
        : base("ms-appx:///Assets/Images/basicEnemy.gif", x, y, SHIP_WIDTH, SHIP_HEIGHT, SCORE) { }
}

