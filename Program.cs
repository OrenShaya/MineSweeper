using System.Data;

// draw the board
void DrawBoard(int[,] mineField, Game game)
{
    Console.Clear();
    Console.Write("  |  ");
    for (int i = 0; i < game.column; i++)
        Console.Write(i + "  ");
    Console.WriteLine();

    Console.Write("--+--");
    for (int i = 0; i < game.column; i++)
        Console.Write("---");
    Console.WriteLine();

    for (int i = 0; i < game.row; i++)
    {
        Console.Write(i + " |");
        for (int j = 0; j < game.column; j++)
        {
            Console.Write("  ");
            if (mineField[i, j] == (int)content.MINE)
                Console.Write('X');
            else
                Console.Write(mineField[i, j]);
        }
        Console.WriteLine();
    }
}
// draw board end

Game game = new Game();
game = initGame(game, 9, 9, 9);

int[,] mineField = new int[game.row, game.column];

// init empty board
for (int i = 0; i < game.row; i++)
{
    for (int j = 0; j < game.column; j++)
    {
        mineField[i, j] = 0;
    }
}

// create the mines
int x, y, minesToCreate = game.mine;
Random rng = new Random();
while (minesToCreate > 0)
{
    do
    {
        x = rng.Next(0, game.row);
        y = rng.Next(0, game.column);
    } while (mineField[x, y] != (int)content.EMPTY);
    mineField[x, y] = (int)content.MINE;

    // do numbers around a mine here
    for (int i = x - 1; i <= x + 1; i++)
    {
        if (i < 0 || i >= game.row)
            continue;
        for (int j = y - 1; j <= y + 1; j++)
        {
            if (j < 0 || j >= game.column)
                continue;
            if (i == x && j == y)
                continue;
            mineField[i, j]++;
        }
    }

    minesToCreate--;
}


void reveal(int[,] mineField, Game game)
{
    Console.WriteLine("\n");
    Console.Write("Choose row number: ");
    int row = Convert.ToInt32(Console.ReadLine());
    Console.Write("Choose column number: ");
    int column = Convert.ToInt32(Console.ReadLine());
    mineField[row, column] = mineField[row, column] * -1;
    DrawBoard(mineField, game);
}


DrawBoard(mineField, game);

reveal(mineField, game);

static Game initGame(Game game, int row, int column, int mine)
{
    game.row = 9;
    game.column = 9;
    game.mine = 9;
    game.state = status.stopped;
    return game;
}

struct Game
{
    public int row;
    public int column;
    public int mine;
    public status state;
}

public enum status
{
    playing,
    stopped
}

enum content
{
    MINE = 9,
    EMPTY = 0
}