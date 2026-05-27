namespace SudokuSolver;

public sealed class SudokuModel
{
    public const int BlockSize = 3;
    public const int BlocksCount = 3;
    public int[,] Board = new int[BlocksCount * BlockSize, BlocksCount * BlockSize];

    public static SudokuModel GenerateExample()
    {
        var model = new SudokuModel();

        for (int i = 0; i < BlocksCount * BlockSize; ++i)
        {
            for (int j = 0; j < BlocksCount * BlockSize; ++j)
            {
                if (Utils.Rnd.Next(0, 2) == 0)
                {
                    foreach (int value in GenValues(BlocksCount * BlockSize).ToHashSet().GetRandomValues())
                    {
                        if (model.IsValid(i, j, value))
                        {
                            var tmp = (int[,])model.Board.Clone();

                            model.Board[i, j] = value;
                            model.Solve();

                            if (model.IsSolved())
                            {
                                model.Board = tmp;
                                model.Board[i, j] = value;
                                break;
                            }

                            model.Board = tmp;
                        }
                    }
                }
            }
        }

        return model;
    }

    public void Solve()
    {
        var field = new HashSet<int>[BlocksCount * BlockSize, BlocksCount * BlockSize];

        for (int i = 0; i < BlocksCount * BlockSize; ++i)
        {
            for (int j = 0; j < BlocksCount * BlockSize; ++j)
            {
                if (Board[i, j] == 0)
                {
                    field[i, j] = GenValues(BlocksCount * BlockSize).ToHashSet();
                }
                else
                {
                    field[i, j] = [Board[i, j]];
                }
            }
        }

        bool changed = true;

        while (changed)
        {
            changed = false;

            for (int i = 0; i < BlocksCount * BlockSize; ++i)
            {
                for (int j = 0; j < BlocksCount * BlockSize; ++j)
                {
                    HashSet<int> set = field[i, j];
                    var updated = new HashSet<int>();

                    foreach (int value in set)
                    {
                        if (IsValid(i, j, value))
                        {
                            updated.Add(value);
                        }
                        else
                        {
                            changed = true;
                        }
                    }

                    if (changed)
                    {
                        field[i, j] = updated;
                    }
                }
            }

            for (int i = 0; i < BlocksCount * BlockSize; ++i)
            {
                for (int j = 0; j < BlocksCount * BlockSize; ++j)
                {
                    if (Board[i, j] == 0 && field[i, j].Count == 1)
                    {
                        Board[i, j] = field[i, j].First();
                        changed = true;
                    }
                }
            }

            if (!changed)
            {
                for (int i = 0; i < BlocksCount * BlockSize && !changed; ++i)
                {
                    for (int j = 0; j < BlocksCount * BlockSize && !changed; ++j)
                    {
                        if (Board[i, j] == 0)
                        {
                            HashSet<int> set = field[i, j].GetRandomValues().ToHashSet();

                            foreach (int value in set)
                            {
                                if (IsValid(i, j, value))
                                {
                                    Board[i, j] = value;
                                    field[i, j].Remove(value);
                                    changed = true;
                                    break;
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    public bool IsSolved()
    {
        if (!IsValid())
        {
            return false;
        }

        for (int i = 0; i < BlocksCount * BlockSize; ++i)
        {
            for (int j = 0; j < BlocksCount * BlockSize; ++j)
            {
                if (Board[i, j] == 0) return false;
                if (!IsValid(i, j, Board[i, j])) return false;
            }
        }

        return true;
    }

    public bool IsValid()
    {
        for (int i = 0; i < BlocksCount * BlockSize; ++i)
        {
            for (int j = 0; j < BlocksCount * BlockSize; ++j)
            {
                if (!IsValid(i, j, Board[i, j])) return false;
            }
        }

        return true;
    }

    private bool IsValid(int x, int y, int value)
    {
        if (x is < 0 or >= BlocksCount * BlockSize) return false;
        if (y is < 0 or >= BlocksCount * BlockSize) return false;

        for (int i = 0; i < BlocksCount * BlockSize; ++i)
        {
            if (i != x && Board[i, y] == value) return false;
        }

        for (int j = 0; j < BlocksCount * BlockSize; ++j)
        {
            if (j != y && Board[x, j] == value) return false;
        }

        int blockX = (int)Math.Floor(x / (float)BlockSize);
        int blockY = (int)Math.Floor(y / (float)BlockSize);

        for (int i = blockX * BlockSize; i < ((blockX + 1) * BlockSize) && i < BlocksCount * BlockSize; ++i)
        {
            for (int j = blockY * BlockSize; j < ((blockY + 1) * BlockSize) && j < BlocksCount * BlockSize; ++j)
            {
                if (i != x && j != y && Board[i, j] == value) return false;
            }
        }

        return true;
    }

    private static IEnumerable<int> GenValues(int count)
    {
        for (int i = 1; i <= count; ++i)
        {
            yield return i;
        }
    }
}
