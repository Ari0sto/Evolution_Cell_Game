using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EvolutionCell
{
    /// <summary>
    /// Вывод поля и статистики на экран.
    /// </summary>
    public static class Renderer
    {
        public static void Draw(bool[,] grid, int generation, int aliveCount, int deadLastGen, int speed, bool soundEnabled)
        {
            Console.Clear();
            int size = grid.GetLength(0);

            for (int r = 0; r < size; r++)
            {
                for (int c  = 0; c < size; c++)
                {
                    if (grid[r,c]) // <--- work
                    {
                        Console.ForegroundColor = ConsoleColor.Green; // Живая клетка
                        Console.Write("■ ");
                        Console.ResetColor();
                    }
                    else 
                    {
                        Console.ForegroundColor = ConsoleColor.DarkGray; // Мёртвая клетка
                        Console.Write(". ");
                        Console.ResetColor();
                    }
                    
                }
                Console.WriteLine();
            }

            Console.ResetColor(); // сброс цвета
            Console.WriteLine($"\nПоколение: {generation}");
            Console.WriteLine($"Живых клеток: {aliveCount}");
            Console.WriteLine($"Умерло в этом поколении: {deadLastGen}");
            Console.WriteLine($"Скорость: {speed} мс");
            Console.WriteLine($"Звук: {(soundEnabled ? "Вкл" : "Выкл")}");
            


            Console.WriteLine("\n[Q] выход  [Space] пауза/продолжить  [N] шаг  [стрелка вверх/вниз] скорость  [R] перезапуск [S] звук вкл/выкл");
            Console.WriteLine("[F5] сохранить  [F9] загрузить");
        }
    }
}
