using Application.Abstractions.Services;
using Application.VMs.ExerciseVMs;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExerciseController : ControllerBase
    {
        private readonly IExerciseService _service;
        private readonly IMapper _mapper;

        public ExerciseController(IExerciseService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        [HttpGet]
        public List<ExerciseVM> GetAllExercises()
        {
            var exercises = _service.GetAllExercises();
            return _mapper.Map<List<ExerciseVM>>(exercises);
        }
    }
}
