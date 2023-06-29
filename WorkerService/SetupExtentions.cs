using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class SetupExtentions
    {
        public static IServiceCollection AddInfra(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<MongoClusterDatabaseSettings>(options =>
            {
                options.DatabaseName = configuration.GetSection("ClientClusterDatabase:DatabaseName").Value;
                options.ClientCollectionName = configuration.GetSection("ClientClusterDatabase:CollectionName").Value;
                options.ConnectionString = configuration.GetSection("ClientClusterDatabase:ConnectionString").Value;
            });

            services.Configure<CloudAmqpSettings>(options => {

                options.ConnectionString = configuration.GetSection("AmqpQueueCluster:ConnectionString").Value;
                options.Queue = configuration.GetSection("AmqpQueueCluster:Queue").Value;

            });

            return services;
        }

        public class MongoClusterDatabaseSettings
        {
            public string? DatabaseName { get; set;}
            public string? ClientCollectionName { get; set;}
            public string? ConnectionString { get; set;}
        }

        public class CloudAmqpSettings
        {
            public string? ConnectionString { get; set; }
            public string? Queue { get; set; }

            public bool Durable = true;
            public bool Exclusive = false;
            public bool AutoDelete = false;
        }
    }
}
