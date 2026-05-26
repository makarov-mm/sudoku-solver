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




        return model;
    }

    public void Solve()
    {

    }
}