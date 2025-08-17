using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EvolutionCell
{
    public static class FileManager
    {
        // Метод записи данных в файл
        public static void WriteFile(string filePath, string content)
        {
            File.WriteAllText(filePath, content);
        }

        // Метод чтения данных из файла
        public static string ReadFile(string filePath)
        {
            return File.Exists(filePath) ? File.ReadAllText(filePath) : null;
        }

    }

                
                   
        
}

