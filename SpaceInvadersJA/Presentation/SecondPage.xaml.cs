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

    private BasicEnemy basicEnemy;

    private MediumEnemy mediumEnemy;

    private AdvancedEnemy advancedEnemy;

    private MysteryShip mysteryShip;





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

        double initialShipY = _canvasHeight - PlayerShip.ShipHeight;



        player = new PlayerShip(initialShipX, initialShipY, GameCanvas);

        basicEnemy = new BasicEnemy(200, 200);

        mediumEnemy = new MediumEnemy(300, 300);

        advancedEnemy = new AdvancedEnemy(250, 250);

        mysteryShip = new MysteryShip(350, 350);

        GameCanvas.Children.Add(player.Sprite);

        GameCanvas.Children.Add(basicEnemy.Sprite);

        GameCanvas.Children.Add(mediumEnemy.Sprite);

        GameCanvas.Children.Add(advancedEnemy.Sprite);

        GameCanvas.Children.Add(mysteryShip.Sprite);

        MainGrid.Focus(FocusState.Programmatic);

    }



    private void Page_KeyDown(object sender, KeyRoutedEventArgs e)

    {

        Debug.WriteLine($"esta escuchando el teclado e.Key = {e.Key}");

        e.Handled = true;

        Debug.WriteLine($"esta escuchando el teclado e.OriginalKey = {e.OriginalKey}");



        if (e.OriginalKey == VirtualKey.Left || e.OriginalKey == VirtualKey.A || e.OriginalKey == VirtualKey.J)

        {

            Debug.WriteLine($"entro en left  VirtualKey.Left = {VirtualKey.Left}");

            player?.MoveLeft();



        }

        else if (e.OriginalKey == VirtualKey.Right || e.OriginalKey == VirtualKey.D || e.OriginalKey == VirtualKey.L)

        {

            Debug.WriteLine($"entro en left  VirtualKey.Right = {VirtualKey.Right}");

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

}
