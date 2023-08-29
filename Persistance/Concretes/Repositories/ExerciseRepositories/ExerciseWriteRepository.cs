using Application.Repositories.ExerciseRepositories;
using Domain.Entities;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Persistance.Concretes.Repositories.BaseRepositories.MongoRepositoryBase;
using Persistance.Context.MongoDbContext;

namespace Persistance.Concretes.Repositories.ExerciseRepositories
{
    public class ExerciseWriteRepository : MongoWriteRepositoryBase<Exercise>, IExerciseWriteRepository
    {
        private readonly MongoDbContext _context;
        private readonly IMongoCollection<Exercise> _collection;
        public ExerciseWriteRepository(IOptions<MongoSettings> settings) : base(settings)
        {
            _context = new MongoDbContext(settings);
            _collection = _context.GetCollection<Exercise>();
        }
    }
}
