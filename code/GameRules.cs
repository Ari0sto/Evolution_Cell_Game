using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EvolutionCell
{
    public struct GameRules
    {
        public List<int> Survive; // кто выживает
        public List<int> Birth;   // кто рождается

        public GameRules(List<int> survive, List<int> birth)
        {
            Survive = survive;
            Birth = birth;
        }

        public override string ToString()
        {
            return $"Выживание: {string.Join(",", Survive)} | Рождение: {string.Join(",", Birth)}";
        }
    }

    public class RulesMenu
    {
        public static GameRules ShowRulesMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("══════════════════════════════════════");
                Console.WriteLine("              РЕЖИМ ИГРЫ              ");
                Console.WriteLine("══════════════════════════════════════");
                Console.WriteLine(" 1. Классический Конвей (B3/S23)");
                Console.WriteLine(" 2. Кастомный режим");
                Console.WriteLine(" 0. Назад");
                Console.WriteLine("══════════════════════════════════════");
                Console.Write("Выберите режим: ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        return new GameRules(new List<int> { 2, 3 }, new List<int> { 3 });

                    case "2":
                        return GetCustomRules();

                    case "0":
                        return new GameRules(new List<int> { 2, 3 }, new List<int> { 3 }); // по умолчанию
                }
            }
        }

        private static GameRules GetCustomRules()
        {
            Console.Clear();
            Console.WriteLine("══════════════════════════════════════");
            Console.WriteLine("          КАСТОМНЫЕ ПРАВИЛА           ");
            Console.WriteLine("══════════════════════════════════════");

            Console.Write("Введите числа для ВЫЖИВАНИЯ (например: 2 3): ");
            var survive = Console.ReadLine()
                .Split(" ", StringSplitOptions.RemoveEmptyEntries)
                .Select(int.Parse).ToList();

            Console.Write("Введите числа для РОЖДЕНИЯ (например: 3): ");
            var birth = Console.ReadLine()
                .Split(" ", StringSplitOptions.RemoveEmptyEntries)
                .Select(int.Parse).ToList();

            return new GameRules(survive, birth);
        }
    }
}
