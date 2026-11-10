#nullable enable
#pragma warning disable CS8618
namespace Module01_InheritedCode;

partial class Form1
{
    private System.ComponentModel.IContainer? components;
    private TabControl tabs;
    private TabPage infoTab;
    private TabPage windowTab;
    private TabPage soundTab;
    private TabPage styleTab;
    private TextBox windowTitle;
    private TextBox studentName;
    private Button changeTitle;
    private Button closeWindow;
    private Button playBeeps;
    private Button launchTarget;
    private Button changeStyle;
    private Label styleStatus;

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
        Text = "Модуль 1 — Win32 API";
        ClientSize = new Size(760, 470);
        MinimumSize = new Size(680, 400);

        tabs = new TabControl { Dock = DockStyle.Fill };
        infoTab = new TabPage("MessageBox");
        windowTab = new TabPage("FindWindow / SendMessage");
        soundTab = new TabPage("Beep / MessageBeep");
        styleTab = new TabPage("Стили другого окна");
        tabs.TabPages.AddRange(new[] { infoTab, windowTab, soundTab, styleTab });
        Controls.Add(tabs);

        infoTab.Controls.Add(new Label
        {
            Text = "Введите сведения о себе и покажите их несколькими окнами MessageBox",
            Left = 25,
            Top = 25,
            Width = 650,
            Font = new Font(Font, FontStyle.Bold)
        });

        infoTab.Controls.Add(new Label
        {
            Text = "ФИО:",
            Left = 25,
            Top = 62,
            AutoSize = true
        });

        studentName = new TextBox
        {
            Left = 25,
            Top = 86,
            Width = 400,
            Text = "Введите ФИО"
        };
        infoTab.Controls.Add(studentName);

        var showInfoButton = new Button
        {
            Text = "Показать информацию",
            Left = 25,
            Top = 130,
            Width = 200,
            Height = 36
        };
        showInfoButton.Click += (_, _) => ShowInfo();
        infoTab.Controls.Add(showInfoButton);

        windowTab.Controls.Add(new Label
        {
            Text = "Заголовок этого окна:",
            Left = 25,
            Top = 25,
            AutoSize = true
        });

        windowTitle = new TextBox
        {
            Left = 25,
            Top = 55,
            Width = 420
        };
        windowTab.Controls.Add(windowTitle);

        changeTitle = new Button
        {
            Text = "Найти окно и изменить заголовок",
            Left = 25,
            Top = 100,
            Width = 270,
            Height = 36
        };
        changeTitle.Click += (_, _) => ChangeWindowTitle();
        windowTab.Controls.Add(changeTitle);

        closeWindow = new Button
        {
            Text = "Найти окно и закрыть",
            Left = 310,
            Top = 100,
            Width = 220,
            Height = 36
        };
        closeWindow.Click += (_, _) => CloseWindowViaMessage();
        windowTab.Controls.Add(closeWindow);

        soundTab.Controls.Add(new Label
        {
            Text = "Функции Windows API создают серию звуковых сигналов с паузой.",
            Left = 25,
            Top = 25,
            Width = 680
        });

        playBeeps = new Button
        {
            Text = "Воспроизвести 5 сигналов",
            Left = 25,
            Top = 70,
            Width = 220,
            Height = 36
        };
        playBeeps.Click += async (_, _) => await PlayBeepsAsync();
        soundTab.Controls.Add(playBeeps);

        styleTab.Controls.Add(new Label
        {
            Text = "Запустите отдельное оконное приложение и измените стиль его кнопок через SendMessage.",
            Left = 25,
            Top = 25,
            Width = 680,
            Height = 45
        });

        launchTarget = new Button
        {
            Text = "Запустить окно приложения",
            Left = 25,
            Top = 85,
            Width = 230,
            Height = 36
        };
        launchTarget.Click += (_, _) => LaunchTarget();
        styleTab.Controls.Add(launchTarget);

        changeStyle = new Button
        {
            Text = "Переключить стиль кнопок",
            Left = 270,
            Top = 85,
            Width = 220,
            Height = 36,
            Enabled = false
        };
        changeStyle.Click += (_, _) => ChangeTargetStyles();
        styleTab.Controls.Add(changeStyle);

        styleStatus = new Label
        {
            Text = "Приложение ещё не запущено",
            Left = 25,
            Top = 145,
            Width = 650,
            Height = 45
        };
        styleTab.Controls.Add(styleStatus);
    }
}
