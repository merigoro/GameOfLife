using FluentAssertions;
using Life.Core.Models.Values;
using Life.Core.Services;

namespace Life.Tests;

public class GameServiceNextTests
{
    [Fact]
    public void Next_ShouldNotMutate_Input()
    {
        // Arrange
        var current = FromAscii(@"
            ....
            .OO.
            .OO.
            ....
        ");

        var snapshot = (CellState[,])current.Clone();

        // Act
        _ = GameService.Next(current);

        // Assert
        current.Should().BeEquivalentTo(snapshot);
    }

    [Fact]
    public void Next_BlockStillLife_ShouldRemainStable()
    {
        // Arrange: 2x2 block
        var current = FromAscii(@"
            ....
            .OO.
            .OO.
            ....
        ");

        // Act
        var next = GameService.Next(current);

        // Assert
        next.Should().BeEquivalentTo(current);
    }

    [Fact]
    public void Next_Blinker_ShouldOscillate_Period2()
    {
        // Arrange: horizontal blinker centered
        var g0 = FromAscii(@"
            .....
            ..O..
            ..O..
            ..O..
            .....
        ");

        var g1Expected = FromAscii(@"
            .....
            .....
            .OOO.
            .....
            .....
        ");

        var g2Expected = g0;

        // Act
        var g1 = GameService.Next(g0);
        var g2 = GameService.Next(g1);

        // Assert
        g1.Should().BeEquivalentTo(g1Expected);
        g2.Should().BeEquivalentTo(g2Expected);
    }

    [Fact]
    public void Next_DeadCellWithThreeNeighbors_ComesToLife()
    {
        // Arrange: dead center with three neighbors
        var current = FromAscii(@"
            ...
            .O.
            OO.
        ");

        // Act
        var next = GameService.Next(current);

        // Assert: center (1,1) should be alive
        next[1, 1].Should().Be(CellState.Alive);
    }

    [Fact]
    public void Next_LiveCellWithFewerThanTwoNeighbors_Dies_Underpopulation()
    {
        // Arrange: isolated live cell
        var current = FromAscii(@"
            ...
            .O.
            ...
        ");

        // Act
        var next = GameService.Next(current);

        // Assert
        next[1, 1].Should().Be(CellState.Dead);
    }

    [Fact]
    public void Next_LiveCellWithMoreThanThreeNeighbors_Dies_Overcrowding()
    {
        // Arrange: center has 4 neighbors
        var current = FromAscii(@"
            OOO
            O.O
            ...
        ");

        // Act
        var next = GameService.Next(current);

        // Assert
        next[1, 1].Should().Be(CellState.Dead);
    }


    // ---------- Helpers ----------

    private static CellState[,] FromAscii(string ascii)
    {
        var lines = ascii
            .Replace("\r", string.Empty)
            .Split('\n', System.StringSplitOptions.RemoveEmptyEntries | System.StringSplitOptions.TrimEntries);

        int rows = lines.Length;
        int cols = lines[0].Length;

        var grid = new CellState[rows, cols];

        for (int r = 0; r < rows; r++)
        {
            lines[r].Length.Should().Be(cols, "all lines must be same width");
            for (int c = 0; c < cols; c++)
            {
                grid[r, c] = lines[r][c] == 'O' ? CellState.Alive : CellState.Dead;
            }
        }

        return grid;
    }
}
