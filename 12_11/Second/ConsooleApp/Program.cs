
using System;
using System.IO;

using GeometryLibrary;
using TextLibrary;
using ContactLibrary;
using FileSystemLibrary;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("==================================");


        Console.WriteLine("\n--- 1 Фигуры ---");

        Console.WriteLine(
            "квадрата: " +
            Geometry.SquareArea(5)
        );

        Console.WriteLine(
            "прямоугольника: " +
            Geometry.RectangleArea(4, 6)
        );

        Console.WriteLine(
            "треугольника: " +
            Geometry.TriangleArea(3, 4, 5)
        );


        Console.WriteLine("\n--- 2 Текст ---");

        string palindrome = "А роза упала на лапу Азора";

        Console.WriteLine(
            "Проверка палиндрома: " +
            TextTools.IsPalindrome(palindrome)
        );

        string text = "пр кд чд";

        Console.WriteLine(
            "Количество предложений: " +
            TextTools.CountSentences(text)
        );

        Console.WriteLine(
            "Перевёрнутая строка: " +
            TextTools.ReverseString("Hello World")
        );


        Console.WriteLine("\n--- 3 контакты ---");

        Console.WriteLine(
            "ФИО: " +
            ContactValidator.IsValidName("Самса Рожок Олеговна")
        );

        Console.WriteLine(
            "Возраст: " +
            ContactValidator.IsValidAge("25")
        );

        Console.WriteLine(
            "Телефон: " +
            ContactValidator.IsValidPhone("+88003553535")
        );

        Console.WriteLine(
            "Email: " +
            ContactValidator.IsValidEmail("test@mail.com")
        );

        Console.WriteLine("\n--- 4 Файлы ---");

        string testFolder = Path.Combine(
            Path.GetTempPath(),
            "DLL_Test_Folder"
        );

        string sourceFolder = Path.Combine(testFolder, "Source");
        string copyFolder = Path.Combine(testFolder, "Copy");

        string nestedFolder = Path.Combine(sourceFolder, "Nested");

        Directory.CreateDirectory(nestedFolder);

        string file1 = Path.Combine(sourceFolder, "file1.txt");
        string file2 = Path.Combine(sourceFolder, "file2.log");
        string file3 = Path.Combine(nestedFolder, "file3.txt");

        File.WriteAllText(file1, "Первый файл");
        File.WriteAllText(file2, "Второй файл");
        File.WriteAllText(file3, "Третий файл");

        string singleCopy = Path.Combine(testFolder, "single.txt");

        FileManager.CopyFile(file1, singleCopy);

        Console.WriteLine(
            "Копирование файла: " +
            File.Exists(singleCopy)
        );

        FileManager.CopyDirectory(sourceFolder, copyFolder);

        Console.WriteLine(
            "Копирование директории: " +
            Directory.Exists(copyFolder)
        );

        FileManager.DeleteFile(singleCopy);

        Console.WriteLine(
            "Удаление файла: " +
            !File.Exists(singleCopy)
        );

        FileManager.DeleteFiles(new string[]
        {
            file2
        });

        Console.WriteLine(
            "Удаление по набору имён: " +
            !File.Exists(file2)
        );

        FileManager.DeleteFilesByMask(sourceFolder, "*.txt");

        Console.WriteLine(
            "Удаление по маске: " +
            (Directory.GetFiles(sourceFolder, "*.txt").Length == 0)
        );

        string moveSource = Path.Combine(copyFolder, "file1.txt");
        string moveDestination = Path.Combine(testFolder, "moved.txt");

        FileManager.MoveFile(moveSource, moveDestination);

        Console.WriteLine(
            "Перенос: " +
            File.Exists(moveDestination)
        );

        Directory.Delete(testFolder, true);

        Console.WriteLine("\n==================================");
        
        Console.ReadKey();
    }
}
