using System.Collections.Concurrent;
using System.Text;

#pragma warning disable CS8618
namespace Module04_Synchronization;

public partial class Form1 : Form
{
    private readonly ManualResetEvent pairsReady = new(false);
    private readonly AutoResetEvent passengerAdded = new(false);
    private readonly AutoResetEvent busArrived = new(false);
    private readonly ManualResetEvent dayFinished = new(false);
    private readonly object stationLock = new();
    private readonly List<(int Route, int Id)> waiting = new();
    private readonly ConcurrentQueue<int> busRouteQueue = new();
    private readonly Dictionary<int, int> aboard = new() { { 175, 0 }, { 42, 0 }, { 8, 0 } };
    private readonly StringBuilder busLog = new();
    private bool isDayRunning;

    public Form1() { InitializeComponent(); }

    private async Task RunPairsAsync()
    {
        runPairs.Enabled = false; pairOutput.Clear(); int count = (int)pairCount.Value; pairsReady.Reset();
        try
        {
            string dir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "SystemProgramming", "Module04"); Directory.CreateDirectory(dir);
            var paths = await Task.Run(() =>
            {
                var dataPath = Path.Combine(dir, "pairs.txt"); var sumsPath = Path.Combine(dir, "sums.txt"); var productsPath = Path.Combine(dir, "products.txt");
                var generation = Task.Run(() => { try { using var sw = new StreamWriter(dataPath, false, Encoding.UTF8); var rng = new Random(); for (int i = 0; i < count; i++) sw.WriteLine($"{rng.Next(-100, 101)} {rng.Next(-100, 101)}"); } finally { pairsReady.Set(); } });
                var sum = Task.Run(() => { pairsReady.WaitOne(); using var input = new StreamReader(dataPath); using var output = new StreamWriter(sumsPath, false, Encoding.UTF8); string? line; while ((line = input.ReadLine()) != null) { var p = line.Split(' ', StringSplitOptions.RemoveEmptyEntries); output.WriteLine(int.Parse(p[0]) + int.Parse(p[1])); } });
                var product = Task.Run(() => { pairsReady.WaitOne(); using var input = new StreamReader(dataPath); using var output = new StreamWriter(productsPath, false, Encoding.UTF8); string? line; while ((line = input.ReadLine()) != null) { var p = line.Split(' ', StringSplitOptions.RemoveEmptyEntries); output.WriteLine(long.Parse(p[0]) * long.Parse(p[1])); } });
                Task.WaitAll(generation, sum, product); return (dataPath, sumsPath, productsPath);
            });
            pairOutput.Text = $"Сгенерировано пар: {count}\r\nФайл исходных пар: {paths.dataPath}\r\nРезультаты сумм: {paths.sumsPath}\r\nРезультаты произведений: {paths.productsPath}\r\n\r\n" + string.Join("\r\n", File.ReadLines(paths.dataPath).Zip(File.ReadLines(paths.sumsPath).Zip(File.ReadLines(paths.productsPath), (s, p) => (s, p)), (pair, res) => $"{pair} => сумма {res.s}, произведение {res.p}"));
        }
        catch (Exception ex) { pairOutput.Text = "Ошибка обработки: " + ex.Message; }
        finally { runPairs.Enabled = true; }
    }

    private async Task RunBusDayAsync()
    {
        if (isDayRunning) return; isDayRunning = true; runBuses.Enabled = false; busLog.Clear(); waiting.Clear(); busRouteQueue.Clear(); dayFinished.Reset();
        int total = (int)busCount.Value, capacity = (int)busCapacity.Value, initial = Math.Min(capacity, (int)initialPassengers.Value);
        foreach (int route in aboard.Keys.ToArray()) aboard[route] = initial;
        busOutput.Text = "Рабочий день начат...";
        try
        {
            string log = await Task.Run(() => RunDayCore(total, capacity)); busOutput.Text = log;
        }
        catch (Exception ex) { busOutput.Text = "Ошибка моделирования: " + ex.Message; }
        finally { isDayRunning = false; runBuses.Enabled = true; }
    }

    private string RunDayCore(int total, int capacity)
    {
        int[] routes = { 175, 42, 8 };
        var arrivals = Task.Run(() => { var rng = new Random(); int id = 0; while (!dayFinished.WaitOne(220)) { int amount = rng.Next(1, 9); lock (stationLock) { for (int i = 0; i < amount; i++) waiting.Add((routes[rng.Next(routes.Length)], ++id)); } passengerAdded.Set(); } });
        var busWorker = Task.Run(() => { for (int i = 0; i < total; i++) { int signal = WaitHandle.WaitAny(new WaitHandle[] { busArrived, dayFinished }); if (signal == 1) break; if (!busRouteQueue.TryDequeue(out int route)) continue; lock (stationLock) { int before = aboard[route]; int unloaded = Random.Shared.Next(0, before + 1); int free = capacity - (before - unloaded); var selected = waiting.Where(x => x.Route == route).Take(free).ToArray(); foreach (var p in selected) waiting.Remove(p); aboard[route] = before - unloaded + selected.Length; busLog.AppendLine($"Автобус {route}: высадил {unloaded}, забрал {selected.Length}, занято {aboard[route]}/{capacity}. Остались: {waiting.Count}."); } } });
        var dispatcher = Task.Run(() => { for (int i = 0; i < total; i++) { if (dayFinished.WaitOne(520)) break; int route = routes[i % routes.Length]; busRouteQueue.Enqueue(route); busArrived.Set(); } });
        var arrivalMonitor = Task.Run(() => { while (true) { int signal = WaitHandle.WaitAny(new WaitHandle[] { passengerAdded, dayFinished }); if (signal == 1) break; lock (stationLock) busLog.AppendLine($"Событие прибытия пассажиров обработано; на остановке {waiting.Count} человек."); } });
        Task.WaitAll(dispatcher, busWorker); dayFinished.Set(); Task.WaitAll(arrivals, arrivalMonitor);
        lock (stationLock) { busLog.AppendLine(); busLog.AppendLine("День завершён."); busLog.AppendLine($"Осталось пассажиров на остановке: {waiting.Count}."); foreach (var r in routes) busLog.AppendLine($"В автобусах маршрута {r}: {aboard[r]} пассажиров."); }
        return busLog.ToString();
    }
}
