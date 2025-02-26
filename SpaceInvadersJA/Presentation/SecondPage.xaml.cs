using Windows.System;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Controls;
using System.Diagnostics;
using Newtonsoft.Json.Bson;



namespace SpaceInvadersJA.Presentation;

public sealed partial class SecondPage : Page
{
    private double _canvasWidth;
    private double _canvasHeight;
    private PlayerShip player;
    private List<Enemy> enemyShips = new();


    public SecondPage()
    {
        this.InitializeComponent();
        this.Loaded += OnPageLoaded;
        GameCanvas.SizeChanged += OnCanvasSizeChanged;
    }


    private void OnPageLoaded(object sender, RoutedEventArgs e)
    {
        _canvasWidth = GameCanvas.ActualWidth;
        _canvasHeight = GameCanvas.ActualHeight;

        double initialShipX = (_canvasWidth / 2) - (PlayerShip.ShipHeight / 2);
        double initialShipY = _canvasHeight - (PlayerShip.ShipHeight + 20);

        player = new PlayerShip(initialShipX, initialShipY, GameCanvas);
        GameCanvas.Children.Add(player.Sprite);
        GenerateEnemyFormation();
        MainGrid.Focus(FocusState.Programmatic);
    }





    private void Page_KeyDown(object sender, KeyRoutedEventArgs e)
    {
        //Debug.WriteLine($"esta escuchando el teclado e.Key = {e.Key}");
        e.Handled = true;
       // Debug.WriteLine($"esta escuchando el teclado e.OriginalKey = {e.OriginalKey}");
        if (e.OriginalKey == VirtualKey.Left || e.OriginalKey == VirtualKey.A || e.OriginalKey == VirtualKey.J)
        {
           // Debug.WriteLine($"entro en left  VirtualKey.Left = {VirtualKey.Left}");
            player?.MoveLeft();
        }
        else if (e.OriginalKey == VirtualKey.Right || e.OriginalKey == VirtualKey.D || e.OriginalKey == VirtualKey.L)
        {
            //Debug.WriteLine($"entro en left  VirtualKey.Right = {VirtualKey.Right}");
            player?.MoveRight(_canvasWidth);
        }
        else
        {
            Debug.WriteLine($"La tecla {e.OriginalKey} no está mapeada.");
        }
    }

    private void OnCanvasSizeChanged(object sender, SizeChangedEventArgs e)
    {
        _canvasWidth = GameCanvas.ActualWidth;
        _canvasHeight = GameCanvas.ActualHeight;

        if (player != null)
        {
            double newShipX = _canvasWidth / 2 - player.Width / 2;
            double newShipY = _canvasHeight - player.Height;

            player.SetPosition(newShipX, newShipY);

        }
    }

    private void GenerateEnemyFormation()
    {
        int rows = 5; 
        int cols = 10; 
        double spacingX = 50; 
        double spacingY = 40; 
        double startX = (_canvasWidth - (cols * spacingX)) / 2; 
        double startY = 50; 

        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < cols; col++)
            {
                double x = startX + col * spacingX;
                double y = startY + row * spacingY;

                Enemy enemy;

                if (row == 0)
                    enemy = new AdvancedEnemy(x, y); // Front row with special ships
                else if (row == 1 || row == 2)
                    enemy = new MediumEnemy(x, y); // Intermediate rows with advanced enemies
                else
                    enemy = new BasicEnemy(x, y); // Last rows with basic enemies

                enemyShips.Add(enemy);
                GameCanvas.Children.Add(enemy.Sprite);
            }
        }
    }

}
