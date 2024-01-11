using System.Data;

// draw the board
void DrawBoard(int[,] mineField, int row, int cols)
{
    Console.Write("  |  ");
    for (int i = 0; i < cols; i++)
        Console.Write(i + "  ");
    Console.WriteLine();

    Console.Write("--+--");
    for (int i = 0; i < cols; i++)
        Console.Write("---");
    Console.WriteLine();

    for (int i = 0; i < row; i++)
    {
        Console.Write(i+" |");
        for (int j = 0; j < cols; j++)
        {
            Console.Write("  ");
            if (mineField[i,j] == (int)content.MINE)
                Console.Write('X');
            else
                Console.Write('-');
        }
        Console.WriteLine();
    }
}
// draw board end

int rows = 9, columns = 9, mines = 9;
int[,] mineField = new int[rows,columns];

// init empty board
for (int i = 0; i < rows; i++)
{
    for (int j = 0; j < columns; j++)
    {
        mineField[i, j] = 0;
    }
}

// create the mines
int x, y, minesToCreate = mines;
Random rng = new Random();
while (minesToCreate > 0)
{
    do {
        x = rng.Next(0, rows);
        y = rng.Next(0, columns);
    } while (mineField[x,y] != (int)content.EMPTY);
    mineField[x, y] = (int)content.MINE;
    
    // do numbers around a mine here
    for (int i = x-1; i < x+1; i++)
    {

    }

    minesToCreate--;
}


DrawBoard(mineField, rows, columns);

enum content
{
    MINE = 9,
    EMPTY = 0
}