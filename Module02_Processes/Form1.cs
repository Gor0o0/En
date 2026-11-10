using System.Diagnostics;
using System.Globalization;

#pragma warning disable CS8618
namespace Module02_Processes;

public partial class Form1 : Form
{
    private Process? longProcess;

    public Form1()
    {
        InitializeComponent();
    }

    private async Task LaunchCalcAsync()
    {
        if (!decimal.TryParse(
                firstNumber.Text,
                NumberStyles.Number,
                CultureInfo.CurrentCulture,
                out decimal first)
            || !decimal.TryParse(
                secondNumber.Text,
                NumberStyles.Number,
                CultureInfo.CurrentCulture,
                out decimal second))
        {
            MessageBox.Show(
                this,
                "Введите два корректных числа.",
                "Проверка данных",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
            return;
        }

        try
        {
            using Process process = StartChild(
                "--child-calc",
                first.ToString(CultureInfo.InvariantCulture),
                operation.Text,
                second.ToString(CultureInfo.InvariantCulture));

            await process.WaitForExitAsync();

            MessageBox.Show(
                this,
                $"Дочерний процесс завершён. Код завершения: {process.ExitCode}",
                "Результат");
        }
        catch (Exception exception)
        {
            MessageBox.Show(this, $"Не удалось запустить процесс: {exception.Message}");
        }
    }

    private async Task LaunchSearchAsync()
    {
        if (string.IsNullOrWhiteSpace(searchPath.Text) || string.IsNullOrEmpty(searchWord.Text))
        {
            MessageBox.Show(this, "Укажите полный путь к файлу и искомое слово.");
            return;
        }

        try
        {
            using Process process = StartChild(
                "--child-search",
                searchPath.Text,
                searchWord.Text);

            await process.WaitForExitAsync();

            MessageBox.Show(
                this,
                $"Дочерний процесс завершён. Код завершения: {process.ExitCode}",
                "Результат");
        }
        catch (Exception exception)
        {
            MessageBox.Show(this, $"Не удалось запустить процесс: {exception.Message}");
        }
    }

    private static Process StartChild(params string[] arguments)
    {
        string executablePath = Environment.ProcessPath
            ?? throw new InvalidOperationException("Не удалось определить путь к приложению.");

        var startInfo = new ProcessStartInfo(executablePath)
        {
            UseShellExecute = false,
            CreateNoWindow = false
        };

        foreach (string argument in arguments)
        {
            startInfo.ArgumentList.Add(argument);
        }

        return Process.Start(startInfo)
            ?? throw new InvalidOperationException("ОС не запустила дочерний процесс.");
    }

    private void StartLong()
    {
        try
        {
            longProcess = StartChild("--child-long");
            status.Text =
                $"Дочерний процесс запущен. PID: {longProcess.Id}. " +
                "Можно дождаться завершения или принудительно закрыть процесс.";

            waitLong.Enabled = true;
            killLong.Enabled = true;
            startLong.Enabled = false;
        }
        catch (Exception exception)
        {
            status.Text = $"Ошибка запуска: {exception.Message}";
        }
    }

    private async Task WaitLongAsync()
    {
        if (longProcess is null)
        {
            return;
        }

        Process process = longProcess;
        waitLong.Enabled = false;
        killLong.Enabled = false;

        try
        {
            await process.WaitForExitAsync();
            status.Text = $"Процесс завершён. Код завершения: {process.ExitCode}.";
        }
        catch (Exception exception)
        {
            status.Text = $"Ошибка ожидания: {exception.Message}";
        }
        finally
        {
            ReleaseLong(process);
        }
    }

    private void KillLong()
    {
        if (longProcess is null)
        {
            return;
        }

        Process process = longProcess;

        try
        {
            if (!process.HasExited)
            {
                process.Kill(entireProcessTree: true);
                process.WaitForExit();
            }

            status.Text = "Дочерний процесс принудительно завершён.";
        }
        catch (Exception exception)
        {
            status.Text = $"Ошибка завершения: {exception.Message}";
        }
        finally
        {
            ReleaseLong(process);
        }
    }

    private void ReleaseLong(Process process)
    {
        if (!ReferenceEquals(longProcess, process))
        {
            return;
        }

        process.Dispose();
        longProcess = null;
        waitLong.Enabled = false;
        killLong.Enabled = false;
        startLong.Enabled = true;
    }
}
