using Domain.Entities.Common;
using System.Linq.Expressions;

namespace Application.Repositories
{
    public interface IReadRepository<T> where T : BaseEntity, new()
    {
        GetManyResult<T> GetAll();
        Task<GetManyResult<T>> GetAllAsync();

        GetManyResult<T> FilterBy(Expression<Func<T, bool>> filter);
        Task<GetManyResult<T>> FilterByAsync(Expression<Func<T, bool>> filter);

        GetOneResult<T> GetById(string id, string type = "object");
        Task<GetOneResult<T>> GetByIdAsync(string id, string type = "object");
    }
}
