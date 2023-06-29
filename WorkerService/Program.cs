using WorkerService;

public class Program
{
    public static void Main(string[] args)
    {
        CreateHostBuilder(args).Build().Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureServices((hostContext, services) => {
                IConfiguration configuration = hostContext.Configuration;

                services.AddHostedService<Worker>();
                services.AddInfra(configuration);
            });
}
