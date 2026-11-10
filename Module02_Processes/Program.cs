using System.Globalization;

namespace Module02_Processes;

internal static class Program
{
    [STAThread]
    private static int Main(string[] args)
    {
        ApplicationConfiguration.Initialize();

        if (args.Length == 0)
        {
            Application.Run(new Form1());
            return 0;
        }

        try
        {
            return args[0] switch
            {
                "--child-calc" => RunCalculation(args),
                "--child-search" => RunFileSearch(args),
                "--child-long" => RunLongProcess(),
                _ => 1
            };
        }
        catch (Exception exception)
        {
            MessageBox.Show(
                $"Ошибка дочернего процесса: {exception.Message}",
                "Ошибка",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
            return 2;
        }
    }

    private static int RunCalculation(string[] args)
    {
        if (args.Length != 4)
        {
            throw new ArgumentException("Передайте два числа и знак операции.");
        }

        decimal first = decimal.Parse(args[1], CultureInfo.InvariantCulture);
        decimal second = decimal.Parse(args[3], CultureInfo.InvariantCulture);
        string operation = args[2];

        decimal result = operation switch
        {
            "+" => first + second,
            "-" => first - second,
            "*" => first * second,
            "/" when second != 0 => first / second,
            "/" => throw new DivideByZeroException("Деление на ноль невозможно."),
            _ => throw new ArgumentException("Неизвестная операция.")
        };

        MessageBox.Show(
            $"Аргументы дочернего процесса:\r\n" +
            $"{args[1]} {operation} {args[3]}\r\n\r\n" +
            $"Результат: {result}",
            "Дочерний процесс: вычисление",
            MessageBoxButtons.OK,
            MessageBoxIcon.Information);

        return 0;
    }

    private static int RunFileSearch(string[] args)
    {
        if (args.Length != 3)
        {
            throw new ArgumentException("Передайте путь к файлу и слово для поиска.");
        }

        string content = File.ReadAllText(args[1]);
        long count = CountOccurrences(content, args[2]);

        MessageBox.Show(
            $"Путь к файлу: {args[1]}\r\n" +
            $"Слово: {args[2]}\r\n" +
            $"Количество вхождений: {count}",
            "Дочерний процесс: поиск",
            MessageBoxButtons.OK,
            MessageBoxIcon.Information);

        return 0;
    }

    private static int RunLongProcess()
    {
        MessageBox.Show(
            "Дочерний процесс запущен и ждёт закрытия этого окна. " +
            "Его можно закрыть здесь или принудительно завершить из родительского приложения.",
            "Дочерний процесс",
            MessageBoxButtons.OK,
            MessageBoxIcon.Information);

        return 0;
    }

    private static long CountOccurrences(string text, string word)
    {
        if (string.IsNullOrEmpty(word))
        {
            throw new ArgumentException("Искомое слово не может быть пустым.");
        }

        long count = 0;
        int position = 0;

        while ((position = text.IndexOf(
                   word,
                   position,
                   StringComparison.CurrentCultureIgnoreCase)) >= 0)
        {
            count++;
            position += word.Length;
        }

        return count;
    }
}
