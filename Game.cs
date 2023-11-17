namespace SimpleGameOfLife
{
    internal class Game
    {
        static char[,] _cells;
        static int _cols;
        static int _rows;
        static int _totalCellsAlive;
        static int _peakPopulation;
        static int _lowestPopulation;
        static int _populationDelta;
        static Game()
        {
            _totalCellsAlive = 0;
            _cols = Console.WindowWidth;
            _rows = Console.WindowHeight - 3;
            _cells = new char[_cols, _rows];
            _peakPopulation = 0;
            _lowestPopulation = 0;
        }
        public static void Run()
        {
            var random = new Random();

            for (int i = 0; i < _cols; ++i)
            {
                for (int j = 0; j < _rows; ++j)
                {
                    int rng = random.Next(0, 2);
                    _cells[i, j] = rng == 1 ? 'O' : ' ';
                    if (_cells[i, j] == 'O')
                    {
                        _totalCellsAlive++;
                        _peakPopulation++;
                        _lowestPopulation++;
                    }
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
                DisplayDataWithColor("  Current population: ", _totalCellsAlive, ConsoleColor.Cyan);
                DisplayDataWithColor("    Peak population: ", _peakPopulation, ConsoleColor.Green);
                DisplayDataWithColor("    Lowest population: ", _lowestPopulation, ConsoleColor.Red);
                DisplayDataWithColor("    Delta: ", _populationDelta, _populationDelta < 0 ? ConsoleColor.Red : ConsoleColor.Green);
                Thread.Sleep(100);
                Update();
            }
        }

        private static void Update()
        {
            var newCells = new char[_cols, _rows];
            int oldCellsAlive = _totalCellsAlive;
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
            if (_totalCellsAlive > _peakPopulation) _peakPopulation = _totalCellsAlive;
            if (_totalCellsAlive < _lowestPopulation) _lowestPopulation = _totalCellsAlive;
            _populationDelta = _totalCellsAlive - oldCellsAlive;
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
        private static void DisplayDataWithColor(string label, int data, ConsoleColor color)
        {
            Console.Write(label);
            Console.ForegroundColor = color;
            Console.Write(data.ToString().PadLeft(4, ' '));
            Console.ForegroundColor = ConsoleColor.White;
        }
    }
}
