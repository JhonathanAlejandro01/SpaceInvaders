using System;
using System.Collections.Generic;
using System.Diagnostics;


namespace SpaceInvadersJA.Models;
public class DeployBlocks
{
    private readonly List<Block> blocks;
    private readonly Canvas _canvas;
    private readonly int Columns = 5;
    private readonly int Rows = 1;
    private readonly int Spaces = 145;

    public DeployBlocks(Canvas canvas)
    {
        _canvas = canvas;
        blocks = new List<Block>();
        GenerateBlocks();
    }

    public void GenerateBlocks()
    {
        Debug.WriteLine("🟢 Generando bloques...");

        double canvasWidth = _canvas.ActualWidth;
        double startY = 600;
        double blockWidth = 80;
        double totalBlocksWidth = (Columns * blockWidth) + ((Columns - 1) * Spaces);
        double startX = (canvasWidth - totalBlocksWidth) / 2;

        for (int col = 0; col < Columns; col++)
        {
            double x = startX + col * (blockWidth + Spaces);
            double y = startY;

            var block = new Block(x, y);
            blocks.Add(block);
            _canvas.Children.Add(block.Sprite);

            Debug.WriteLine($"🟢 Bloque añadido en ({x}, {y})");
        }

        Debug.WriteLine($"🟢 Total de bloques creados: {blocks.Count}");
    }

    public List<Block> GetBlocks()
    {
        return blocks;
    }
}

