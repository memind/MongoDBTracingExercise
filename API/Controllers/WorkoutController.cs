using Application.Abstractions.Services;
using Application.DTOs.ExerciseDTOs;
using Application.DTOs.WorkoutDTOs;
using Application.VMs.ExerciseVMs;
using Application.VMs.WorkoutVMs;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkoutController : ControllerBase
    {
        private readonly IWorkoutService _service;
        private readonly IMapper _mapper;

        public WorkoutController(IWorkoutService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        [HttpGet]
        public List<WorkoutVM> GetAllWorkouts()
        {
            var workouts = _service.GetAllWorkouts();
            return _mapper.Map<List<WorkoutVM>>(workouts);
        }

        [HttpPost]
        public void CreateWorkout()
        {
            var wo = new WorkoutDTO() 
            { 
                Id = ObjectId.GenerateNewId().ToString(),
                CreatedDate = DateTime.Now,
                Name = "Workout Test 2",
                Description = "Description",
                DayOfWeek = new List<Days>() { Days.Monday, Days.Wednesday, Days.Friday },
            };

            _service.CreateWorkout(wo);
        }

        [HttpPut]
        public WorkoutDTO AddExerciseToWorkout()
        {
            var exercise = _service.GetAllExercises().First();
            var map = _mapper.Map<Exercise>(exercise);
            var workout = _service.GetAllWorkouts().First();

            workout.Exercises.Add(map);

            return _service.UpdateWorkout(workout.Id,workout);
        }

    }
}
