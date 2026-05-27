namespace SudokuSolver;

public static class Utils
{
    public static readonly Random Rnd = new();

    public static IEnumerable<int> GetRandomValues(this HashSet<int> set)
    {
        set = new HashSet<int>(set.ToArray());

        while (set.Count > 0)
        {
            int value = set.ToArray()[Rnd.Next(0, set.Count)];
            set.Remove(value);
            yield return value;
        }
    }
}