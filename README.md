# Sudoku

A small WinForms Sudoku player and solver. The board is rendered with GDI+ on a
custom double-buffered panel. You play entirely with the mouse (click a cell,
then click a number on the on-screen pad), and the keyboard works too.

## Screenshot
![Screenshot](screenshots/screenshot.png)

## Controls

- **Click a cell** to select it. Click it again to deselect.
- **Click a number** on the pad below the board to enter it, or **Erase** to clear the cell.
- **Keyboard:** `1`–`9` enter a value, `0` / `Delete` / `Backspace` clear it,
  arrow keys move the selection, `Esc` deselects.
- Given clues are fixed and cannot be changed; your entries are shown in blue.
- Cells that break a Sudoku rule are highlighted in red as you type.

## Buttons

- **Solve** – completes the current board (or reports that it has no solution).
- **Clear** – removes your entries but keeps the puzzle's clues.
- **New puzzle** – generates a fresh puzzle with a single, unique solution at the
  selected difficulty.

## Build & run

```
dotnet run --project SudokuSolver.csproj
```

Targets `net9.0-windows`.

## Structure

- `SudokuModel.cs` – the board and all rules: validity checks, conflict
  detection, a backtracking solver (with an MRV heuristic), and a unique-solution
  puzzle generator. Has no UI dependencies, so it is unit-testable on any OS.
- `SudokuRenderer.cs` – a stateless GDI+ renderer plus the board/number-pad
  geometry (`Layout`), which the form reuses for hit-testing so what you click
  always matches what is drawn.
- `SudokuSolver.cs` / `.Designer.cs` – the form: wires mouse, number pad and
  keyboard to the model and repaints via `Invalidate` only.
- `DoubleBufferedPanel.cs` – flicker-free drawing surface.
- `Utils.cs` – shared RNG and a Fisher–Yates shuffle.

## Notes on the rewrite

The original had a few correctness problems that this version fixes:

- Digits and the selection highlight were indexed by *block / cell-in-block*
  instead of *row / column*, so values rendered in the wrong cells and the
  highlight landed on the wrong square.
- The in-block duplicate check skipped any cell sharing a row *or* column with
  the candidate, so several block conflicts went undetected.
- There was no keyboard handling, so values could not actually be entered.
- The solver used random guessing without backtracking and could leave the board
  invalid; it is now a proper backtracking solver, and generation guarantees a
  unique solution.
- Drawing happened via `CreateGraphics()` on resize/click/move, which flickered;
  all painting now goes through `Paint` + `Invalidate`.
