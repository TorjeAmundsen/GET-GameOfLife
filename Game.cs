namespace SimpleGameOfLife
{
    internal class Game
    {
        static char[,] _cells;
        static int _cols;
        static int _rows;
        static int _totalCellsAlive;
        public static void Run()
        {
            _totalCellsAlive = 0;
            _cols = Console.WindowWidth;
            _rows = Console.WindowHeight - 3;
            _cells = new char[_cols, _rows];

            var random = new Random();

            for (int i = 0; i < _cols; ++i)
            {
                for (int j = 0; j < _rows; ++j)
                {
                    int rng = random.Next(0, 2);
                    _cells[i, j] = rng == 1 ? 'O' : ' ';
                    if (_cells[i, j] == 'O') _totalCellsAlive++;
                }
            }
            Console.CursorVisible = false;
            while (true)
            {
                Console.ForegroundColor = ConsoleColor.White;
                string output = "";
                for (int i = 0; i < _rows; ++i)
                {
                    for (int j = 0; j < _cols; ++j)
                    {
                        output += _cells[j, i];
                    }
                    output += "\n";
                }
                Console.SetCursorPosition(0, 0);
                Console.WriteLine(output);
                Console.Write("Total cells alive: ");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write(_totalCellsAlive.ToString().PadLeft(4, ' '));
                Thread.Sleep(100);
                Update();
            }
        }

        private static void Update()
        {
            var newCells = new char[_cols, _rows];
            _totalCellsAlive = 0;
            for (int i = 0; i < _cols; ++i)
            {
                for (int j = 0; j < _rows; ++j)
                {
                    bool isAlive = _cells[i, j] == 'O';
                    int aliveNeighborCount = CountAliveNeighbors(i, j);
                    bool isNewCellAlive =
                        (isAlive && aliveNeighborCount is 2 or 3)
                        || (!isAlive && aliveNeighborCount == 3);
                    newCells[i, j] = isNewCellAlive ? 'O' : ' ';
                    if (isNewCellAlive) _totalCellsAlive++;
                }
            }
            _cells = newCells;
        }
        private static int CountAliveNeighbors(int col, int row)
        {
            int count = 0;
            int[,] directions =
            {
                { 1,  0 },
                {-1,  0 },
                { 0,  1 },
                { 0, -1 },
                { 1,  1 },
                { 1, -1 },
                {-1,  1 },
                {-1, -1 },
            };
            for (int i = 0; i < 8; ++i)
            {
                int newCol = col + directions[i, 0];
                int newRow = row + directions[i, 1];
                if (newRow < 0 || newCol < 0 || newRow >= _rows || newCol >= _cols) continue;
                if (_cells[newCol, newRow] == 'O') count++;
            }
            return count;
        }
    }
}
