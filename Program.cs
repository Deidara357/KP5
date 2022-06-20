using System;
using System.IO;
using System.Diagnostics;

namespace КП5
{
    class Program
    {
        static void Main(string[] args)
        {
            const string path = @"C:\Users\user\OneDrive\Робочий стіл\1-й курс_ІК-11_Колісецький Іван\2-й семестр\Теорія алгоритмів\КП5\rating.txt";
            string line;
            string key;
            var hashTable = new HashTable();

            using (StreamReader sr = new StreamReader(path))
            {
                while ((key = sr.ReadLine()) != null)
                {
                    line = sr.ReadLine().TrimEnd();
                    if (line != null) hashTable.Insert(key, line);
                    else break;
                }
            }

            //var timer = new Stopwatch();
            //timer.Start();
            //ShowHashTable(hashTable, "Хеш-функцiя MD5");
            //timer.Stop();
            //Console.WriteLine($"{timer.ElapsedMilliseconds} мiлiсекунд");

            bool isWorking = true;
            while (isWorking)
            {
                Console.WriteLine($"Щоб вивести всiх студентiв на екран натиснiть 1 \nЩоб видалити студента з таблицi та побачити оновлену таблицю натиснiть 2 \nЩоб вивести рейтинговий бал студента натиснiть 3 \nЩоб вийти, натиснiть будь-що iнше  ");
                switch (Console.ReadLine())
                {
                    case "1":
                        ShowHashTable(hashTable, "Хеш-функцiя MD5");
                        break;
                    case "2":
                        Console.WriteLine($"Введіть повне iм'я студента, якого потрiбно видалити з таблицi:");
                        line = Console.ReadLine();
                        hashTable.Delete(line);
                        ShowHashTable(hashTable, "Delete item from hashtable.");
                        break;
                    case "3":
                        Console.WriteLine($"Введiть повне iм'я студента, якого потрiбно знайти в таблицi:");
                        line = Console.ReadLine();
                        var text = hashTable.Search(line);
                        Console.WriteLine(text);
                        break;
                    default:
                        isWorking = false;
                        break;
                }
                Console.ReadKey();
            }
        }

        private static void ShowHashTable(HashTable hashTable, string title)
        {
            if (hashTable == null)
            {
                throw new ArgumentNullException(nameof(hashTable));
            }

            if (string.IsNullOrEmpty(title))
            {
                throw new ArgumentNullException(nameof(title));
            }

            Console.WriteLine(title);
            foreach (var item in hashTable.Items)
            {
                Console.WriteLine(item.Key);

                foreach (var value in item.Value)
                {
                    Console.WriteLine($"\t{value.Key} - {value.Value}");
                }
            }
            Console.WriteLine();
        }
    }
}
