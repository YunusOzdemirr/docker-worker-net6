using System.Security.Cryptography;
using System.Text;

namespace docker_worker_net6;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;

    public Worker(ILogger<Worker> logger)
    {
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            for (int i = 0; i < 10; i++)
            {
                var bytearray = GetHashString(i+1+DateTime.Now.Second.ToString()+i);
                _logger.LogInformation(bytearray);
                await Task.Delay(50, stoppingToken);
            }
            _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
        }
    }
    
    public static byte[] GetHash(string inputString)
    {
        using (HashAlgorithm algorithm = SHA256.Create())
            return algorithm.ComputeHash(Encoding.UTF8.GetBytes(inputString));
    }

    public static string GetHashString(string inputString)
    {
        StringBuilder sb = new StringBuilder();
        foreach (byte b in GetHash(inputString))
            sb.Append(b.ToString("X2"));

        return sb.ToString();
    }
}