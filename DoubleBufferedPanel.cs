namespace SudokuSolver;

public sealed class DoubleBufferedPanel : Panel
{
    public DoubleBufferedPanel()
    {
        DoubleBuffered = true; 
        ResizeRedraw = true;
    }
}