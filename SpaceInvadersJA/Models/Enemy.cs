

namespace SpaceInvadersJA.Models;
public abstract class Enemy : GameEntity
{
    public int ScoreValue { get; protected set; }

    public Enemy(string imagePath, double initialX, double initialY, double width, double height, int score)
        : base(imagePath, initialX, initialY, width, height)
    {
        ScoreValue = score;
    }
}

