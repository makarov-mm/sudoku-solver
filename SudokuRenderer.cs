using System.Drawing.Drawing2D;

namespace SudokuSolver;

/// <summary>
/// Stateless GDI+ renderer. All selection / hover state is passed in, so the same
/// renderer can be driven by any caller and there is no hidden global state.
/// It also owns the board geometry (<see cref="Layout"/>) so the form and the
/// painter compute cell rectangles in exactly the same way.
/// </summary>
public static class SudokuRenderer
{
    // Surface
    private static readonly Color PanelBackground = Color.FromArgb(247, 249, 252);
    private static readonly Color BoardBackground = Color.White;

    // Cell highlights (strongest first when several apply)
    private static readonly Color ConflictFill = Color.FromArgb(252, 226, 226);
    private static readonly Color SelectedFill = Color.FromArgb(187, 212, 245);
    private static readonly Color SameValueFill = Color.FromArgb(213, 229, 250);
    private static readonly Color PeerFill = Color.FromArgb(233, 240, 251);
    private static readonly Color HoverFill = Color.FromArgb(240, 245, 252);

    // Lines
    private static readonly Color ThinLine = Color.FromArgb(199, 208, 221);
    private static readonly Color ThickLine = Color.FromArgb(52, 74, 102);

    // Digits
    private static readonly Color GivenText = Color.FromArgb(28, 37, 48);
    private static readonly Color EnteredText = Color.FromArgb(37, 99, 235);
    private static readonly Color ConflictText = Color.FromArgb(200, 30, 30);

    // Number pad
    private static readonly Color PadFill = Color.FromArgb(255, 255, 255);
    private static readonly Color PadHoverFill = Color.FromArgb(225, 236, 250);
    private static readonly Color PadBorder = Color.FromArgb(176, 190, 210);
    private static readonly Color PadText = Color.FromArgb(40, 56, 78);
    private static readonly Color EraseText = Color.FromArgb(200, 70, 70);

    public static void Draw(Graphics g, Layout layout, SudokuModel model,
        (int Row, int Col)? selected, (int Row, int Col)? hoverCell, int? hoverPad)
    {
        g.SmoothingMode = SmoothingMode.AntiAlias;
        g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
        g.Clear(PanelBackground);

        if (layout.CellSize <= 0) return;

        DrawCells(g, layout, model, selected, hoverCell);
        DrawGrid(g, layout, model);
        DrawDigits(g, layout, model);
        DrawPad(g, layout, model, hoverPad);
    }

    private static void DrawCells(Graphics g, Layout layout, SudokuModel model,
        (int Row, int Col)? selected, (int Row, int Col)? hoverCell)
    {
        int n = model.Size;
        int block = model.BlockSize;
        HashSet<(int, int)> conflicts = model.FindConflicts();

        int selValue = 0;
        if (selected is { } s) selValue = model[s.Row, s.Col];

        using var board = new SolidBrush(BoardBackground);
        g.FillRectangle(board, layout.BoardRect);

        for (int r = 0; r < n; r++)
            for (int c = 0; c < n; c++)
            {
                Color? fill = null;

                bool isSelected = selected is { } sel && sel.Row == r && sel.Col == c;
                bool isPeer = selected is { } p &&
                              (p.Row == r || p.Col == c ||
                               (p.Row / block == r / block && p.Col / block == c / block));
                bool sameValue = selValue != 0 && model[r, c] == selValue && !isSelected;
                bool isHover = hoverCell is { } h && h.Row == r && h.Col == c;

                if (conflicts.Contains((r, c))) fill = ConflictFill;
                else if (isSelected) fill = SelectedFill;
                else if (sameValue) fill = SameValueFill;
                else if (isPeer) fill = PeerFill;
                else if (isHover) fill = HoverFill;

                if (fill is { } color)
                {
                    using var brush = new SolidBrush(color);
                    g.FillRectangle(brush, layout.CellRect(r, c));
                }
            }
    }

    private static void DrawGrid(Graphics g, Layout layout, SudokuModel model)
    {
        int n = model.Size;
        int block = model.BlockSize;

        using var thin = new Pen(ThinLine, 1f);
        using var thick = new Pen(ThickLine, 2.6f);

        // Thin lines first.
        for (int i = 0; i <= n; i++)
        {
            if (i % block == 0) continue;
            int x = layout.OriginX + i * layout.CellSize;
            int y = layout.OriginY + i * layout.CellSize;
            g.DrawLine(thin, x, layout.OriginY, x, layout.OriginY + n * layout.CellSize);
            g.DrawLine(thin, layout.OriginX, y, layout.OriginX + n * layout.CellSize, y);
        }

        // Thick block separators and the outer frame on top.
        for (int i = 0; i <= n; i += block)
        {
            int x = layout.OriginX + i * layout.CellSize;
            int y = layout.OriginY + i * layout.CellSize;
            g.DrawLine(thick, x, layout.OriginY, x, layout.OriginY + n * layout.CellSize);
            g.DrawLine(thick, layout.OriginX, y, layout.OriginX + n * layout.CellSize, y);
        }
    }

    private static void DrawDigits(Graphics g, Layout layout, SudokuModel model)
    {
        int n = model.Size;
        HashSet<(int, int)> conflicts = model.FindConflicts();

        float fontSize = layout.CellSize * 0.56f;
        using var givenFont = new Font("Segoe UI", fontSize, FontStyle.Bold, GraphicsUnit.Pixel);
        using var enteredFont = new Font("Segoe UI", fontSize, FontStyle.Regular, GraphicsUnit.Pixel);
        using var givenBrush = new SolidBrush(GivenText);
        using var enteredBrush = new SolidBrush(EnteredText);
        using var conflictBrush = new SolidBrush(ConflictText);
        using var format = new StringFormat
        {
            Alignment = StringAlignment.Center,
            LineAlignment = StringAlignment.Center
        };

        for (int r = 0; r < n; r++)
            for (int c = 0; c < n; c++)
            {
                int value = model[r, c];
                if (value == 0) continue;

                bool given = model.IsGiven(r, c);
                Brush brush = conflicts.Contains((r, c)) ? conflictBrush
                            : given ? givenBrush
                            : enteredBrush;
                Font font = given ? givenFont : enteredFont;

                g.DrawString(value.ToString(), font, brush, layout.CellRect(r, c), format);
            }
    }

    private static void DrawPad(Graphics g, Layout layout, SudokuModel model, int? hoverPad)
    {
        if (layout.PadButtons.Count == 0) return;

        float fontSize = layout.PadButtonSize.Height * 0.42f;
        using var digitFont = new Font("Segoe UI", fontSize, FontStyle.Bold, GraphicsUnit.Pixel);
        using var labelFont = new Font("Segoe UI", fontSize * 0.62f, FontStyle.Bold, GraphicsUnit.Pixel);
        using var digitBrush = new SolidBrush(PadText);
        using var eraseBrush = new SolidBrush(EraseText);
        using var border = new Pen(PadBorder, 1.4f);
        using var format = new StringFormat
        {
            Alignment = StringAlignment.Center,
            LineAlignment = StringAlignment.Center
        };

        foreach ((int value, Rectangle rect) in layout.PadButtons)
        {
            bool hovered = hoverPad == value;
            using var fill = new SolidBrush(hovered ? PadHoverFill : PadFill);
            using GraphicsPath path = RoundedRect(rect, Math.Max(4, rect.Height / 6));
            g.FillPath(fill, path);
            g.DrawPath(border, path);

            if (value == 0)
                g.DrawString("Erase", labelFont, eraseBrush, rect, format);
            else
                g.DrawString(value.ToString(), digitFont, digitBrush, rect, format);
        }
    }

    private static GraphicsPath RoundedRect(Rectangle rect, int radius)
    {
        int d = radius * 2;
        var path = new GraphicsPath();
        path.AddArc(rect.X, rect.Y, d, d, 180, 90);
        path.AddArc(rect.Right - d, rect.Y, d, d, 270, 90);
        path.AddArc(rect.Right - d, rect.Bottom - d, d, d, 0, 90);
        path.AddArc(rect.X, rect.Bottom - d, d, d, 90, 90);
        path.CloseFigure();
        return path;
    }

    /// <summary>
    /// Board + number-pad geometry for a given panel size. Created identically by
    /// the painter and the mouse handlers so hit-testing always matches what is drawn.
    /// </summary>
    public sealed class Layout
    {
        public int OriginX { get; private set; }
        public int OriginY { get; private set; }
        public int CellSize { get; private set; }
        public int BoardN { get; private set; }
        public Rectangle BoardRect { get; private set; }

        public Size PadButtonSize { get; private set; }
        /// <summary>Pad buttons as (value, rect); value 1..N are digits, value 0 is "erase".</summary>
        public List<(int Value, Rectangle Rect)> PadButtons { get; } = new();

        public static Layout Create(Size panel, int boardN, int blockSize)
        {
            const int margin = 18;
            const int gap = 14;

            var layout = new Layout { BoardN = boardN };

            int availW = panel.Width - 2 * margin;
            int availH = panel.Height - 2 * margin;
            if (availW < boardN || availH < boardN) return layout; // too small to draw

            // Reserve one cell-height for the pad row beneath the board:
            //   side + gap + side/N <= availH  ->  side <= (availH - gap) * N/(N+1)
            int byHeight = (int)((availH - gap) * (boardN / (float)(boardN + 1)));
            int side = Math.Min(availW, byHeight);

            int cell = side / boardN;
            if (cell <= 0) return layout;
            side = cell * boardN; // snap to whole cells so grid lines stay sharp

            int totalH = side + gap + cell;
            int ox = margin + (availW - side) / 2;
            int oy = margin + Math.Max(0, (availH - totalH) / 2);

            layout.CellSize = cell;
            layout.OriginX = ox;
            layout.OriginY = oy;
            layout.BoardRect = new Rectangle(ox, oy, side, side);

            // Pad: N digit buttons + 1 erase button, laid out across the board width.
            int count = boardN + 1;
            int spacing = Math.Max(2, cell / 12);
            int btnW = (side - spacing * (count - 1)) / count;
            int btnH = cell;
            int padY = oy + side + gap;
            layout.PadButtonSize = new Size(btnW, btnH);

            for (int i = 0; i < count; i++)
            {
                int value = i < boardN ? i + 1 : 0; // last button is erase
                int bx = ox + i * (btnW + spacing);
                layout.PadButtons.Add((value, new Rectangle(bx, padY, btnW, btnH)));
            }

            return layout;
        }

        public Rectangle CellRect(int row, int col) =>
            new(OriginX + col * CellSize, OriginY + row * CellSize, CellSize, CellSize);

        /// <summary>Returns the (row, col) under a point, or null if it is outside the board.</summary>
        public (int Row, int Col)? HitTestBoard(Point p)
        {
            if (CellSize <= 0) return null;
            if (!BoardRect.Contains(p)) return null;
            int col = (p.X - OriginX) / CellSize;
            int row = (p.Y - OriginY) / CellSize;
            if (row < 0 || row >= BoardN || col < 0 || col >= BoardN) return null;
            return (row, col);
        }

        /// <summary>Returns the pad value under a point (0 = erase, 1..N = digit), or null.</summary>
        public int? HitTestPad(Point p)
        {
            foreach ((int value, Rectangle rect) in PadButtons)
                if (rect.Contains(p)) return value;
            return null;
        }
    }
}
