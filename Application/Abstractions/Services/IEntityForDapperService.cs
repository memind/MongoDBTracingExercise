using Domain.Entities;

namespace Application.Abstractions.Services
{
    public interface IEntityForDapperService
    {
        List<EntityForDapper> GetAll();

        EntityForDapper GetById(string id);

        int Create(EntityForDapper entity);

        int Update(string id, EntityForDapper entity);

        int Delete(Guid id);
    }
}
