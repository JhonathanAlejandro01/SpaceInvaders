

namespace SpaceInvadersJA.Models;
public class MediumEnemy: Enemy
{
    private const double SHIP_WIDTH = 60;
    private const double SHIP_HEIGHT = 60;
    private const int SCORE = 20;
    public MediumEnemy(double x, double y)
        : base("ms-appx:///Assets/Images/mediumEnemy.gif", x, y, SHIP_WIDTH, SHIP_HEIGHT, SCORE) { }
}

