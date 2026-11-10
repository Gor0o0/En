#nullable enable
#pragma warning disable CS8618

namespace Module04_Synchronization;

partial class Form1
{
    private System.ComponentModel.IContainer? components;
    private NumericUpDown pairCount, busCount, busCapacity, initialPassengers;
    private Button runPairs, runBuses;
    private TextBox pairOutput, busOutput;
    private TabControl tabs;

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            components?.Dispose();
        }

        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        components = new System.ComponentModel.Container();
        Text = "Синхронизация: события и автобусная остановка";
        ClientSize = new Size(920, 650);
        MinimumSize = new Size(800, 560);

        tabs = new TabControl { Dock = DockStyle.Fill };
        var pairsTab = new TabPage("События: пары чисел");
        var busesTab = new TabPage("Автобусная остановка");
        tabs.TabPages.AddRange(new[] { pairsTab, busesTab });
        Controls.Add(tabs);

        pairsTab.Controls.Add(new Label
        {
            Text = "Количество пар:",
            Left = 20,
            Top = 25,
            AutoSize = true
        });
        pairCount = new NumericUpDown
        {
            Left = 140,
            Top = 21,
            Width = 110,
            Minimum = 1,
            Maximum = 100000,
            Value = 100
        };
        pairsTab.Controls.Add(pairCount);

        runPairs = new Button
        {
            Text = "Сгенерировать и обработать",
            Left = 270,
            Top = 18,
            Width = 220,
            Height = 32
        };
        runPairs.Click += async (_, _) => await RunPairsAsync();
        pairsTab.Controls.Add(runPairs);

        pairOutput = CreateOutputBox(20, 70, 850, 500);
        pairsTab.Controls.Add(pairOutput);

        busesTab.Controls.Add(new Label
        {
            Text = "Число автобусов (за день):",
            Left = 20,
            Top = 24,
            AutoSize = true
        });
        busCount = new NumericUpDown
        {
            Left = 210,
            Top = 20,
            Width = 90,
            Minimum = 1,
            Maximum = 100,
            Value = 12
        };
        busesTab.Controls.Add(busCount);

        busesTab.Controls.Add(new Label
        {
            Text = "Вместимость:",
            Left = 320,
            Top = 24,
            AutoSize = true
        });
        busCapacity = new NumericUpDown
        {
            Left = 410,
            Top = 20,
            Width = 90,
            Minimum = 1,
            Maximum = 200,
            Value = 30
        };
        busesTab.Controls.Add(busCapacity);

        busesTab.Controls.Add(new Label
        {
            Text = "Пассажиров в автобусах при старте:",
            Left = 20,
            Top = 62,
            AutoSize = true
        });
        initialPassengers = new NumericUpDown
        {
            Left = 270,
            Top = 58,
            Width = 90,
            Minimum = 0,
            Maximum = 200,
            Value = 8
        };
        busesTab.Controls.Add(initialPassengers);

        runBuses = new Button
        {
            Text = "Запустить день",
            Left = 390,
            Top = 55,
            Width = 150,
            Height = 32
        };
        runBuses.Click += async (_, _) => await RunBusDayAsync();
        busesTab.Controls.Add(runBuses);

        busOutput = CreateOutputBox(20, 105, 850, 465);
        busesTab.Controls.Add(busOutput);
    }

    private static TextBox CreateOutputBox(int left, int top, int width, int height)
    {
        return new TextBox
        {
            Left = left,
            Top = top,
            Width = width,
            Height = height,
            Multiline = true,
            ReadOnly = true,
            ScrollBars = ScrollBars.Vertical,
            Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right
        };
    }
}
