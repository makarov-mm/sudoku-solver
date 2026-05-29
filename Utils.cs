namespace SudokuSolver;

public static class Utils
{
    public static readonly Random Rnd = new();

    /// <summary>In-place Fisher-Yates shuffle.</summary>
    public static void Shuffle<T>(IList<T> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int j = Rnd.Next(i + 1);
            (list[i], list[j]) = (list[j], list[i]);
        }
    }
}
