using Application.Repositories.ExerciseRepositories;
using Domain.Entities;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Persistance.Concretes.Repositories.BaseRepositories.MongoRepositoryBase;
using Persistance.Context.MongoDbContext;

namespace Persistance.Concretes.Repositories.ExerciseRepositories
{
    public class ExerciseReadRepository :MongoReadRepositoryBase<Exercise>, IExerciseReadRepository
    {
        private readonly MongoDbContext _context;
        private readonly IMongoCollection<Exercise> _collection;
        public ExerciseReadRepository(IOptions<MongoSettings> settings) : base(settings)
        {
            _context = new MongoDbContext(settings);
            _collection = _context.GetCollection<Exercise>();
        }
    }
}
