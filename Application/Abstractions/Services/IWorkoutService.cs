using Application.DTOs.ExerciseDTOs;
using Application.DTOs.WorkoutDTOs;

namespace Application.Abstractions.Services
{
    public interface IWorkoutService
    {
        public List<WorkoutDTO> GetAllWorkouts();
        public Task<List<WorkoutDTO>> GetAllWorkoutsAsync();

        public WorkoutDTO GetWorkoutById(string id);
        public Task<WorkoutDTO> GetWorkoutByIdAsync(string id);

        public WorkoutDTO CreateWorkout(WorkoutDTO newWorkout);
        public Task<WorkoutDTO> CreateWorkoutAsync(WorkoutDTO newWorkout);

        public void DeleteWorkout(string id);
        public Task DeleteWorkoutAsync(string id);

        public WorkoutDTO UpdateWorkout(string id, WorkoutDTO updating);
        public Task<WorkoutDTO> UpdateWorkoutAsync(string id);


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
