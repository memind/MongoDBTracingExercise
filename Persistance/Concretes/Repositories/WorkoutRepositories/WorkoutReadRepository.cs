using Application.Repositories.WorkoutRepositories;
using Domain.Entities;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Persistance.Concretes.Repositories.BaseRepositories.MongoRepositoryBase;
using Persistance.Context.MongoDbContext;

namespace Persistance.Concretes.Repositories.WorkoutRepositories
{
    public class WorkoutReadRepository : MongoReadRepositoryBase<Workout>, IWorkoutReadRepository
    {
        private readonly MongoDbContext _context;
        private readonly IMongoCollection<Workout> _collection;
        public WorkoutReadRepository(IOptions<MongoSettings> settings) : base(settings)
        {
            _context = new MongoDbContext(settings);
            _collection = _context.GetCollection<Workout>();
        }
    }
}
