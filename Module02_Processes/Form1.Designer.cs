#nullable enable
#pragma warning disable CS8618
namespace Module02_Processes;

partial class Form1
{
    private System.ComponentModel.IContainer? components;
    private TextBox firstNumber;
    private TextBox secondNumber;
    private TextBox searchPath;
    private TextBox searchWord;
    private ComboBox operation;
    private Button calcWait;
    private Button searchWait;
    private Button startLong;
    private Button waitLong;
    private Button killLong;
    private Label status;

    protected override void Dispose(bool disposing)
    {
        if (disposing && components != null)
        {
            components.Dispose();
        }

        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        components = new System.ComponentModel.Container();
        Text = "Модуль 2 — управление процессами";
        ClientSize = new Size(780, 520);
        MinimumSize = new Size(700, 460);

        var tabs = new TabControl { Dock = DockStyle.Fill };
        var calcTab = new TabPage("Аргументы и вычисление");
        var searchTab = new TabPage("Поиск в файле");
        var processTab = new TabPage("Ожидание / завершение");

        tabs.TabPages.AddRange(new[] { calcTab, searchTab, processTab });
        Controls.Add(tabs);

        calcTab.Controls.Add(new Label
        {
            Text = "Число 1",
            Left = 25,
            Top = 25,
            AutoSize = true
        });

        firstNumber = new TextBox
        {
            Left = 25,
            Top = 50,
            Width = 150,
            Text = "7"
        };
        calcTab.Controls.Add(firstNumber);

        calcTab.Controls.Add(new Label
        {
            Text = "Операция",
            Left = 195,
            Top = 25,
            AutoSize = true
        });

        operation = new ComboBox
        {
            Left = 195,
            Top = 50,
            Width = 100,
            DropDownStyle = ComboBoxStyle.DropDownList
        };
        operation.Items.AddRange(new object[] { "+", "-", "*", "/" });
        operation.SelectedIndex = 0;
        calcTab.Controls.Add(operation);

        calcTab.Controls.Add(new Label
        {
            Text = "Число 2",
            Left = 315,
            Top = 25,
            AutoSize = true
        });

        secondNumber = new TextBox
        {
            Left = 315,
            Top = 50,
            Width = 150,
            Text = "3"
        };
        calcTab.Controls.Add(secondNumber);

        calcWait = new Button
        {
            Text = "Запустить дочерний процесс и дождаться кода",
            Left = 25,
            Top = 100,
            Width = 360,
            Height = 38
        };
        calcWait.Click += async (_, _) => await LaunchCalcAsync();
        calcTab.Controls.Add(calcWait);

        calcTab.Controls.Add(new Label
        {
            Text = "Дочернее окно покажет аргументы и результат. Код завершения появится здесь.",
            Left = 25,
            Top = 155,
            Width = 680,
            Height = 40
        });

        searchTab.Controls.Add(new Label
        {
            Text = "Файл:",
            Left = 25,
            Top = 25,
            AutoSize = true
        });

        searchPath = new TextBox
        {
            Left = 25,
            Top = 50,
            Width = 680,
            Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
        };
        searchTab.Controls.Add(searchPath);

        searchTab.Controls.Add(new Label
        {
            Text = "Слово:",
            Left = 25,
            Top = 95,
            AutoSize = true
        });

        searchWord = new TextBox
        {
            Left = 25,
            Top = 120,
            Width = 300,
            Text = "bicycle"
        };
        searchTab.Controls.Add(searchWord);

        searchWait = new Button
        {
            Text = "Запустить поиск и дождаться завершения",
            Left = 25,
            Top = 165,
            Width = 400,
            Height = 38
        };
        searchWait.Click += async (_, _) => await LaunchSearchAsync();
        searchTab.Controls.Add(searchWait);

        processTab.Controls.Add(new Label
        {
            Text = "Демонстрационный дочерний процесс ждёт закрытия своего окна.",
            Left = 25,
            Top = 25,
            Width = 600
        });

        startLong = new Button
        {
            Text = "Запустить процесс",
            Left = 25,
            Top = 65,
            Width = 180,
            Height = 36
        };
        startLong.Click += (_, _) => StartLong();
        processTab.Controls.Add(startLong);

        waitLong = new Button
        {
            Text = "Ожидать завершения",
            Left = 220,
            Top = 65,
            Width = 190,
            Height = 36,
            Enabled = false
        };
        waitLong.Click += async (_, _) => await WaitLongAsync();
        processTab.Controls.Add(waitLong);

        killLong = new Button
        {
            Text = "Принудительно завершить",
            Left = 425,
            Top = 65,
            Width = 220,
            Height = 36,
            Enabled = false
        };
        killLong.Click += (_, _) => KillLong();
        processTab.Controls.Add(killLong);

        status = new Label
        {
            Left = 25,
            Top = 125,
            Width = 700,
            Height = 260,
            Text = "Готово"
        };
        processTab.Controls.Add(status);
    }
}
