#nullable enable
#pragma warning disable CS8618

namespace Module03_Threads;

partial class Form1
{
    private System.ComponentModel.IContainer? components;
    private NumericUpDown primeStart, primeEnd, fibonacciEnd;
    private CheckBox openPrimeEnd, openFibonacciEnd;
    private Button startPrime, stopPrime, pausePrime, resumePrime;
    private Button startFib, stopFib, pauseFib, resumeFib, restart;
    private ListBox primeList, fibonacciList;
    private Label status;

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            StopAll();
            components?.Dispose();
        }

        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        components = new System.ComponentModel.Container();
        Text = "Потоки: простые числа и Фибоначчи";
        ClientSize = new Size(1000, 660);
        MinimumSize = new Size(850, 600);

        var panel = new Panel
        {
            Dock = DockStyle.Fill,
            Padding = new Padding(12)
        };
        Controls.Add(panel);

        panel.Controls.Add(new Label
        {
            Text = "Простые числа",
            Left = 10,
            Top = 8,
            Width = 450,
            Font = new Font(Font, FontStyle.Bold)
        });
        panel.Controls.Add(new Label { Text = "От", Left = 10, Top = 42, Width = 30 });

        primeStart = new NumericUpDown
        {
            Left = 45,
            Top = 38,
            Width = 100,
            Minimum = 2,
            Maximum = long.MaxValue,
            Value = 2
        };
        panel.Controls.Add(primeStart);
        panel.Controls.Add(new Label { Text = "До", Left = 155, Top = 42, Width = 30 });

        primeEnd = new NumericUpDown
        {
            Left = 190,
            Top = 38,
            Width = 120,
            Minimum = 2,
            Maximum = long.MaxValue,
            Value = 100
        };
        panel.Controls.Add(primeEnd);

        openPrimeEnd = new CheckBox
        {
            Text = "Без верхней границы",
            Left = 320,
            Top = 39,
            AutoSize = true
        };
        openPrimeEnd.CheckedChanged += (_, _) => primeEnd.Enabled = !openPrimeEnd.Checked;
        panel.Controls.Add(openPrimeEnd);

        startPrime = CreateButton("Старт", 10, 76);
        stopPrime = CreateButton("Стоп", 100, 76);
        pausePrime = CreateButton("Пауза", 190, 76);
        resumePrime = CreateButton("Продолжить", 280, 76);
        panel.Controls.AddRange(new Control[] { startPrime, stopPrime, pausePrime, resumePrime });

        primeList = new ListBox
        {
            Left = 10,
            Top = 116,
            Width = 460,
            Height = 440,
            Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left
        };
        panel.Controls.Add(primeList);

        panel.Controls.Add(new Label
        {
            Text = "Числа Фибоначчи",
            Left = 500,
            Top = 8,
            Width = 450,
            Font = new Font(Font, FontStyle.Bold)
        });
        panel.Controls.Add(new Label
        {
            Text = "До (пустая граница — бесконечно)",
            Left = 500,
            Top = 42,
            Width = 280
        });

        fibonacciEnd = new NumericUpDown
        {
            Left = 785,
            Top = 38,
            Width = 160,
            Minimum = 0,
            Maximum = long.MaxValue,
            Value = 100000
        };
        panel.Controls.Add(fibonacciEnd);

        openFibonacciEnd = new CheckBox
        {
            Text = "Без верхней границы",
            Left = 500,
            Top = 72,
            AutoSize = true
        };
        openFibonacciEnd.CheckedChanged += (_, _) => fibonacciEnd.Enabled = !openFibonacciEnd.Checked;
        panel.Controls.Add(openFibonacciEnd);

        startFib = CreateButton("Старт", 500, 106);
        stopFib = CreateButton("Стоп", 590, 106);
        pauseFib = CreateButton("Пауза", 680, 106);
        resumeFib = CreateButton("Продолжить", 770, 106);
        panel.Controls.AddRange(new Control[] { startFib, stopFib, pauseFib, resumeFib });

        fibonacciList = new ListBox
        {
            Left = 500,
            Top = 146,
            Width = 460,
            Height = 410,
            Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right
        };
        panel.Controls.Add(fibonacciList);

        restart = CreateButton("Полный рестарт с указанными границами", 10, 574, 320);
        restart.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
        panel.Controls.Add(restart);

        status = new Label
        {
            Left = 350,
            Top = 580,
            Width = 610,
            Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right,
            Text = "Готово"
        };
        panel.Controls.Add(status);

        startPrime.Click += (_, _) => StartPrime();
        stopPrime.Click += (_, _) => StopPrime();
        pausePrime.Click += (_, _) => primeGate.Reset();
        resumePrime.Click += (_, _) => primeGate.Set();
        startFib.Click += (_, _) => StartFib();
        stopFib.Click += (_, _) => StopFib();
        pauseFib.Click += (_, _) => fibonacciGate.Reset();
        resumeFib.Click += (_, _) => fibonacciGate.Set();
        restart.Click += (_, _) => RestartAll();
    }

    private static Button CreateButton(string text, int left, int top, int width = 80)
    {
        return new Button
        {
            Text = text,
            Left = left,
            Top = top,
            Width = width,
            Height = 30
        };
    }
}
