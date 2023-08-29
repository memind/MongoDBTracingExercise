using API.Extensions;
using API.Models;
using Application.DTOs.WorkoutDTOs;
using Application.Repositories.ExerciseRepositories;
using Application.Repositories.WorkoutRepositories;
using AutoMapper;
using Domain.Entities;
using Domain.Entities.Common;
using Domain.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using OpenTracing;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private List<string> _list = new List<string>() { "Tracing ", "is ", "started ", "successfully" };
        private readonly ITracer _tracer;
        private readonly IHttpClientFactory _clientFactory;

        private readonly IWorkoutReadRepository _readWorkout;
        private readonly IWorkoutWriteRepository _writeWorkout;

        private readonly IExerciseReadRepository _readExercise;
        private readonly IExerciseWriteRepository _writeExercise;
        private readonly IMapper _mapper;

        public TestController(IWorkoutReadRepository read, IWorkoutWriteRepository write, IExerciseReadRepository readExercise, IExerciseWriteRepository writeExercise, IMapper mapper, ITracer tracer, IHttpClientFactory clientFactory)
        {
            _readWorkout = read;
            _writeWorkout = write;
            _readExercise = readExercise;
            _writeExercise = writeExercise;
            _mapper = mapper;
            _tracer = tracer;
            _clientFactory = clientFactory;
        }

        [HttpGet("/getAll")]
        public GetManyResult<Workout> GetAll()
        {
            return _readWorkout.GetAll();
        }

        [HttpGet("/getOne")]
        public GetOneResult<Workout> GetOne()
        {
            return _readWorkout.GetById("64eccb155a36d9f78ef3b058");
        }

        [HttpDelete("/delete")]
        public GetOneResult<Workout> Delete()
        {
            return _writeWorkout.DeleteById("64eccb3f1732eda824c1249d");
        }
        [HttpPost]
        public GetOneResult<Workout> Post()
        {
            var newId = ObjectId.GenerateNewId().ToString();
            Random rnd = new Random();
            var w = new Workout()
            {
                Id = newId,
                Name = $"workout-test{rnd.Next()}",
                Description = $"workout-test{rnd.Next()}",
                DayOfWeek = new List<Days>() { Days.Sunday },
            };

            var ex1Count = rnd.Next();
            var exercise1 = new Exercise()
            {
                Name = $"exercise-test{ex1Count}",
                Description = $"exercise-test{ex1Count}",
                Sets = 3,
                Reps = 10,
                CurrentWeight = 10,
                Order = 1,
                WorkoutId = newId
            };


            var ex2Count = rnd.Next();
            var exercise2 = new Exercise()
            {
                Name = $"exercise-test{ex2Count}",
                Description = $"exercise-test{ex2Count}",
                Sets = 4,
                Reps = 12,
                CurrentWeight = 20,
                Order = 2,
                WorkoutId = newId
            };

            w.Exercises.Add(exercise1);
            w.Exercises.Add(exercise2);

            _writeExercise.InsertMany(w.Exercises);
            return _writeWorkout.InsertOne(w);
        }

        [HttpPut("{id}")]
        public GetOneResult<Workout> Update(string id)
        {
            WorkoutDTO dto = new WorkoutDTO()
            {
                Id = id,
                CreatedDate = DateTime.UtcNow,
                Name = "UpdatedWorkout",
                Description = "Updated Workout Description",

                DayOfWeek = new List<Days>() { Days.Tuesday },
                Exercises = null
            };

            var workout = _mapper.Map<Workout>(dto);
            return _writeWorkout.ReplaceOne(workout, id);
        }



        [HttpGet("/getAllWithConsule")]
        public async Task<GetManyResult<Workout>> GetAllWithConsule()
        {

            var consulDemoKey = await ConsulKeyValueProvider.GetValueAsync<ConsulDemoKey>(key: "ConsulDemoKey");
            var result = _readWorkout.GetAll();

            if (consulDemoKey != null && consulDemoKey.IsEnabled)
                result.Message = consulDemoKey.Message;

            return result;
        }

        [HttpGet]
        public List<string> Get()
        {
            var actionName = ControllerContext.ActionDescriptor.DisplayName;
            using var scope = _tracer.BuildSpan(actionName).StartActive(true);
            var client = _clientFactory.CreateClient("logService");
            return _list;

        }
    }
}
