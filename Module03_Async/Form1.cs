using System.Numerics;
using System.Text;
using System.Collections.Concurrent;

#pragma warning disable CS8618
namespace Module03_Async;

public partial class Form1 : Form
{
    public Form1() { InitializeComponent(); }

    private async Task RunDanceAsync()
    {
        danceStart.Enabled = false;
        try
        {
            dancePanel.Controls.Clear(); int count = (int)danceCount.Value; danceBars = new ProgressBar[count];
            for (int i = 0; i < count; i++) { var row = new Panel { Width = 815, Height = 45 }; row.Controls.Add(new Label { Text = $"Индикатор {i + 1}", Left = 0, Top = 12, Width = 100 }); danceBars[i] = new() { Left = 105, Top = 8, Width = 690, Height = 28, Maximum = 100 }; row.Controls.Add(danceBars[i]); dancePanel.Controls.Add(row); }
            var bars = danceBars;
            await Task.WhenAll(bars.Select(bar => Task.Run(async () => { var rng = new Random(Guid.NewGuid().GetHashCode()); int value = 0; for (int step = 0; step < 100; step++) { await Task.Delay(rng.Next(20, 100)); value = Math.Min(100, value + rng.Next(1, 12)); int v = value; Color color = Color.FromArgb(rng.Next(256), rng.Next(256), rng.Next(256)); if (IsDisposed) return; BeginInvoke(() => { if (!bar.IsDisposed) { bar.Value = v; bar.ForeColor = color; } }); } })));
        }
        catch (Exception ex) when (ex is InvalidOperationException || ex is ObjectDisposedException) { }
        finally { if (!IsDisposed) danceStart.Enabled = true; }
    }

    private async Task RunRaceAsync()
    {
        raceStart.Enabled = false; foreach (var h in horses) h.Value = 0; raceResults.Text = "Гонка идёт...";
        try
        {
            var finishOrder = new ConcurrentQueue<int>();
            await Task.WhenAll(horses.Select((bar, index) => Task.Run(async () => { var rng = new Random(Guid.NewGuid().GetHashCode()); int value = 0; while (value < 100) { await Task.Delay(rng.Next(70, 180)); value = Math.Min(100, value + rng.Next(1, 9)); int next = value; if (IsDisposed) return; BeginInvoke(() => { if (!bar.IsDisposed) bar.Value = next; }); } finishOrder.Enqueue(index); })));
            raceResults.Text = "Итоговая таблица:\r\n" + string.Join("\r\n", finishOrder.Select((horse, position) => $"{position + 1}. Лошадь {horse + 1}"));
        }
        catch (Exception ex) when (ex is InvalidOperationException || ex is ObjectDisposedException) { }
        finally { if (!IsDisposed) raceStart.Enabled = true; }
    }

    private async Task RunFibAsync()
    {
        fibStart.Enabled = false; long limit = (long)fibLimit.Value;
        try { fibOutput.Text = await Task.Run(() => { var result = new StringBuilder(); BigInteger a = 0, b = 1; while (a <= limit) { result.AppendLine(a.ToString()); (a, b) = (b, a + b); if (result.Length > 2_000_000) return result.AppendLine("... вывод сокращён (слишком много значений)").ToString(); } return result.ToString(); }); }
        catch (Exception ex) { fibOutput.Text = ex.Message; }
        finally { if (!IsDisposed) fibStart.Enabled = true; }
    }

    private void BrowseFile() { using var d = new OpenFileDialog { Title = "Выберите файл", CheckFileExists = true }; if (d.ShowDialog(this) == DialogResult.OK) filePath.Text = d.FileName; }
    private void BrowseFolder() { using var d = new FolderBrowserDialog { Description = "Выберите каталог для рекурсивного поиска" }; if (d.ShowDialog(this) == DialogResult.OK) folderPath.Text = d.SelectedPath; }
    private async Task SearchFileAsync()
    {
        string path = filePath.Text.Trim(), word = fileWord.Text; if (word.Length == 0) { fileOutput.Text = "Введите слово для поиска."; return; }
        fileSearch.Enabled = false;
        try { long count = await Task.Run(() => CountMatches(File.ReadAllText(path), word)); fileOutput.Text = $"Файл: {path}\r\nСлово: {word}\r\nКоличество вхождений: {count}"; } catch (Exception ex) { fileOutput.Text = $"Не удалось выполнить поиск: {ex.Message}"; } finally { fileSearch.Enabled = true; }
    }
    private async Task SearchFolderAsync()
    {
        string root = folderPath.Text.Trim(), word = folderWord.Text; if (word.Length == 0) { folderOutput.Text = "Введите слово для поиска."; return; }
        folderSearch.Enabled = false; folderOutput.Clear();
        try
        {
            var result = await Task.Run(() => SearchTree(root, word));
            folderOutput.Text = result.Count == 0 ? "Совпадений не найдено." : string.Join(Environment.NewLine + Environment.NewLine, result.Select(x => $"Название файла: {Path.GetFileName(x.Path)}\r\nПуть к файлу: {x.Path}\r\nКоличество вхождений слова: {x.Count}"));
        }
        catch (Exception ex) { folderOutput.Text = $"Не удалось выполнить поиск: {ex.Message}"; }
        finally { folderSearch.Enabled = true; }
    }
    private static long CountMatches(string text, string word)
    {
        long count = 0; int at = 0; while ((at = text.IndexOf(word, at, StringComparison.CurrentCultureIgnoreCase)) >= 0) { count++; at += word.Length; }
        return count;
    }
    private static List<(string Path, long Count)> SearchTree(string root, string word)
    {
        if (!Directory.Exists(root)) throw new DirectoryNotFoundException("Каталог не найден."); var results = new List<(string, long)>(); var pending = new Stack<string>(); pending.Push(root);
        while (pending.Count > 0)
        {
            string dir = pending.Pop(); try { foreach (string f in Directory.EnumerateFiles(dir)) { try { long n = CountMatches(File.ReadAllText(f), word); if (n > 0) results.Add((f, n)); } catch (IOException) { } catch (UnauthorizedAccessException) { } } } catch (IOException) { } catch (UnauthorizedAccessException) { }
            try { foreach (string sub in Directory.EnumerateDirectories(dir)) { try { if ((File.GetAttributes(sub) & FileAttributes.ReparsePoint) == 0) pending.Push(sub); } catch (IOException) { } catch (UnauthorizedAccessException) { } } } catch (IOException) { } catch (UnauthorizedAccessException) { }
        }
        return results.OrderBy(x => x.Item1, StringComparer.CurrentCultureIgnoreCase).ToList();
    }
}
