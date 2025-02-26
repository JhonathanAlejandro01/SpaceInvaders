

namespace SpaceInvadersJA.Models;
public class AdvancedEnemy : Enemy
{
    private const double SHIP_WIDTH = 60;
    private const double SHIP_HEIGHT = 60;
    private const int SCORE = 40;
    public AdvancedEnemy(double x, double y)
        : base("ms-appx:///Assets/Images/AdvancedEnemy.gif", x, y, SHIP_WIDTH, SHIP_HEIGHT, SCORE) { }
}

