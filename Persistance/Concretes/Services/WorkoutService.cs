using Application.Abstractions.Services;
using Application.DTOs.ExerciseDTOs;
using Application.DTOs.WorkoutDTOs;
using Application.Repositories.ExerciseRepositories;
using Application.Repositories.WorkoutRepositories;
using AutoMapper;
using AutoMapper.Configuration.Annotations;
using Domain.Entities;
using SharpCompress.Writers;
using System.Threading;

namespace Persistance.Concretes.Services
{
    public class WorkoutService : IWorkoutService
    {
        private readonly IWorkoutReadRepository _workoutRead;
        private readonly IWorkoutWriteRepository _workoutWrite;
        private readonly IExerciseReadRepository _exerciseRead;
        private readonly IExerciseWriteRepository _exerciseWrite;
        private readonly IMapper _mapper;

        public WorkoutService(IWorkoutReadRepository read, IWorkoutWriteRepository write, IMapper mapper, IExerciseReadRepository exerciseRead, IExerciseWriteRepository exerciseWrite)
        {
            _workoutRead = read;
            _workoutWrite = write;
            _mapper = mapper;
            _exerciseRead = exerciseRead;
            _exerciseWrite = exerciseWrite;
        }

        public WorkoutDTO CreateWorkout(WorkoutDTO newWorkout)
        {
            var map = _mapper.Map<Workout>(newWorkout);
            var result = _workoutWrite.InsertOne(map);

            return _mapper.Map<WorkoutDTO>(result.Data);
        }

        public async Task<WorkoutDTO> CreateWorkoutAsync(WorkoutDTO newWorkout)
        {
            var map = _mapper.Map<Workout>(newWorkout);
            var result = await _workoutWrite.InsertOneAsync(map);

            return _mapper.Map<WorkoutDTO>(result.Data);
        }

        public void DeleteWorkout(string id)
        {
            _workoutWrite.DeleteOne(x => x.Id == id);
        }

        public async Task DeleteWorkoutAsync(string id)
        {
            await _workoutWrite.DeleteOneAsync(x => x.Id == id);
        }

        public List<WorkoutDTO> GetAllWorkouts()
        {
            List<Workout> workouts = _workoutRead.GetAll().Data.ToList();
            return _mapper.Map<List<WorkoutDTO>>(workouts);
        }

        public async Task<List<WorkoutDTO>> GetAllWorkoutsAsync()
        {
            List<Workout> workouts = (await _workoutRead.GetAllAsync()).Data.ToList();
            return _mapper.Map<List<WorkoutDTO>>(workouts);
        }

        public WorkoutDTO GetWorkoutById(string id)
        {
            var workout = _workoutRead.GetById(id.ToString()).Data;
            return _mapper.Map<WorkoutDTO>(workout);
        }

        public async Task<WorkoutDTO> GetWorkoutByIdAsync(string id)
        {
            var workout = (await _workoutRead.GetByIdAsync(id.ToString())).Data;
            return _mapper.Map<WorkoutDTO>(workout);
        }

        public WorkoutDTO UpdateWorkout(string id, WorkoutDTO updating)
        {
            var map = _mapper.Map<Workout>(updating);
            var result = _workoutWrite.ReplaceOne(map, id.ToString()).Data;

            return _mapper.Map<WorkoutDTO>(result);
        }

        public async Task<WorkoutDTO> UpdateWorkoutAsync(string id)
        {
            var workout = (await _workoutRead.GetByIdAsync(id.ToString())).Data;
            var result = (await _workoutWrite.ReplaceOneAsync(workout, id.ToString())).Data;

            return _mapper.Map<WorkoutDTO>(result);
        }




        public ExerciseDTO CreateExercise(ExerciseDTO newExercise)
        {
            var map = _mapper.Map<Exercise>(newExercise);
            var result = _exerciseWrite.InsertOne(map);

            return _mapper.Map<ExerciseDTO>(result.Data);
        }

        public async Task<ExerciseDTO> CreateExerciseAsync(ExerciseDTO newExercise)
        {
            var exercise = _mapper.Map<Exercise>(newExercise);
            var result = await _exerciseWrite.InsertOneAsync(exercise);

            return _mapper.Map<ExerciseDTO>(result.Data);
        }

        public void DeleteExercise(string id)
        {
            _exerciseWrite.DeleteOne(x => x.Id == id);
        }

        public async Task DeleteExerciseAsync(string id)
        {
            await _exerciseWrite.DeleteOneAsync(x => x.Id == id);
        }

        public List<ExerciseDTO> GetAllExercises()
        {
            List<Exercise> exercises = _exerciseRead.GetAll().Data.ToList();
            return _mapper.Map<List<ExerciseDTO>>(exercises);
        }

        public async Task<List<ExerciseDTO>> GetAllExercisesAsync()
        {
            var exercises = (await _exerciseRead.GetAllAsync()).Data;
            return _mapper.Map<List<ExerciseDTO>>(exercises);
        }

        public List<ExerciseDTO> GetAllExercisesInWorkout(string id)
        {
            var exercises = _exerciseRead.GetAll().Data.Where(x => x.WorkoutId == id);
            return _mapper.Map<List<ExerciseDTO>>(exercises);
        }

        public async Task<List<ExerciseDTO>> GetAllExercisesInWorkoutAsync(string id)
        {
            var exercises = (await _exerciseRead.GetAllAsync()).Data.Where(x => x.WorkoutId == id);
            return _mapper.Map<List<ExerciseDTO>>(exercises);
        }

        public ExerciseDTO GetExerciseById(string id)
        {
            var exercise = _exerciseRead.GetById(id.ToString());
            return _mapper.Map<ExerciseDTO>(exercise.Data);
        }

        public async Task<ExerciseDTO> GetExerciseByIdAsync(string id)
        {
            var exercise = await _exerciseRead.GetByIdAsync(id.ToString());
            return _mapper.Map<ExerciseDTO>(exercise.Data);
        }

        public ExerciseDTO UpdateExercise(string id)
        {
            var exercise = GetExerciseById(id);
            return _mapper.Map<ExerciseDTO>(
                _exerciseWrite.ReplaceOne(
                    _mapper.Map<Exercise>(exercise),
                        id.ToString())
                            .Data);
        }

        public async Task<ExerciseDTO> UpdateExerciseAsync(string id)
        {
            var exercise = GetExerciseById(id);
            var map = _mapper.Map<Exercise>(exercise);
            var result = await _exerciseWrite.ReplaceOneAsync(map, id.ToString());

            return _mapper.Map<ExerciseDTO>(result.Data);
        }
    }
}
