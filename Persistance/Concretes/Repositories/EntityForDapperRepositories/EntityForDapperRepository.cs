using Application.Repositories.EntityForDapperRepositories;
using Dapper;
using Domain.Entities;
using MongoDB.Driver.Core.Configuration;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Persistance.Concretes.Repositories.EntityForDapperRepositories
{
    public class EntityForDapperRepository : IEntityForDapperRepository
    {
        private readonly SqlConnection _dbConnection = new SqlConnection("Server=.;Database=DapperTest;Trusted_Connection=true;Encrypt=false;");
        public int Create(EntityForDapper entity)
        {
            var date = $"{entity.CreatedDate.Year}-{entity.CreatedDate.Month}-{entity.CreatedDate.Day}";
            return _dbConnection.Execute($"INSERT INTO DapperTestTable VALUES('{entity.Id}', '{date}','{entity.Name}', '{entity.Description}','{entity.DapperBool}')");
        }

        public int Delete(Guid id)
        {
            return _dbConnection.Execute($"DELETE FROM DapperTestTable WHERE Id = '{id}'");
        }

        public List<EntityForDapper> GetAll()
        {
            return _dbConnection.Query<EntityForDapper>("SELECT * FROM DapperTestTable").ToList();
        }

        public EntityForDapper GetById(string id)
        {
            Guid guid = Guid.Parse(id);
            return _dbConnection.QueryFirst<EntityForDapper>($"SELECT * FROM DapperTestTable WHERE Id = '{guid.ToString()}'");
        }

        public int Update(string id, EntityForDapper entity)
        {
            var date = $"{entity.CreatedDate.Year}-{entity.CreatedDate.Month}-{entity.CreatedDate.Day}";

            return _dbConnection.Execute($"UPDATE DapperTestTable SET Name = '{entity.Name}', Description = '{entity.Description}', CreatedDate = '{date}', DapperBool = '{entity.DapperBool}' WHERE Id = '{id}'");
        }

        public async Task DeleteAsync(Guid id)
        {
            await _dbConnection.ExecuteAsync($"DELETE FROM DapperTestTable WHERE Id = '{id}'");
        }
    }
}