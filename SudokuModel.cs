namespace SudokuSolver;

/// <summary>
/// The Sudoku board and all of its rules. Holds no UI / drawing state, so it can
/// be unit-tested in isolation. Coordinates are always (row, col): row grows
/// downward, col grows to the right. Empty cells are stored as 0.
/// </summary>
public sealed class SudokuModel
{
    /// <summary>Edge length of one block (3 for a classic 9x9 board).</summary>
    public int BlockSize { get; }

    /// <summary>Edge length of the whole board (9 for a classic board).</summary>
    public int Size => BlockSize * BlockSize;

    private readonly int[,] _cells;
    private readonly bool[,] _given;

    public SudokuModel(int blockSize = 3)
    {
        if (blockSize < 1) throw new ArgumentOutOfRangeException(nameof(blockSize));
        BlockSize = blockSize;
        _cells = new int[Size, Size];
        _given = new bool[Size, Size];
    }

    /// <summary>Value at a cell (0 = empty).</summary>
    public int this[int row, int col] => _cells[row, col];

    /// <summary>True when the cell is part of the fixed puzzle and may not be edited.</summary>
    public bool IsGiven(int row, int col) => _given[row, col];

    /// <summary>
    /// Sets the value of an editable cell. Given cells are left untouched.
    /// A value of 0 clears the cell. Returns false when the edit was rejected.
    /// </summary>
    public bool SetValue(int row, int col, int value)
    {
        if (!InRange(row) || !InRange(col)) return false;
        if (_given[row, col]) return false;
        if (value < 0 || value > Size) return false;
        _cells[row, col] = value;
        return true;
    }

    public void Clear(int row, int col) => SetValue(row, col, 0);

    /// <summary>Clears everything, including the given clues.</summary>
    public void ClearAll()
    {
        Array.Clear(_cells);
        Array.Clear(_given);
    }

    /// <summary>Clears only the values the user (or solver) added, keeping the clues.</summary>
    public void ClearUserEntries()
    {
        for (int r = 0; r < Size; r++)
            for (int c = 0; c < Size; c++)
                if (!_given[r, c])
                    _cells[r, c] = 0;
    }

    /// <summary>Replaces the board with a puzzle; every non-zero cell becomes a fixed clue.</summary>
    public void LoadPuzzle(int[,] puzzle)
    {
        for (int r = 0; r < Size; r++)
            for (int c = 0; c < Size; c++)
            {
                int v = puzzle[r, c];
                _cells[r, c] = v;
                _given[r, c] = v != 0;
            }
    }

    /// <summary>
    /// True when <paramref name="value"/> may legally go into (row, col): no other
    /// cell in the same row, column or block already holds it.
    /// </summary>
    public bool CanPlace(int row, int col, int value)
    {
        if (value == 0) return true;

        for (int c = 0; c < Size; c++)
            if (c != col && _cells[row, c] == value) return false;

        for (int r = 0; r < Size; r++)
            if (r != row && _cells[r, col] == value) return false;

        int br = (row / BlockSize) * BlockSize;
        int bc = (col / BlockSize) * BlockSize;
        for (int r = br; r < br + BlockSize; r++)
            for (int c = bc; c < bc + BlockSize; c++)
                if ((r != row || c != col) && _cells[r, c] == value) return false;

        return true;
    }

    /// <summary>True when the filled value at this cell collides with another cell.</summary>
    public bool IsConflicting(int row, int col)
    {
        int v = _cells[row, col];
        return v != 0 && !CanPlace(row, col, v);
    }

    /// <summary>Every cell whose value currently breaks a Sudoku rule.</summary>
    public HashSet<(int Row, int Col)> FindConflicts()
    {
        var conflicts = new HashSet<(int, int)>();
        for (int r = 0; r < Size; r++)
            for (int c = 0; c < Size; c++)
                if (IsConflicting(r, c))
                    conflicts.Add((r, c));
        return conflicts;
    }

    public bool IsFull()
    {
        for (int r = 0; r < Size; r++)
            for (int c = 0; c < Size; c++)
                if (_cells[r, c] == 0) return false;
        return true;
    }

    public bool IsSolved() => IsFull() && FindConflicts().Count == 0;

    /// <summary>
    /// Solves the current board in place using backtracking. The user's clues and
    /// entries are preserved; only empty cells are filled. Returns false (and leaves
    /// the board unchanged) when there is no solution.
    /// </summary>
    public bool TrySolve()
    {
        if (FindConflicts().Count != 0) return false;

        int[,] work = (int[,])_cells.Clone();
        if (!Backtrack(work)) return false;

        for (int r = 0; r < Size; r++)
            for (int c = 0; c < Size; c++)
                _cells[r, c] = work[r, c];
        return true;
    }

    /// <summary>Counts solutions, stopping early once <paramref name="limit"/> is reached.</summary>
    public int CountSolutions(int limit = 2)
    {
        int[,] work = (int[,])_cells.Clone();
        int count = 0;
        CountBacktrack(work, limit, ref count);
        return count;
    }

    // --- Solver core -------------------------------------------------------

    private bool Backtrack(int[,] g)
    {
        if (!FindBestCell(g, out int row, out int col)) return true; // full -> solved

        for (int v = 1; v <= Size; v++)
        {
            if (CanPlaceIn(g, row, col, v))
            {
                g[row, col] = v;
                if (Backtrack(g)) return true;
                g[row, col] = 0;
            }
        }
        return false;
    }

    private void CountBacktrack(int[,] g, int limit, ref int count)
    {
        if (count >= limit) return;
        if (!FindBestCell(g, out int row, out int col)) { count++; return; }

        for (int v = 1; v <= Size && count < limit; v++)
        {
            if (CanPlaceIn(g, row, col, v))
            {
                g[row, col] = v;
                CountBacktrack(g, limit, ref count);
                g[row, col] = 0;
            }
        }
    }

    /// <summary>
    /// Picks the empty cell with the fewest legal candidates (the MRV heuristic),
    /// which prunes the search tree dramatically. Returns false when the grid is full.
    /// </summary>
    private bool FindBestCell(int[,] g, out int bestRow, out int bestCol)
    {
        bestRow = -1;
        bestCol = -1;
        int bestCount = Size + 1;

        for (int r = 0; r < Size; r++)
            for (int c = 0; c < Size; c++)
            {
                if (g[r, c] != 0) continue;

                int candidates = 0;
                for (int v = 1; v <= Size; v++)
                    if (CanPlaceIn(g, r, c, v)) candidates++;

                if (candidates < bestCount)
                {
                    bestCount = candidates;
                    bestRow = r;
                    bestCol = c;
                    if (bestCount <= 1) return true; // can't do better
                }
            }

        return bestRow != -1;
    }

    private bool CanPlaceIn(int[,] g, int row, int col, int value)
    {
        for (int c = 0; c < Size; c++)
            if (g[row, c] == value) return false;

        for (int r = 0; r < Size; r++)
            if (g[r, col] == value) return false;

        int br = (row / BlockSize) * BlockSize;
        int bc = (col / BlockSize) * BlockSize;
        for (int r = br; r < br + BlockSize; r++)
            for (int c = bc; c < bc + BlockSize; c++)
                if (g[r, c] == value) return false;

        return true;
    }

    // --- Puzzle generation -------------------------------------------------

    /// <summary>
    /// Builds a brand-new puzzle with a single, unique solution and loads it.
    /// <paramref name="clues"/> is the approximate number of filled cells to keep
    /// (lower = harder). Defaults to a comfortable medium board.
    /// </summary>
    public void GeneratePuzzle(int clues = 32)
    {
        int[,] full = BuildFullSolution();

        var positions = new List<(int r, int c)>(Size * Size);
        for (int r = 0; r < Size; r++)
            for (int c = 0; c < Size; c++)
                positions.Add((r, c));
        Utils.Shuffle(positions);

        int filled = Size * Size;
        int target = Math.Clamp(clues, Size, Size * Size);

        foreach (var (r, c) in positions)
        {
            if (filled <= target) break;

            int saved = full[r, c];
            full[r, c] = 0;

            // Removing this clue must keep the solution unique.
            var probe = new SudokuModel(BlockSize);
            probe.LoadPuzzle(full);
            if (probe.CountSolutions(2) != 1)
            {
                full[r, c] = saved; // can't remove without losing uniqueness
            }
            else
            {
                filled--;
            }
        }

        LoadPuzzle(full);
    }

    private int[,] BuildFullSolution()
    {
        var g = new int[Size, Size];
        FillRandom(g);
        return g;
    }

    private bool FillRandom(int[,] g)
    {
        if (!FindBestCell(g, out int row, out int col)) return true;

        var values = new List<int>(Size);
        for (int v = 1; v <= Size; v++) values.Add(v);
        Utils.Shuffle(values);

        foreach (int v in values)
        {
            if (CanPlaceIn(g, row, col, v))
            {
                g[row, col] = v;
                if (FillRandom(g)) return true;
                g[row, col] = 0;
            }
        }
        return false;
    }

    private bool InRange(int i) => i >= 0 && i < Size;
}
