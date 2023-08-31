using Application.Abstractions.Services;
using AutoMapper.Configuration.Annotations;
using Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DapperController : ControllerBase
    {
        private IEntityForDapperService _service;

        public DapperController(IEntityForDapperService service)
        {
            _service = service;
        }

        [HttpGet("/getOneEntity")]
        public EntityForDapper GetOneEntity(string id)
        {
            return _service.GetById(id);
        }

        [HttpGet("/getAllEntities")]
        public List<EntityForDapper> GetAllEntities()
        {
            return _service.GetAll();
        }

        [HttpPut("/updateEntity")]
        public int PutEntity(string id)
        {
            var entity = _service.GetById(id);
            entity.Name = "Updated";
            entity.Description = "Updated";
            return _service.Update(id, entity);
        }

        [HttpPost("/createEntity")]
        public int PostEntity()
        {
            Random rnd = new Random();
            EntityForDapper entity = new EntityForDapper() 
            {
                Id = Guid.NewGuid(),
                CreatedDate = DateTime.Now,
                Name = $"AddedEntity{rnd.Next(1000)}",
                Description = "If you see me, ITS TOO LATE!!!",
                DapperBool = true
            };
            return _service.Create(entity);
        }

        [HttpDelete("/deleteEntity")]
        public int DeleteEntity(string id)
        {
            return _service.Delete(Guid.Parse(id));
        }
    }
}
