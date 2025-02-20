using System.ComponentModel;

public static class Programm
{
  public static CancellationTokenSource source = new();
  public static CancellationToken token = source.Token;
  public static BackgroundWorker uiWorker = new()
  {
    WorkerSupportsCancellation = true,
  };
  public static BackgroundWorker bgWorker = new()
  {
    WorkerSupportsCancellation = true,
  };

  public static void Main()
  {
    uiWorker.DoWork += UiThread_DoWork;
    uiWorker.RunWorkerCompleted += UiThread_Completed;
    uiWorker.RunWorkerAsync();

    // "main thread"
    while (!uiWorker.CancellationPending) { }
  }

  public static void UiThread_DoWork(object? sender, DoWorkEventArgs eventArgs)
  {
    bgWorker.DoWork += BgThread_DoWork;
    bgWorker.RunWorkerCompleted += BgThread_Completed;
    bgWorker.RunWorkerAsync();
  }

  public static void UiThread_Completed(object? sender, RunWorkerCompletedEventArgs eventArgs)
  {
    Console.WriteLine("Exiting UiThread");
  }

  public static void BgThread_DoWork(object? sender, DoWorkEventArgs eventArgs)
  {
    Console.WriteLine("Starting BgThread");

    // this will not cancel
    var t1 = TaskWorker.Work1();

    // this will cancel
    var t2 = TaskWorker.Work1(token);
    // miminc user calculation
    source.Cancel();

    Task.WaitAll([t1, t2]);
  }

  public static void BgThread_Completed(object? sender, RunWorkerCompletedEventArgs eventArgs)
  {
    Console.WriteLine("Exiting BGThread");
  }
}


public static class TaskWorker
{
  public static void Work1Sync()
  {
    Work1().Wait();
  }

  public async static Task Work1()
  {
    Console.WriteLine("Start Work1");
    await Work2(1);
    Console.WriteLine("End Work1");
  }

  public async static Task Work2(int i)
  {
    Console.WriteLine($"Start Work2({i})");
    await Task.Delay(5000);
    Console.WriteLine($"End Work2({i})");
  }

  public async static Task Work1(CancellationToken cancellationToken)
  {
    Console.WriteLine("Start Work1 with token");
    await Work2(1, cancellationToken);
    Console.WriteLine("End Work1 with token");
  }

  public async static Task Work2(int i, CancellationToken cancellationToken)
  {
    Console.WriteLine($"Start Work2({i}) with token");
    while (!cancellationToken.IsCancellationRequested) { await Task.Delay(1); }
    Console.WriteLine($"Cancelled Work2({i}) with token");
  }
}
