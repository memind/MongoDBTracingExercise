using Application.Abstractions.Services;
using Application.DTOs.ExerciseDTOs;
using Application.Repositories.ExerciseRepositories;
using AutoMapper;
using Domain.Entities;

namespace Persistance.Concretes.Services
{
    public class ExerciseService : IExerciseService
    {
        private readonly IExerciseReadRepository _read;
        private readonly IExerciseWriteRepository _write;
        private readonly IMapper _mapper;

        public ExerciseService(IExerciseWriteRepository write, IExerciseReadRepository read, IMapper mapper)
        {
            _write = write;
            _read = read;
            _mapper = mapper;
        }

        public ExerciseDTO CreateExercise(ExerciseDTO newExercise)
        {
            var map = _mapper.Map<Exercise>(newExercise);
            var result = _write.InsertOne(map);

            return _mapper.Map<ExerciseDTO>(result.Data);
        }

        public async Task<ExerciseDTO> CreateExerciseAsync(ExerciseDTO newExercise)
        {
            var exercise = _mapper.Map<Exercise>(newExercise);
            var result = await _write.InsertOneAsync(exercise);

            return _mapper.Map<ExerciseDTO>(result.Data);
        }

        public void DeleteExercise(string id)
        {
            _write.DeleteOne(x => x.Id == id);
        }

        public async Task DeleteExerciseAsync(string id)
        {
            await _write.DeleteOneAsync(x => x.Id == id);
        }

        public List<ExerciseDTO> GetAllExercises()
        {
            List<Exercise> exercises = _read.GetAll().Data.ToList();
            return _mapper.Map<List<ExerciseDTO>>(exercises);
        }

        public async Task<List<ExerciseDTO>> GetAllExercisesAsync()
        {
            var exercises = (await _read.GetAllAsync()).Data;
            return _mapper.Map<List<ExerciseDTO>>(exercises);
        }

        public List<ExerciseDTO> GetAllExercisesInWorkout(string id)
        {
            var exercises = _read.GetAll().Data.Where(x => x.WorkoutId == id);
            return _mapper.Map<List<ExerciseDTO>>(exercises);
        }

        public async Task<List<ExerciseDTO>> GetAllExercisesInWorkoutAsync(string id)
        {
            var exercises = (await _read.GetAllAsync()).Data.Where(x => x.WorkoutId == id);
            return _mapper.Map<List<ExerciseDTO>>(exercises);
        }

        public ExerciseDTO GetExerciseById(string id)
        {
            var exercise = _read.GetById(id.ToString());
            return _mapper.Map<ExerciseDTO>(exercise.Data);
        }

        public async Task<ExerciseDTO> GetExerciseByIdAsync(string id)
        {
            var exercise = await _read.GetByIdAsync(id.ToString());
            return _mapper.Map<ExerciseDTO>(exercise.Data);
        }

        public ExerciseDTO UpdateExercise(string id)
        {
            var exercise = GetExerciseById(id);
            return _mapper.Map<ExerciseDTO>(
                _write.ReplaceOne(
                    _mapper.Map<Exercise>(exercise),
                        id.ToString())
                            .Data);
        }

        public async Task<ExerciseDTO> UpdateExerciseAsync(string id)
        {
            var exercise = GetExerciseById(id);
            var map = _mapper.Map<Exercise>(exercise);
            var result = await _write.ReplaceOneAsync(map, id.ToString());
            
            return _mapper.Map<ExerciseDTO>(result.Data);
        }
    }
}
