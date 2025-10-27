using Life.Core.Models;
using Life.Core.Models.Values;

namespace Life.Core.Services;

public static class GameService
{
    public static CellState[,] Random(
        GameGrid size,
        double aliveProbability = 0.25,
        int? seed = null)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(size.Rows);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(size.Cols);
        if (aliveProbability is < 0 or > 1) throw new ArgumentOutOfRangeException(nameof(aliveProbability));

        var random = seed.HasValue ? new Random(seed.Value) : new Random();
        var grid = new CellState[size.Rows, size.Cols];
        for (var row = 0; row < size.Rows; row++)
            for (var column = 0; column < size.Cols; column++)
                grid[row, column] = random.NextDouble() < aliveProbability ? CellState.Alive : CellState.Dead;
        return grid;
    }

    public static CellState[,] Next(
        CellState[,] current)
    {
        var rows = current.GetLength(0);
        var columns = current.GetLength(1);
        var next = new CellState[rows, columns];

        for (var row = 0; row < rows; row++)
        {
            for (var column = 0; column < columns; column++)
            {
                var aliveNeighbors = CountAliveNeighbors(current, row, column, rows, columns);
                var alive = current[row, column] == CellState.Alive;

                next[row, column] = (alive, aliveNeighbors) switch
                {
                    (true, < 2) => CellState.Dead,         // underpopulation
                    (true, > 3) => CellState.Dead,         // overcrowding
                    (true, 2 or 3) => CellState.Alive,     // survival
                    (false, 3) => CellState.Alive,         // reproduction
                    _ => CellState.Dead
                };
            }
        }
        return next;
    }

    private static int CountAliveNeighbors(
        CellState[,] grid,
        int row,
        int col,
        int rowCount,
        int colCount)
    {
        var aliveCount = 0;

        for (int rowOffset = -1; rowOffset <= 1; rowOffset++)
        {
            for (int colOffset = -1; colOffset <= 1; colOffset++)
            {
                if (rowOffset == 0 && colOffset == 0) continue;

                int neighborRow = row + rowOffset;
                int neighborCol = col + colOffset;

                if (neighborRow < 0 || neighborRow >= rowCount ||
                    neighborCol < 0 || neighborCol >= colCount)
                    continue;

                if (grid[neighborRow, neighborCol] == CellState.Alive)
                    aliveCount++;
            }
        }

        return aliveCount;
    }
}