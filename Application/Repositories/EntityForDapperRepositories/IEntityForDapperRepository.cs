using Domain.Entities;

namespace Application.Repositories.EntityForDapperRepositories
{
    public interface IEntityForDapperRepository
    {
        List<EntityForDapper> GetAll();

        EntityForDapper GetById(string id);

        int Create(EntityForDapper entity);

        int Update(string id, EntityForDapper entity);

        int Delete(Guid id);
    }
}
