using Application.Repositories.WorkoutRepositories;
using Domain.Entities;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Persistance.Concretes.Repositories.BaseRepositories.MongoRepositoryBase;
using Persistance.Context.MongoDbContext;

namespace Persistance.Concretes.Repositories.WorkoutRepositories
{
    public class WorkoutWriteRepository : MongoWriteRepositoryBase<Workout>, IWorkoutWriteRepository
    {
        private readonly MongoDbContext _context;
        private readonly IMongoCollection<Workout> _collection;
        public WorkoutWriteRepository(IOptions<MongoSettings> settings) : base(settings)
        {
            _context = new MongoDbContext(settings);
            _collection = _context.GetCollection<Workout>();
        }
    }
}
