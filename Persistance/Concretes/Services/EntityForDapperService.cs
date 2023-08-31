

using Application.Abstractions.Services;
using Application.Repositories.EntityForDapperRepositories;
using Domain.Entities;

namespace Persistance.Concretes.Services
{
    public class EntityForDapperService : IEntityForDapperService
    {
        private readonly IEntityForDapperRepository _repo;

        public EntityForDapperService(IEntityForDapperRepository repo)
        {
            _repo = repo;
        }

        public int Create(EntityForDapper entity)
        {
            return _repo.Create(entity);
        }

        public int Delete(Guid id)
        {
            return _repo.Delete(id);
        }

        public List<EntityForDapper> GetAll()
        {
            return _repo.GetAll();
        }

        public EntityForDapper GetById(string id)
        {
            return _repo.GetById(id);
        }

        public int Update(string id, EntityForDapper entity)
        {
            return _repo.Update(id, entity);
        }
    }
}
