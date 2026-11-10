#nullable enable
#pragma warning disable CS8618
namespace Module01_StyleTarget;

partial class Form1
{
    private System.ComponentModel.IContainer? components;
    private TextBox messageText;
    private Button primaryButton;
    private Button secondaryButton;

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
        Text = "Окно для изменения стилей";
        ClientSize = new Size(560, 300);
        MinimumSize = new Size(480, 260);

        var description = new Label
        {
            Text = "Это отдельное приложение. Внешний контроллер найдёт окно через FindWindow " +
                   "и изменит стили кнопок через SendMessage.",
            Left = 25,
            Top = 25,
            Width = 500,
            Height = 55
        };
        Controls.Add(description);

        messageText = new TextBox
        {
            Left = 25,
            Top = 100,
            Width = 480,
            Text = "Поле ввода доступно для тестирования"
        };
        Controls.Add(messageText);

        primaryButton = new Button
        {
            Name = "primaryButton",
            Text = "Первичная кнопка",
            Left = 25,
            Top = 155,
            Width = 210,
            Height = 42
        };
        primaryButton.Click += (_, _) =>
            MessageBox.Show(this, messageText.Text, "Целевое приложение");
        Controls.Add(primaryButton);

        secondaryButton = new Button
        {
            Name = "secondaryButton",
            Text = "Вторичная кнопка",
            Left = 255,
            Top = 155,
            Width = 210,
            Height = 42
        };
        secondaryButton.Click += (_, _) => messageText.Clear();
        Controls.Add(secondaryButton);
    }
}
