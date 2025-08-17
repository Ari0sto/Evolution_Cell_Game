using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EvolutionCell
{
    /// <summary>
    /// Отвечает за стартовое меню и запуск игры.
    /// </summary>
    public class Menu
    {
        public void Show()
        {
            while (true)
            {
                Console.Clear();
                Console.Title = "Эволюция клетки: Жизнь";
                Console.WriteLine("==================================");
                Console.WriteLine("   Э В О Л Ю Ц И Я   К Л Е Т К И  ");
                Console.WriteLine("==================================\n");
                Console.WriteLine("[1] Новый мир");
                Console.WriteLine("[2] Загрузить сохранение");
                Console.WriteLine("[3] Выход\n");

                int choice = ReadInt(1, 3);

                switch (choice)
                {
                    case 1:
                        StartNewGame();
                        break;
                    case 2:
                        LoadSavedGame();
                        break;
                    case 3:
                        Console.WriteLine("Выход из игры...");
                        Environment.Exit(0);
                        break;
                }
            }
        }

        private void StartNewGame()
        {
            Console.Clear();
            Console.WriteLine("Введите размер поля (от 10 до 50):");
            int size = ReadInt(10, 50);

            var game = new Game(size);

            Console.WriteLine("\nВыберите правила игры (по умолчанию — Конвей):");
            game.ConfigureRules();
            Console.WriteLine($"Текущие правила: {game.Rules}");
            Console.WriteLine("Нажмите любую клавишу для продолжения...");
            Console.ReadKey();

            Console.WriteLine("\nВыберите режим инициализации:");
            Console.WriteLine("[1] Случайная генерация");
            Console.WriteLine("[2] Ручная настройка");
            int initChoice = ReadInt(1, 2);


            if (initChoice == 1)
                game.InitializeRandom();
            else
                game.InitializeManual();

            game.Run();
        }

        private void LoadSavedGame()
        {
            Console.Clear();
            var loadedGame = SaveManager.LoadGame();
            if (loadedGame != null)
            {
                loadedGame.Run();
            }
            else
            {
                Console.WriteLine("Сохранений не найдено. Нажмите любую клавишу для возврата в меню...");
                Console.ReadKey(true);
            }
        }

        private int ReadInt(int min, int max)
        {
            int value;
            while (true)
            {
                Console.Write($"> ");
                if (int.TryParse(Console.ReadLine(), out value) && value >= min && value <= max)
                    return value;
                Console.WriteLine($"Введите число от {min} до {max}.");
            }
        }
    }
}
