
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

string filePath = @"C:\Users\User2\source\repos\ConsoleApp5\numbers.txt";

if (!File.Exists(filePath))
{
    Console.WriteLine("Не найден файл");
    return;
}

string fileContent = await File.ReadAllTextAsync(filePath);

List<int> numbers = fileContent
    .Split(new char[] { ' ', '\t', '\n', '\r', ',', ';' },
        StringSplitOptions.RemoveEmptyEntries)
    .Select(int.Parse)
    .ToList();

int uniqueCount = await Task.Run(() =>
    numbers
        .AsParallel()
        .Distinct()
        .Count()
);

Console.WriteLine("1.");
Console.WriteLine("Кол уник знач: " + uniqueCount);

int maxLength = await Task.Run(() =>
    numbers
        .AsParallel()
        .Select((value, index) =>
        {
            int length = 1;

            for (int i = index + 1; i < numbers.Count; i++)
            {
                if (numbers[i] > numbers[i - 1])
                {
                    length++;
                }
                else
                {
                    break;
                }
            }

            return length;
        })
        .DefaultIfEmpty(0)
        .Max()
);

Console.WriteLine();
Console.WriteLine("Задание 2.");
Console.WriteLine("Макс длина возраст послед: " + maxLength);
