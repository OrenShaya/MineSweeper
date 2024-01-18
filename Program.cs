using System.Data;

// draw the board
static void DrawBoard(int[,] mineField, bool[,] mineFieldVisable)
{
    Console.Clear();
    Console.Write("  |  ");
    for (int i = 0; i < mineField.GetLength(1); i++)
        Console.Write(i + "  ");
    Console.WriteLine();

    Console.Write("--+--");
    for (int i = 0; i < mineField.GetLength(1); i++)
        Console.Write("---");
    Console.WriteLine();

    for (int i = 0; i < mineField.GetLength(0); i++)
    {
        Console.Write(i + " |");
        for (int j = 0; j < mineField.GetLength(1); j++)
        {
            Console.Write(" ");
            if (mineFieldVisable[i, j] == false)
                Console.Write(" #");
            else
            {
                if (mineField[i, j] == (int)content.MINE)
                    Console.Write(" X");
                else if (mineField[i, j] > 0)
                    Console.Write("{0,2}", (mineField[i, j]));
                else
                    Console.Write(" -");
            }
        }
        Console.WriteLine();
    }
    Console.WriteLine();
}

bool reveal(int row, int column, ref int[,] mineField, ref bool[,] mineFieldVisable)
{
    if (mineField[row, column] == Convert.ToInt16(content.MINE))
    {   // mine
        return true;
    }
    else if (mineField[row, column] == 0)
    {   // empty
        mineFieldVisable[row, column] = true;

        for (int i = Math.Max(row - 1, 0); i <= Math.Min(mineField.GetLength(0) - 1, row + 1); i++)
            for (int j = Math.Max(column - 1, 0); j <= Math.Min(mineField.GetLength(1) - 1, column + 1); j++)
            {
                if (i == row && j == column)
                    continue;
                if (mineField[i, j] == 0 && mineFieldVisable[i, j] == false)
                    reveal(i, j, ref mineField, ref mineFieldVisable);
                mineFieldVisable[i, j] = true;
            }
    }
    else
    {   // number
        mineFieldVisable[row, column] = true;
    }
    DrawBoard(mineField, mineFieldVisable);
    return false;
}

bool checkWin(int[,] mineField, bool[,] mineFieldVisable)
{
    bool win = true;
    for (int i = 0; i < mineField.GetLength(0); i++)
        for (int j = 0; j < mineField.GetLength(1); j++)
        {
            if (mineFieldVisable[i, j] && mineField[i, j] != Convert.ToInt32(content.MINE))
            {   // number revealed
                continue;
            }
            else if (mineFieldVisable[i, j] && mineField[i, j] == Convert.ToInt32(content.MINE))
            {   // mine revealed
                win = false;
                break;
            }
            else if (mineFieldVisable[i, j] == false && mineField[i, j] != Convert.ToInt32(content.MINE))
            {   // number unrevealed
                win = false;
                break;
            }
        }
    return win;
}

Game game = new Game();
int[,] mineField = null;
bool[,] mineFieldVisable = null;
startNewGame(9, 9, 9, ref game, ref mineField, ref mineFieldVisable);

bool lost = false;
DrawBoard(mineField, mineFieldVisable);
while (game.state == status.playing)
{
    int row, column;
    do
    {    // Get X,Y coordinate from user
        Console.WriteLine();
        Console.Write("Choose row number: ");
        row = Convert.ToInt32(Console.ReadLine());
        Console.Write("Choose column number: ");
        column = Convert.ToInt32(Console.ReadLine());
    } while (row < 0 || row > game.row || column < 0 || column > game.column);

    lost = reveal(row, column, ref mineField, ref mineFieldVisable);
    if (lost)
    {
        game.state = status.stopped;

        for (int i = 0; i < game.row; i++)
            for (int j = 0; j < game.column; j++)
                mineFieldVisable[i, j] = true;

        DrawBoard(mineField, mineFieldVisable);

        Console.WriteLine("You lost");
        char again = tryAgain();
        if (again == 'y' || again == 'Y')
        {
            game.state = status.playing;
            startNewGame(game.row, game.column, game.mine, ref game, ref mineField, ref mineFieldVisable);
        }
        else
            game.state = status.stopped;
    }
    if (checkWin(mineField, mineFieldVisable))
    {
        Console.WriteLine("You won!");
        char again = tryAgain();
        if (again == 'y' || again == 'Y')
        {
            game.state = status.playing;
            startNewGame(game.row, game.column, game.mine, ref game, ref mineField, ref mineFieldVisable);
        }
        else
            game.state = status.stopped;
    }
}

static void startNewGame(int row, int column, int mines, ref Game game, ref int[,] mineField, ref bool[,] mineFieldVisable)
{
    game = initGame(game, row, column, mines);

    mineField = new int[game.row, game.column];
    mineFieldVisable = new bool[game.row, game.column];

    emptyBoard(game, ref mineField, ref mineFieldVisable);
    createMines(game, mineField);

    DrawBoard(mineField, mineFieldVisable);
}

static Game initGame(Game game, int row, int column, int mine)
{
    game.row = row;
    game.column = column;
    game.mine = mine;
    game.state = status.playing;
    return game;
}

static char tryAgain()
{
    Console.WriteLine("Try again? y/n");
    char again = Convert.ToChar(Console.ReadLine());
    while ((again != 'y' && again != 'n' && again != 'Y' && again != 'N'))
    {
        Console.WriteLine("invalid, want to play again?");
        again = Convert.ToChar(Console.Read());
    }
    return again;
}

static void emptyBoard(Game game, ref int[,] mineField, ref bool[,] mineFieldVisable)
{
    // init empty board
    for (int i = 0; i < game.row; i++)
    {
        for (int j = 0; j < game.column; j++)
        {
            mineField[i, j] = 0;
            mineFieldVisable[i, j] = false;
        }
    }
}

static void createMines(Game game, int[,] mineField) {   
    // create the mines
    int x, y, minesToCreate = game.mine;
    Random rng = new Random();
    while (minesToCreate > 0)
    {
        do
        {
            x = rng.Next(0, game.row);
            y = rng.Next(0, game.column);
        }
        while (mineField[x, y] != (int)content.EMPTY);

        mineField[x, y] = (int)content.MINE;

        // place numbers around the mines
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

