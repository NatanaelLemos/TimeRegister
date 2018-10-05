using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;
using TimeRegister.Domain.Entities;

namespace TimeRegister.Infrastructure.Data
{
    public class MongoDbContext
    {
        private IMongoDatabase _database { get; }

        public MongoDbContext(string connectionString)
        {
            var url = new MongoUrl(connectionString);
            MongoClientSettings settings = MongoClientSettings.FromUrl(url);
            settings.SslSettings = new SslSettings { EnabledSslProtocols = System.Security.Authentication.SslProtocols.Tls12 };
            var mongoClient = new MongoClient(settings);
            _database = mongoClient.GetDatabase("timeRegister");

            BsonClassMap.RegisterClassMap<TimeKeeping>(cm =>
            {
                cm.AutoMap();
                cm.SetIgnoreExtraElements(true);
            });
        }

        private IMongoCollection<T> GetCollection<T>(string collectionName) => _database.GetCollection<T>(collectionName);

        public IMongoCollection<TimeKeeping> Times => GetCollection<TimeKeeping>("times");
    }
}
