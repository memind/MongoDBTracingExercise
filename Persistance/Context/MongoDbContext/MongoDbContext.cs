using MongoDB.Driver;
using Microsoft.Extensions.Options;

namespace Persistance.Context.MongoDbContext
{
    public class MongoDbContext
    {
        private readonly IMongoDatabase _db;

        public MongoDbContext(IOptions<MongoSettings> settings)
        {
            var client = new MongoClient(settings.Value.ConnectionString);
            _db = client.GetDatabase(settings.Value.Database);
        }

        public IMongoCollection<T> GetCollection<T>()
        {
            return _db.GetCollection<T>(typeof(T).Name.Trim());
        }

        public IMongoDatabase GetDatabase()
        {
            return _db;
        }
    }
}
