#pragma warning disable CS8618
using System.Numerics;

namespace Module03_Threads;

public partial class Form1 : Form
{
    private readonly ManualResetEventSlim primeGate = new(initialState: true);
    private readonly ManualResetEventSlim fibonacciGate = new(initialState: true);

    private CancellationTokenSource? primeCancellation;
    private CancellationTokenSource? fibonacciCancellation;

    public Form1()
    {
        InitializeComponent();
    }

    private void StartPrime()
    {
        StopPrime();
        primeList.Items.Clear();

        long first = decimal.ToInt64(primeStart.Value);
        long last = decimal.ToInt64(primeEnd.Value);
        bool hasNoUpperLimit = openPrimeEnd.Checked;

        var cancellation = new CancellationTokenSource();
        primeCancellation = cancellation;
        primeGate.Set();

        Task.Factory.StartNew(
            () => GeneratePrimes(first, last, hasNoUpperLimit, cancellation.Token),
            CancellationToken.None,
            TaskCreationOptions.LongRunning,
            TaskScheduler.Default);

        PostStatus("Генерация простых чисел запущена.");
    }

    private void GeneratePrimes(
        long first,
        long last,
        bool hasNoUpperLimit,
        CancellationToken cancellationToken)
    {
        try
        {
            for (long number = Math.Max(2, first);
                 !cancellationToken.IsCancellationRequested
                 && (hasNoUpperLimit || number <= last);
                 number++)
            {
                primeGate.Wait(cancellationToken);

                if (IsPrime(number))
                {
                    Post(primeList, number.ToString());
                }

                if (number == long.MaxValue)
                {
                    break;
                }
            }
        }
        catch (OperationCanceledException)
        {
            // The stop button cancels the worker and releases its pause gate.
        }
    }

    private void StartFib()
    {
        StopFib();
        fibonacciList.Items.Clear();

        long last = decimal.ToInt64(fibonacciEnd.Value);
        bool hasNoUpperLimit = openFibonacciEnd.Checked;

        var cancellation = new CancellationTokenSource();
        fibonacciCancellation = cancellation;
        fibonacciGate.Set();

        Task.Factory.StartNew(
            () => GenerateFibonacci(last, hasNoUpperLimit, cancellation.Token),
            CancellationToken.None,
            TaskCreationOptions.LongRunning,
            TaskScheduler.Default);

        PostStatus("Генерация чисел Фибоначчи запущена.");
    }

    private void GenerateFibonacci(
        long last,
        bool hasNoUpperLimit,
        CancellationToken cancellationToken)
    {
        BigInteger first = 0;
        BigInteger second = 1;

        try
        {
            while (!cancellationToken.IsCancellationRequested
                   && (hasNoUpperLimit || first <= last))
            {
                fibonacciGate.Wait(cancellationToken);

                if (cancellationToken.IsCancellationRequested)
                {
                    break;
                }

                Post(fibonacciList, first.ToString());
                (first, second) = (second, first + second);
                Thread.Sleep(250);
            }
        }
        catch (OperationCanceledException)
        {
            // The stop button cancels the worker and releases its pause gate.
        }
    }

    private static bool IsPrime(long number)
    {
        if (number < 2)
        {
            return false;
        }

        if (number % 2 == 0)
        {
            return number == 2;
        }

        for (long divisor = 3; divisor <= number / divisor; divisor += 2)
        {
            if (number % divisor == 0)
            {
                return false;
            }
        }

        return true;
    }

    private void StopPrime()
    {
        primeCancellation?.Cancel();
        primeGate.Set();
        primeCancellation = null;
    }

    private void StopFib()
    {
        fibonacciCancellation?.Cancel();
        fibonacciGate.Set();
        fibonacciCancellation = null;
    }

    private void RestartAll()
    {
        StartPrime();
        StartFib();
        PostStatus("Оба потока перезапущены с указанными границами.");
    }

    private void StopAll()
    {
        StopPrime();
        StopFib();
        primeGate.Dispose();
        fibonacciGate.Dispose();
    }

    private void Post(ListBox list, string value)
    {
        if (IsDisposed || !IsHandleCreated)
        {
            return;
        }

        try
        {
            BeginInvoke(() =>
            {
                if (list.IsDisposed)
                {
                    return;
                }

                if (list.Items.Count >= 5000)
                {
                    list.Items.RemoveAt(0);
                }

                list.Items.Add(value);
                list.TopIndex = list.Items.Count - 1;
            });
        }
        catch (InvalidOperationException)
        {
            // The form can close while a worker is posting its final value.
        }
    }

    private void PostStatus(string value)
    {
        if (IsDisposed || !IsHandleCreated)
        {
            return;
        }

        try
        {
            BeginInvoke(() =>
            {
                if (!status.IsDisposed)
                {
                    status.Text = value;
                }
            });
        }
        catch (InvalidOperationException)
        {
            // The form can close while a worker is posting its status.
        }
    }
}
