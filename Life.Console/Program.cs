using Life.Core.Models;
using Life.Core.Models.Values;
using Life.Core.Services;

Console.OutputEncoding = System.Text.Encoding.UTF8;

int rows = args.Length > 0 && int.TryParse(args[0], out var r) ? r : 25;
int cols = args.Length > 1 && int.TryParse(args[1], out var c) ? c : 50;
double p = args.Length > 2 && double.TryParse(args[2], out var prob) ? prob : 0.25;
int speedMs = args.Length > 3 && int.TryParse(args[3], out var ms) ? ms : 100;
int? seed = args.Length > 4 && int.TryParse(args[4], out var s) ? s : null;

var size = new GameGrid(rows, cols);
var grid = GameService.Random(size, p, seed);

bool running = true;
Console.CancelKeyPress += (_, e) => { e.Cancel = true; running = false; };

while (running)
{
    Console.SetCursorPosition(0, 0);
    Draw(grid);
    Console.WriteLine();
    Console.WriteLine("Q to quit — Generation updates every {0} ms", speedMs);

    if (Console.KeyAvailable)
    {
        var key = Console.ReadKey(intercept: true);
        if (key.Key is ConsoleKey.Q) break;
    }

    await Task.Delay(speedMs);
    grid = GameService.Next(grid);
}

static void Draw(CellState[,] grid)
{
    var rows = grid.GetLength(0);
    var columns = grid.GetLength(1);

    for (int row = 0; row < rows; row++)
    {
        for (int column = 0; column < columns; column++)
        {
            Console.Write(grid[row, column] == CellState.Alive ? '█' : ' ');
        }

        Console.WriteLine();
    }
}
