using Amazon.Runtime.Internal.Endpoints.StandardLibrary;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;



namespace WorkerService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;

        private readonly IMongoCollection<Client> _clientsCollection;

        private ManualResetEvent _resetEvent = null;

        private IConnection _connection;

        private IModel _channel;

        private string routingKey;

        public Worker(ILogger<Worker> logger, IOptions<SetupExtentions.MongoClusterDatabaseSettings> clientStoreDatabaseSettings, IOptions<SetupExtentions.CloudAmqpSettings> amqp)
        {

            routingKey = amqp.Value.Queue;  

            var factory = new ConnectionFactory
            {
                Uri = new Uri(amqp.Value.ConnectionString)
            };

            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            _channel.QueueDeclare(amqp.Value.Queue, amqp.Value.Durable, amqp.Value.Exclusive, amqp.Value.AutoDelete, null);
            
            var mongoClient = new MongoClient(
            clientStoreDatabaseSettings.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(
                clientStoreDatabaseSettings.Value.DatabaseName);

            _clientsCollection = mongoDatabase.GetCollection<Client>(
                clientStoreDatabaseSettings.Value.ClientCollectionName);

            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var Clients = await  _clientsCollection.Find(_ => true).ToListAsync();

            List<Client> ClientFiltered = Clients.FindAll(x => x.Birthday.Value.Day == DateTime.Now.Day && x.Birthday.Value.Month == DateTime.Now.Month);
            
            string jsonString = JsonSerializer.Serialize(ClientFiltered);

            ClientFiltered.RemoveAll(_ => true);

            _channel.BasicPublish("", routingKey, null, Encoding.UTF8.GetBytes(jsonString));

            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                Console.WriteLine(Clients);  
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}