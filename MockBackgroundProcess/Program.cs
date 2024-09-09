namespace MockBackgroundProcess
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            // Set up a cancellation token source to allow graceful shutdown
            var cts = new CancellationTokenSource();

            // Start a task to handle graceful shutdown on user interrupt
            Task.Run(() =>
            {
                Console.CancelKeyPress += (sender, eventArgs) =>
                {
                    Console.WriteLine("Cancellation requested...");
                    cts.Cancel();
                    eventArgs.Cancel = true; // Prevent the process from terminating
                };
            });

            Console.WriteLine("MockProcess Logging started. Press Ctrl+C to exit.");

            try
            {
                // Main loop for continuous logging
                while (!cts.Token.IsCancellationRequested)
                {
                    Console.WriteLine($"MockProcess Log entry at {DateTime.UtcNow}");
                    await Task.Delay(1000); // Log every second
                }
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine("Logging stopped.");
            }
        }
    }
}
