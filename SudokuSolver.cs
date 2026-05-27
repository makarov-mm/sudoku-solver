namespace SudokuSolver;

public partial class SudokuSolver : Form
{
    private SudokuModel? _model;

    public SudokuSolver()
    {
        InitializeComponent();
    }

    private void SudokuSolver_Load(object sender, EventArgs e)
    {
        _model = new SudokuModel();
    }

    private void btnSolve_Click(object sender, EventArgs e)
    {
        _model ??= new SudokuModel();
        _model.Solve();
        pnlMain.Invalidate();
    }

    private void btnReset_Click(object sender, EventArgs e)
    {
        _model = new SudokuModel();
        pnlMain.Invalidate();
    }

    private void btnExample_Click(object sender, EventArgs e)
    {
        _model = SudokuModel.GenerateExample();
        pnlMain.Invalidate();
    }

    private void pnlMain_Paint(object sender, PaintEventArgs e)
    {
        SudokuRenderer.Draw(e.Graphics, pnlMain.ClientSize, _model?.Board, 3, 3);
    }

    private void pnlMain_Resize(object sender, EventArgs e)
    {
        using Graphics gfx = pnlMain.CreateGraphics();
        SudokuRenderer.Draw(gfx, pnlMain.ClientSize, _model?.Board, 3, 3);
    }
}