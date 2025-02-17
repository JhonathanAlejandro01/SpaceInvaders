
using System.Diagnostics;

namespace SpaceInvadersJA.Models;
public class PlayerShip : GameEntity
{
    private double _speedMove;
    private Canvas _canvas;
    private const double SHIP_WIDTH = 50;
    private const double SHIP_HEIGHT = 50;


    public PlayerShip(
        double initialXPosition,
        double initialYPosition,
        Canvas ParamCanvas
        ): base("ms-appx:///Assets/Images/ship.png", initialXPosition, initialYPosition, SHIP_WIDTH, SHIP_HEIGHT)
    {
        _canvas = ParamCanvas;
        _speedMove = 10;
    }

    public void MoveLeft()
    {
        Debug.WriteLine($"move left dentro de PlayerShip PositionX = {PositionX}");
        const int MARGIN_LEFT = 10;
        if (PositionX > MARGIN_LEFT)
        {
            base.Move(-(_speedMove), 0);
        }
    }
    public void MoveRight(double ParamCanvasWidth)
    {
        //Debug.WriteLine($"move right dentro de PlayerShip PositionX = {PositionX}");
        //Debug.WriteLine($"move right dentro de PlayerShip ParamCanvasWidth = {ParamCanvasWidth}");
        //Debug.WriteLine($"move right dentro de PlayerShip (PositionX + Width) = {(PositionX + Width)}");
        Debug.WriteLine($"move right dentro de PlayerShip PositionX = {PositionX}");
        Debug.WriteLine($"move right dentro de PlayerShip _speedMove = {_speedMove}");
        Debug.WriteLine($"move right dentro de PlayerShip Width = {Width}");

        Debug.WriteLine($"move right dentro de PlayerShip (PositionX + _speedMove + Width) = {(PositionX + _speedMove + Width)}");
        Debug.WriteLine($"move right dentro de PlayerShip ParamCanvasWidth = {ParamCanvasWidth}");

        // Margen de 10 píxeles a la derecha
        const double marginRight = 10;

        // Verifica que la nave no sobrepase el borde derecho de la pantalla con margen
        if ((PositionX + _speedMove + Width) < (ParamCanvasWidth - marginRight))
        {
            Debug.WriteLine("se movio");
            base.Move(_speedMove, 0);
        }
    }
}
