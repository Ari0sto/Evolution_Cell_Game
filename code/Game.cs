using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;


namespace EvolutionCell
{
    /// <summary>
    /// Логика игры: хранение поля, расчет поколений, статистика, управление скоростью.
    /// </summary>
    public class Game
    {
        public int Size { get; set; }
        public bool[,] Grid { get; set; }
        public bool SoundEnabled { get; set; } = true;

        public int MaxAlive { get; set; } = 0;
        public int Generation { get; set; } = 0;
        public int AliveCount { get; set; } = 0;
        public int DeadLastGen { get; set; } = 0;
        public int Speed { get; set; } = 500;
        public bool IsRunning { get; set; } = true;

        private readonly Random _rand = new Random();
        public GameRules Rules { get; private set; } = new GameRules(new List<int> { 2, 3 }, new List<int> { 3 });


        public Game(int size)
        {
            Size = size;
            Grid = new bool[size, size];
        }

        public void InitializeRandom()
        {
            for (int r = 0; r < Size; r++)
                for (int c = 0; c < Size; c++)
                    Grid[r, c] = _rand.Next(100) < 20; // 20% живых

            AliveCount = CountAliveCells();
            Generation = 0;
            MaxAlive = AliveCount;
            DeadLastGen = 0;

        }


        public void InitializeManual()
        {
            Console.Clear();
            Console.WriteLine("Введите координаты живых клеток в формате рядок,столбец. Пустая строка - завершить.");
            while (true)
            {
                Console.Write("> ");
                string input = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(input))
                    break;

                var parts = input.Split(',');
                if (parts.Length == 2 &&
                    int.TryParse(parts[0], out int row) &&
                    int.TryParse(parts[1], out int col) &&
                    row >= 1 && row <= Size &&
                    col >= 1 && col <= Size)
                {
                    Grid[row - 1, col - 1] = true;
                }
                else
                {
                    Console.WriteLine("Неверный формат или координаты вне поля.");
                }
            }

            AliveCount = CountAliveCells();
            Generation = 0;
            MaxAlive = AliveCount;
            DeadLastGen = 0;

        }

        public void Run()
        {
            ConsoleKey key;
            bool wasPaused = false;

            do
            {
                if (IsRunning)
                {
                    // Обычный режим — отрисовали, посчитали следующее, подождали
                    Renderer.Draw(Grid, Generation, AliveCount, DeadLastGen, Speed, SoundEnabled);

                    // Проверка: если нет живых клеток — конец игры
                    if (AliveCount == 0 && Generation > 0)
                    {
                        ShowFinalReport();
                        return;
                    }

                    NextGeneration();
                    Thread.Sleep(Speed);
                    wasPaused = false;
                }
                else
                {
                    // В паузе — отрисуем один раз экран с подсказкой (при входе в паузу)
                    if (!wasPaused)
                    {
                        Renderer.Draw(Grid, Generation, AliveCount, DeadLastGen, Speed, SoundEnabled);
                        Console.WriteLine();
                        Console.WriteLine("=== П А У З А ===");
                        Console.WriteLine("Space — продолжить, N — шаг вперед, R — перезапуск, Q — выход");
                        wasPaused = true;
                    }

                    // Небольшая задержка
                    Thread.Sleep(50);
                }

                // Обработка ввода (если есть)
                if (Console.KeyAvailable)
                {
                    key = Console.ReadKey(true).Key;
                    HandleInput(key);
                }
                else
                {
                    key = ConsoleKey.NoName;
                }

            } while (key != ConsoleKey.Q);
        }

        private void ShowFinalReport()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("========================================");
            Console.WriteLine("       С И М У Л Я Ц И Я  З А В Е Р Ш Е Н А");
            Console.WriteLine("========================================\n");

            Console.ResetColor();
            Console.WriteLine($"Всего поколений:                {Generation}");
            Console.WriteLine($"Максимальное живых клеток:      {MaxAlive}");

            Console.WriteLine("\n----------------------------------------");
            Console.WriteLine("[R] — начать заново");
            Console.WriteLine("[Q] — выйти в главное меню");
            Console.WriteLine("----------------------------------------");

            while (true)
            {
                var endKey = Console.ReadKey(true).Key;
                if (endKey == ConsoleKey.R)
                {
                    var menu = new Menu();
                    menu.Show();   // возврат в главное меню
                    return;
                }
                else if (endKey == ConsoleKey.Q)
                {
                    return; // выход из игры
                }
            }
        }


        private void HandleInput(ConsoleKey key)
        {
            switch (key)
            {
                case ConsoleKey.Spacebar:
                    // входим/выходим из паузы
                    IsRunning = !IsRunning;
                    // сразу отрисовать текущий кадр (при входе/выходе)
                    Renderer.Draw(Grid, Generation, AliveCount, DeadLastGen, Speed, SoundEnabled);
                    if (!IsRunning)
                    {
                        Console.WriteLine();
                        Console.WriteLine("=== П А У З А ===");
                        Console.WriteLine("N — шаг вперед, Space — продолжить");
                    }
                    break;

                case ConsoleKey.N:
                    // шаг вперёд в паузе
                    if (!IsRunning)
                    {
                        NextGeneration();
                        // обязательно показать результат шага
                        Renderer.Draw(Grid, Generation, AliveCount, DeadLastGen, Speed, SoundEnabled);
                        Console.WriteLine();
                        Console.WriteLine("=== П А У З А === (последний шаг показан)");
                    }
                    break;

                case ConsoleKey.UpArrow:
                    if (Speed > 50) Speed -= 50;
                    // обновляем отображение, чтобы показать новую скорость
                    Renderer.Draw(Grid, Generation, AliveCount, DeadLastGen, Speed, SoundEnabled);
                    if (!IsRunning) Console.WriteLine("\n=== П А У З А ===");
                    break;

                case ConsoleKey.DownArrow:
                    if (Speed < 2000) Speed += 50;
                    Renderer.Draw(Grid, Generation, AliveCount, DeadLastGen, Speed, SoundEnabled);
                    if (!IsRunning) Console.WriteLine("\n=== П А У З А ===");
                    break;

                case ConsoleKey.R:
                    // Новый вариант
                    var menu = new Menu();
                    menu.Show();
                    break;

                case ConsoleKey.S:
                    SoundEnabled = !SoundEnabled;
                    break;

                case ConsoleKey.F5:
                    SaveManager.SaveGame(this);
                    break;

                case ConsoleKey.F9:
                    var loadedGame = SaveManager.LoadGame();
                    if (loadedGame != null)
                    {
                        // Копируем состояние загруженной игры в текущий объект
                        this.Size = loadedGame.Size;
                        this.Grid = loadedGame.Grid;
                        this.Generation = loadedGame.Generation;
                        this.AliveCount = loadedGame.AliveCount;
                        this.DeadLastGen = loadedGame.DeadLastGen;
                        this.MaxAlive = loadedGame.MaxAlive;
                        this.Speed = loadedGame.Speed;
                        this.SoundEnabled = loadedGame.SoundEnabled;
                    }
                    break;

            }
        }

        private void NextGeneration()
        {
            int size = Grid.GetLength(0);
            bool[,] next = new bool[size, size];
            int born = 0;
            int died = 0;

            for (int x = 0; x < Size; x++)
            {
                for (int y = 0; y < Size; y++)
                {
                    int neighbors = CountNeighbors(Grid, x, y);

                    if (!Grid[x, y] && Rules.Birth.Contains(neighbors)) // Рождение
                    {
                        next[x, y] = true;
                        born++;

                        if (SoundEnabled)
                        {
                            Console.Beep(1000, 100); // звук рождения
                        }
                    }
                    else if (Grid[x, y] && !Rules.Survive.Contains(neighbors)) // смерть
                    {
                        next[x, y] = false;
                        died++;

                        if (SoundEnabled)
                        {
                            Console.Beep(400, 100); // звук смерти
                        }
                    }
                    else
                    {
                        next[x, y] = Grid[x, y];
                    }


                }
            }

            Grid = next;
            Generation++;
            AliveCount = born + (AliveCount - died);
            DeadLastGen = died;

            if (AliveCount > MaxAlive)
                MaxAlive = AliveCount;

        }

        private int CountNeighbors(bool[,] grid, int row, int col)
        {
            int count = 0;
            int size = grid.GetLength(0);

            for (int dr = -1; dr <= 1; dr++)
            {
                for (int dc = -1; dc <= 1; dc++)
                {
                    if (dr == 0 && dc == 0) continue;

                    int nr = row + dr;
                    int nc = col + dc;

                    if (nr >= 0 && nr < size && nc >= 0 && nc < size && grid[nr, nc])
                        count++;
                }
            }
            return count;
        }

        

        private int CountAliveCells()
        {
            int count = 0;
            for (int r = 0; r < Size; r++)
                for (int c = 0; c < Size; c++)
                    if (Grid[r, c]) count++;
            return count;
        }

        public void ConfigureRules()
        {
            Rules = RulesMenu.ShowRulesMenu();
        }


    }
}
