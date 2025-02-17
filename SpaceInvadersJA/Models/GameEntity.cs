using Microsoft.UI.Xaml.Media.Imaging;

namespace SpaceInvadersJA.Models;
public abstract class GameEntity
{
    public Image Sprite { get; }
    private double _positionX;
    private double _positionY;
    private double _widthSprite;
    private double _heightSprite;

    public double PositionX => _positionX;
    public double PositionY => _positionY;
    public double Width => _widthSprite;
    public double Height => _heightSprite;

    public GameEntity(
        string imagePath
        , double initialXPosition
        , double initialYPosition
        , double ParamWidthSpirte
        , double ParamHightSpirte
        )
    {
        _positionX = initialXPosition;
        _positionY = initialYPosition;
        _widthSprite = ParamWidthSpirte;
        _heightSprite = ParamHightSpirte;
        Sprite = new Image { 
            Source = new BitmapImage(new Uri(imagePath, UriKind.RelativeOrAbsolute)),
            Width = ParamWidthSpirte,
            Height = ParamHightSpirte
        };
        Canvas.SetTop(Sprite,_positionY);
        Canvas.SetLeft(Sprite,_positionX);
    }

    public virtual void Move(
        double MovePositionX,
        double MovePositionY
        )
    {
        _positionY += MovePositionY;
        _positionX += MovePositionX;
        Canvas.SetLeft(Sprite, _positionX);
        Canvas.SetTop(Sprite, _positionY);

    }
}
