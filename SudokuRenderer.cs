namespace SudokuSolver;

public static class SudokuRenderer
{
    public static void Draw(Graphics gfx, Size size, int[,]? board, int blocksCount, int cellsInBlockCount)
    {
        gfx.Clear(Color.White);
        if (board is null) return;

        List<List<Rectangle>> blocks = CalculateBlocks(size, blocksCount, cellsInBlockCount);

        DrawBoard(gfx, blocks);
        DrawValues(gfx, blocks, board);
    }

    private static void DrawBoard(Graphics gfx, IEnumerable<IEnumerable<Rectangle>> blocks)
    {
        var backgroundColor = Color.FromArgb(235, 240, 248);
        var borderColor = Color.FromArgb(70, 130, 200);

        using var backgroundBrush = new SolidBrush(backgroundColor);
        using var borderPen = new Pen(borderColor, 2);

        foreach (List<Rectangle> block in blocks)
        {
            foreach (Rectangle cell in block)
            {
                gfx.FillRectangle(backgroundBrush, cell);
                gfx.DrawRectangle(borderPen, cell);
            }
        }
    }

    private static void DrawValues(Graphics gfx, List<List<Rectangle>> blocks, int[,] board)
    {
        var fontColor = Color.FromArgb(30, 60, 100);
        using var brush = new SolidBrush(fontColor);

        for (int i = 0; i < blocks.Count; ++i)
        {
            for (int j = 0; j < blocks[i].Count; ++j)
            {
                Rectangle cell = blocks[i][j];
                int value = board[i, j];
                string text = $"{board[i, j]}";
                using var font = new Font("Segoe UI", cell.Width / 3f, FontStyle.Bold, GraphicsUnit.Pixel);
                SizeF textSize = gfx.MeasureString(text, font);
                var pos = new PointF(cell.X + (cell.Width - textSize.Width) / 2f, cell.Y + (cell.Height - textSize.Height) / 2f);

                if (value != 0)
                {
                    gfx.DrawString(text, font, brush, pos);
                }
            }
        }
    }

    public static List<List<Rectangle>> CalculateBlocks(Size size, int blocksCount, int cellsInBlockCount)
    {
        const int minMargin = 3;
        const int minCellSize = 10;

        int cellCount = blocksCount * cellsInBlockCount;
        int minRequiredSize = minCellSize * cellCount + minMargin * (cellCount + 1);
        int minSize = Math.Min(size.Width, size.Height);

        int targetSize = Math.Max(minRequiredSize, minSize);
        float scaleFactor = targetSize / (float)(minCellSize * cellCount + minMargin * (cellCount + 1));
        int targetCellSize = (int)Math.Floor(minCellSize * scaleFactor);
        int targetMarginSize = (int)Math.Floor(minMargin * scaleFactor);

        var offset = new Point(
            (size.Width - targetSize) / 2,
            (size.Height - targetSize) / 2);

        var result = new List<List<Rectangle>>();
        int blockSize = cellsInBlockCount * (targetCellSize + targetMarginSize);

        for (int i = 0; i < blocksCount; ++i)
        {
            for (int j = 0; j < blocksCount; ++j)
            {
                var block = new List<Rectangle>();
                int blockOffsetX = i * blockSize;
                int blockOffsetY = j * blockSize;

                for (int ii = 0; ii < cellsInBlockCount; ++ii)
                {
                    for (int jj = 0; jj < cellsInBlockCount; ++jj)
                    {
                        int x = offset.X + blockOffsetX + (targetCellSize + targetMarginSize) * ii + targetMarginSize;
                        int y = offset.Y + blockOffsetY + (targetCellSize + targetMarginSize) * jj + targetMarginSize;
                        var cell = new Rectangle(x, y, targetCellSize, targetCellSize);
                        block.Add(cell);
                    }
                }

                result.Add(block);
            }
        }

        return result;
    }
}