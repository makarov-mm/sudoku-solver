namespace SudokuSolver;

public static class SudokuRenderer
{
    public static void Draw(Graphics gfx, Size size, int[,]? board)
    {
        gfx.Clear(Color.White);
        if (board is null) return;

        const int minMargin = 3;
        const int minCellSize = 10;
        const int cellCount = 9;

        int minRequiredSize = minCellSize * cellCount + minMargin * (cellCount + 1);
        int minSize = Math.Min(size.Width, size.Height);

        int targetSize = Math.Max(minRequiredSize, minSize);
        float scaleFactor = targetSize / (float)(minCellSize * cellCount + minMargin * (cellCount + 1));
        int targetCellSize = (int)Math.Floor(minCellSize * scaleFactor);
        int targetMarginSize = (int)Math.Floor(minMargin * scaleFactor);

        var offset = new Point(
            (size.Width - targetSize) / 2,
            (size.Height - targetSize) / 2);

        DrawBoard(gfx, offset, cellCount, targetCellSize, targetMarginSize);
    }

    private static void DrawBoard(Graphics gfx, Point offset, int cellCount, int cellSize, int margin)
    {
        var backgroundColor = Color.FromArgb(235, 240, 248);
        var borderColor = Color.FromArgb(70, 130, 200);

        using var backgroundBrush = new SolidBrush(backgroundColor);
        using var borderPen = new Pen(borderColor, 2);

        for (int i = 0; i < cellCount; ++i)
        {
            for (int j = 0; j < cellCount; ++j)
            {
                int x = offset.X + (cellSize + margin) * i + margin;
                int y = offset.Y + (cellSize + margin) * j + margin;

                gfx.FillRectangle(backgroundBrush, x, y, cellSize, cellSize);
                gfx.DrawRectangle(borderPen, x, y, cellSize, cellSize);
            }
        }
    }
}