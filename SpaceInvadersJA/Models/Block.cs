using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.UI.Xaml.Media.Imaging;
using Windows.Foundation;

namespace SpaceInvadersJA.Models;
public class Block : GameEntity
{
    public int Resistence { get; private set; }
    private const double BLOCK_WIDTH = 80;
    private const double BLOCK_HEIGHT = 80;

    public Block(double x, double y)
        : base("ms-appx:///Assets/Images/Meteor1.png", x, y, BLOCK_WIDTH, BLOCK_HEIGHT)
    {
        Resistence = 20;
        Debug.WriteLine($"🟢 Bloque creado en {x}, {y} con imagen Meteor1.png");
    }

    public void UpdateBlock()
    {
        switch (Resistence)
        {
            case 15:
                Sprite.Source = new BitmapImage(new Uri("ms-appx:///Assets/Images/Meteor4.png"));
                break;
            case 10:
                Sprite.Source = new BitmapImage(new Uri("ms-appx:///Assets/Images/Meteor3.png"));
                break;
            case 5:
                Sprite.Source = new BitmapImage(new Uri("ms-appx:///Assets/Images/Block_1.png"));
                break;
        }
    }

    public void TakeDamage(int damage)
    {
        Resistence -= damage;
        if (Resistence <= 0)
        {
            // add logic to remove the block from the game.
        }
        else
        {
            UpdateBlock();
        }
    }

    public Rect GetBounds()
    {
        return new Rect(PositionX, PositionY, Width, Height);
    }
}

