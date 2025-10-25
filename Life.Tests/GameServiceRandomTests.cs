using FluentAssertions;
using Life.Core.Models;
using Life.Core.Services;

namespace Life.Tests;

public class GameServiceRandomTests
{
    [Theory]
    [InlineData(0, 5)]
    [InlineData(5, 0)]
    [InlineData(-1, 5)]
    [InlineData(5, -1)]
    public void Random_ShouldThrow_WhenGridSizeInvalid(int rows, int cols)
    {
        // Arrange
        var size = new GameGrid(rows, cols);

        // Act
        Action act = () => GameService.Random(size);

        // Assert
        act.Should().Throw<ArgumentOutOfRangeException>();
    }

    [Theory]
    [InlineData(-0.1)]
    [InlineData(1.1)]
    public void Random_ShouldThrow_WhenProbabilityOutOfRange(double p)
    {
        // Arrange
        var size = new GameGrid(2, 2);

        // Act
        Action act = () => GameService.Random(size, p);

        // Assert
        act.Should().Throw<ArgumentOutOfRangeException>()
            .WithParameterName("aliveProbability");
    }
}