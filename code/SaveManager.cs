using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;

namespace EvolutionCell
{
    public static class SaveManager
    {
        private static string savePath = "savegame.json";

        // Сохранение игры
        public static void SaveGame(Game game)
        {
            try
            {
                // Преобразуем двумерный массив в зубчатый (bool[][])
                bool[][] gridJagged = new bool[game.Size][];
                for (int i = 0; i < game.Size; i++)
                {
                    gridJagged[i] = new bool[game.Size];
                    for (int j = 0; j < game.Size; j++)
                        gridJagged[i][j] = game.Grid[i, j];
                }

                // Объект для сериализации
                var saveObj = new
                {
                    game.Size,
                    Grid = gridJagged,
                    game.Generation,
                    game.AliveCount,
                    game.DeadLastGen,
                    game.MaxAlive,
                    game.Speed,
                    game.SoundEnabled
                };

                string json = JsonSerializer.Serialize(saveObj, new JsonSerializerOptions { WriteIndented = true });
                FileManager.WriteFile(savePath, json);
                Console.WriteLine("Игра успешно сохранена.");
                Thread.Sleep(1000); // пауза 1 секунда

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при сохранении игры: {ex.Message}");
                Thread.Sleep(1000); // пауза 1 секунда
            }
        }



        // Загрузка игры
        public static Game LoadGame()
        {
            try
            {
                string json = FileManager.ReadFile("savegame.json");
                if (string.IsNullOrEmpty(json))
                {
                    Console.WriteLine("Сохранения не найдены. Начинаем новую игру.");
                    Thread.Sleep(1000); // пауза 1 секунда
                    return null;
                }

                // Временный анонимный объект
                var temp = JsonSerializer.Deserialize<TempSave>(json);

                Game game = new Game(temp.Size)
                {
                    Grid = new bool[temp.Size, temp.Size],
                    Generation = temp.Generation,
                    AliveCount = temp.AliveCount,
                    DeadLastGen = temp.DeadLastGen,
                    MaxAlive = temp.MaxAlive,
                    Speed = temp.Speed,
                    SoundEnabled = temp.SoundEnabled
                };

                for (int i = 0; i < temp.Size; i++)
                    for (int j = 0; j < temp.Size; j++)
                        game.Grid[i, j] = temp.Grid[i][j];

                Console.WriteLine("Игра успешно загружена.");
                Thread.Sleep(1000); // пауза 1 секунда
                return game;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при загрузке игры: {ex.Message}");
                Thread.Sleep(1000); // пауза 1 секунда
                return null;
            }
        }

        // Вспомогательный класс для десериализации
        private class TempSave
        {
            public int Size { get; set; }
            public bool[][] Grid { get; set; }
            public int Generation { get; set; }
            public int AliveCount { get; set; }
            public int DeadLastGen { get; set; }
            public int MaxAlive { get; set; }
            public int Speed { get; set; }
            public bool SoundEnabled { get; set; }
        }
    }
}
