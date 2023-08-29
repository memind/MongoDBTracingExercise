using Application.DTOs.ExerciseDTOs;
using Domain.Entities;
using Domain.Entities.Common;

namespace Application.Abstractions.Services
{
    public interface IExerciseService
    {
        public List<ExerciseDTO> GetAllExercises();
        public Task<List<ExerciseDTO>> GetAllExercisesAsync();

        public ExerciseDTO GetExerciseById(string id);
        public Task<ExerciseDTO> GetExerciseByIdAsync(string id);

        public List<ExerciseDTO> GetAllExercisesInWorkout(string id);
        public Task<List<ExerciseDTO>> GetAllExercisesInWorkoutAsync(string id);


        public ExerciseDTO CreateExercise(ExerciseDTO newExercise);
        public Task<ExerciseDTO> CreateExerciseAsync(ExerciseDTO newExercise);

        public void DeleteExercise(string id);
        public Task DeleteExerciseAsync(string id);

        public ExerciseDTO UpdateExercise(string id);
        public Task<ExerciseDTO> UpdateExerciseAsync(string id);
    }
}
