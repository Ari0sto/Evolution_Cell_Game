using System;
using System.Threading;

namespace EvolutionCell
{
    /// <summary>
    /// Точка входа для консольной игры.
    /// </summary>
    public static class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Эволюция клетки: Жизнь";
            var menu = new Menu();
            menu.Show();
        }
    }
}