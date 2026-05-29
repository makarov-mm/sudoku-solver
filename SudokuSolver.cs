namespace SudokuSolver;

public partial class SudokuSolver : Form
{
    private SudokuModel _model = new();

    private (int Row, int Col)? _selected;
    private (int Row, int Col)? _hoverCell;
    private int? _hoverPad;

    public SudokuSolver()
    {
        InitializeComponent();
        cmbDifficulty.SelectedIndex = 1; // Medium
    }

    private SudokuRenderer.Layout CurrentLayout() =>
        SudokuRenderer.Layout.Create(pnlMain.ClientSize, _model.Size, _model.BlockSize);

    private void SudokuSolver_Load(object sender, EventArgs e)
    {
        NewPuzzle();
    }

    // --- Painting ----------------------------------------------------------

    private void pnlMain_Paint(object sender, PaintEventArgs e)
    {
        SudokuRenderer.Draw(e.Graphics, CurrentLayout(), _model, _selected, _hoverCell, _hoverPad);
    }

    // --- Mouse -------------------------------------------------------------

    private void pnlMain_MouseClick(object sender, MouseEventArgs e)
    {
        SudokuRenderer.Layout layout = CurrentLayout();

        if (layout.HitTestPad(e.Location) is { } padValue)
        {
            ApplyValue(padValue);
            return;
        }

        if (layout.HitTestBoard(e.Location) is { } cell)
        {
            _selected = (_selected is { } s && s == cell) ? null : cell;
            pnlMain.Invalidate();
        }
    }

    private void pnlMain_MouseMove(object sender, MouseEventArgs e)
    {
        SudokuRenderer.Layout layout = CurrentLayout();
        (int, int)? cell = layout.HitTestBoard(e.Location);
        int? pad = layout.HitTestPad(e.Location);

        if (cell != _hoverCell || pad != _hoverPad)
        {
            _hoverCell = cell;
            _hoverPad = pad;
            pnlMain.Invalidate(); // only repaint when the hover target actually changes
        }
    }

    private void pnlMain_MouseLeave(object sender, EventArgs e)
    {
        if (_hoverCell is null && _hoverPad is null) return;
        _hoverCell = null;
        _hoverPad = null;
        pnlMain.Invalidate();
    }

    private void pnlMain_Resize(object sender, EventArgs e) => pnlMain.Invalidate();

    // --- Keyboard ----------------------------------------------------------

    private void SudokuSolver_KeyDown(object sender, KeyEventArgs e)
    {
        bool handled = true;

        switch (e.KeyCode)
        {
            case >= Keys.D1 and <= Keys.D9:
                ApplyValue(e.KeyCode - Keys.D0);
                break;
            case >= Keys.NumPad1 and <= Keys.NumPad9:
                ApplyValue(e.KeyCode - Keys.NumPad0);
                break;
            case Keys.D0 or Keys.NumPad0 or Keys.Delete or Keys.Back:
                ApplyValue(0);
                break;
            case Keys.Left: MoveSelection(0, -1); break;
            case Keys.Right: MoveSelection(0, 1); break;
            case Keys.Up: MoveSelection(-1, 0); break;
            case Keys.Down: MoveSelection(1, 0); break;
            case Keys.Escape: _selected = null; pnlMain.Invalidate(); break;
            default: handled = false; break;
        }

        if (handled)
        {
            e.Handled = true;
            e.SuppressKeyPress = true; // stop the system "ding" and button navigation
        }
    }

    private void MoveSelection(int dRow, int dCol)
    {
        int n = _model.Size;
        if (_selected is not { } s)
        {
            _selected = (0, 0);
        }
        else
        {
            int r = Math.Clamp(s.Row + dRow, 0, n - 1);
            int c = Math.Clamp(s.Col + dCol, 0, n - 1);
            _selected = (r, c);
        }
        pnlMain.Invalidate();
    }

    /// <summary>Writes <paramref name="value"/> (0 = clear) into the selected cell, if editable.</summary>
    private void ApplyValue(int value)
    {
        if (_selected is not { } s) return;
        if (_model.IsGiven(s.Row, s.Col)) return;
        if (value > _model.Size) return;

        _model.SetValue(s.Row, s.Col, value);
        UpdateStatus();
        pnlMain.Invalidate();
    }

    // --- Buttons -----------------------------------------------------------

    private void btnSolve_Click(object sender, EventArgs e)
    {
        if (_model.FindConflicts().Count != 0)
            lblStatus.Text = "Fix the highlighted conflicts first.";
        else if (_model.TrySolve())
            lblStatus.Text = "Solved.";
        else
            lblStatus.Text = "This board has no solution.";

        pnlMain.Invalidate();
    }

    private void btnClear_Click(object sender, EventArgs e)
    {
        _model.ClearUserEntries();
        lblStatus.Text = string.Empty;
        pnlMain.Invalidate();
    }

    private void btnNew_Click(object sender, EventArgs e) => NewPuzzle();

    private void NewPuzzle()
    {
        int clues = cmbDifficulty.SelectedIndex switch
        {
            0 => 42, // Easy
            2 => 28, // Hard
            3 => 24, // Expert
            _ => 34, // Medium
        };

        UseWaitCursor = true;
        try
        {
            var model = new SudokuModel();
            model.GeneratePuzzle(clues);
            _model = model;
            _selected = null;
            _hoverCell = null;
            _hoverPad = null;
            lblStatus.Text = string.Empty;
        }
        finally
        {
            UseWaitCursor = false;
        }

        pnlMain.Invalidate();
    }

    private void UpdateStatus()
    {
        if (_model.IsSolved())
            lblStatus.Text = "Solved.";
        else if (_model.FindConflicts().Count != 0)
            lblStatus.Text = "There are conflicts on the board.";
        else
            lblStatus.Text = string.Empty;
    }

    private void cmbDifficulty_SelectedIndexChanged(object sender, EventArgs e)
    {
        // Selecting a difficulty does not regenerate automatically; the user presses "New".
    }
}
