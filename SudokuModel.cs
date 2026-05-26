using System;
using System.Collections.Generic;
using System.Text;

namespace SudokuSolver;

public sealed class SudokuModel
{
    public const int BoardSize = 9;
    public int[,] Board = new int[BoardSize, BoardSize];

    public static SudokuModel GenerateExample()
    {
        var model = new SudokuModel();

        model.Board = new int[BoardSize, BoardSize]
        {
            { 5, 3, 0, 0, 7, 0, 0, 0, 0 },
            { 6, 0, 0, 1, 9, 5, 0, 0, 0 },
            { 0, 9, 8, 0, 0, 0, 0, 6, 0 },
            { 8, 0, 0, 0, 6, 0, 0, 0, 3 },
            { 4, 0, 0, 8, 0, 3, 0, 0, 1 },
            { 7, 0, 0, 0, 2, 0, 0, 0, 6 },
            { 0, 6, 0, 0, 0, 0, 2, 8, 0 },
            { 0, 0, 0, 4, 1, 9, 0, 0, 5 },
            { 0, 0, 0, 0, 8, 0, 0, 7, 9 }
        };

        return model;
    }

    public void Solve()
    {

    }
}