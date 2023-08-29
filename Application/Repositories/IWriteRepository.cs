using Domain.Entities.Common;
using System.Linq.Expressions;

namespace Application.Repositories
{
    public interface IWriteRepository<T> where T : BaseEntity, new()
    {
        GetOneResult<T> InsertOne(T entity);
        Task<GetOneResult<T>> InsertOneAsync(T entity);

        GetManyResult<T> InsertMany(ICollection<T> entities);
        Task<GetManyResult<T>> InsertManyAsync(ICollection<T> entities);

        GetOneResult<T> ReplaceOne(T entity, string id, string type = "object");
        Task<GetOneResult<T>> ReplaceOneAsync(T entity, string id, string type = "object");

        GetOneResult<T> DeleteOne(Expression<Func<T, bool>> filter);
        Task<GetOneResult<T>> DeleteOneAsync(Expression<Func<T, bool>> filter);

        GetOneResult<T> DeleteById(string id);
        Task<GetOneResult<T>> DeleteByIdAsync(string id);

        void DeleteMany(Expression<Func<T, bool>> filter);
        Task DeleteManyAsync(Expression<Func<T, bool>> filter);
    }
}
