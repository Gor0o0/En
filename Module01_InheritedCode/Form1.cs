using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

#pragma warning disable CS8618
namespace Module01_InheritedCode;

public partial class Form1 : Form
{
    private const uint WmSetText = 0x000C;
    private const uint WmClose = 0x0010;
    private const uint BmSetStyle = 0x00F4;

    private const int BsPushButton = 0;
    private const int BsDefaultPushButton = 1;

    private Process? targetProcess;
    private bool emphasizedButtons;

    public Form1()
    {
        InitializeComponent();
    }

    [DllImport("user32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    private static extern IntPtr FindWindow(string? className, string? windowName);

    [DllImport("user32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    private static extern IntPtr SendMessage(
        IntPtr windowHandle,
        uint message,
        IntPtr wParam,
        string lParam);

    [DllImport("user32.dll", SetLastError = true)]
    private static extern IntPtr SendMessage(
        IntPtr windowHandle,
        uint message,
        IntPtr wParam,
        IntPtr lParam);

    [DllImport("user32.dll")]
    private static extern bool EnumChildWindows(
        IntPtr parentHandle,
        EnumWindowsProc callback,
        IntPtr lParam);

    [DllImport("user32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    private static extern int GetWindowText(
        IntPtr windowHandle,
        StringBuilder text,
        int maxCount);

    [DllImport("kernel32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool Beep(uint frequency, uint duration);

    [DllImport("user32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool MessageBeep(uint type);

    private delegate bool EnumWindowsProc(IntPtr windowHandle, IntPtr lParam);

    private void ShowInfo()
    {
        string name = studentName.Text.Trim();
        if (string.IsNullOrWhiteSpace(name) || name == "Введите ФИО")
        {
            name = "Не указано";
        }

        MessageBox.Show(
            $"Студент: {name}\nКурс: Системное программирование",
            "Информация о студенте",
            MessageBoxButtons.OK,
            MessageBoxIcon.Information);

        MessageBox.Show(
            "Модуль 1: Использование унаследованного кода",
            "Задание",
            MessageBoxButtons.OK,
            MessageBoxIcon.Information);

        MessageBox.Show(
            "Приложение использует функцию MessageBox из Windows API.",
            "Технология",
            MessageBoxButtons.OK,
            MessageBoxIcon.Information);
    }

    private void ChangeWindowTitle()
    {
        string newTitle = windowTitle.Text.Trim();
        if (newTitle.Length == 0)
        {
            MessageBox.Show(this, "Введите новый заголовок окна.");
            return;
        }

        IntPtr windowHandle = FindWindow(null, Text);
        if (windowHandle == IntPtr.Zero)
        {
            MessageBox.Show(this, "Окно не найдено.");
            return;
        }

        SendMessage(windowHandle, WmSetText, IntPtr.Zero, newTitle);
        Text = newTitle;
    }

    private void CloseWindowViaMessage()
    {
        IntPtr windowHandle = FindWindow(null, Text);
        if (windowHandle == IntPtr.Zero)
        {
            MessageBox.Show(this, "Окно не найдено.");
            return;
        }

        SendMessage(windowHandle, WmClose, IntPtr.Zero, IntPtr.Zero);
    }

    private async Task PlayBeepsAsync()
    {
        playBeeps.Enabled = false;

        try
        {
            await Task.Run(() =>
            {
                for (int index = 0; index < 5; index++)
                {
                    if (index % 2 == 0)
                    {
                        Beep((uint)(440 + index * 110), 180);
                    }
                    else
                    {
                        MessageBeep(0x00000040);
                    }

                    Thread.Sleep(450);
                }
            });
        }
        finally
        {
            if (!IsDisposed)
            {
                playBeeps.Enabled = true;
            }
        }
    }

    private async void LaunchTarget()
    {
        try
        {
            string configuration = IsDebugBuild() ? "Debug" : "Release";
            string executablePath = Path.GetFullPath(Path.Combine(
                AppContext.BaseDirectory,
                "..",
                "..",
                "..",
                "Module01_StyleTarget",
                "bin",
                configuration,
                "net8.0-windows",
                "Module01_StyleTarget.exe"));

            if (!File.Exists(executablePath))
            {
                throw new FileNotFoundException(
                    "Целевое приложение не найдено. Соберите всё решение.",
                    executablePath);
            }

            targetProcess = Process.Start(new ProcessStartInfo(executablePath)
            {
                UseShellExecute = true
            });

            if (targetProcess is null)
            {
                throw new InvalidOperationException("Не удалось запустить целевое приложение.");
            }

            for (int attempt = 0; attempt < 30; attempt++)
            {
                await Task.Delay(200);
                if (FindWindow(null, "Окно для изменения стилей") != IntPtr.Zero)
                {
                    break;
                }
            }

            changeStyle.Enabled = FindWindow(null, "Окно для изменения стилей") != IntPtr.Zero;
            styleStatus.Text = changeStyle.Enabled
                ? "Окно найдено через FindWindow."
                : "Окно не найдено. Попробуйте запустить его ещё раз.";
        }
        catch (Exception exception)
        {
            styleStatus.Text = $"Ошибка запуска: {exception.Message}";
        }
    }

    private void ChangeTargetStyles()
    {
        IntPtr targetWindow = FindWindow(null, "Окно для изменения стилей");
        if (targetWindow == IntPtr.Zero)
        {
            styleStatus.Text = "Целевое окно не найдено.";
            changeStyle.Enabled = false;
            return;
        }

        var buttonHandles = new List<IntPtr>();
        EnumChildWindows(targetWindow, (windowHandle, _) =>
        {
            var buttonText = new StringBuilder(128);
            GetWindowText(windowHandle, buttonText, buttonText.Capacity);

            if (buttonText.ToString() is "Первичная кнопка" or "Вторичная кнопка")
            {
                buttonHandles.Add(windowHandle);
            }

            return true;
        }, IntPtr.Zero);

        if (buttonHandles.Count == 0)
        {
            styleStatus.Text = "Кнопки целевого окна не найдены.";
            return;
        }

        emphasizedButtons = !emphasizedButtons;
        int newStyle = emphasizedButtons ? BsDefaultPushButton : BsPushButton;

        foreach (IntPtr buttonHandle in buttonHandles)
        {
            SendMessage(buttonHandle, BmSetStyle, (IntPtr)newStyle, (IntPtr)1);
        }

        string styleName = emphasizedButtons ? "BS_DEFPUSHBUTTON" : "BS_PUSHBUTTON";
        styleStatus.Text = $"Найдено кнопок: {buttonHandles.Count}. Стиль: {styleName}.";
    }

    private static bool IsDebugBuild()
    {
#if DEBUG
        return true;
#else
        return false;
#endif
    }
}
