using Domain.Entities;
using Domain.Enums;

namespace Application.DTOs.WorkoutDTOs
{
    public class WorkoutDTO
    {
        public WorkoutDTO()
        {
            DayOfWeek = new List<Days>();
            Exercises = new List<Exercise>();
        }
        public string Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public IEnumerable<Days> DayOfWeek { get; set; }
        public List<Exercise> Exercises { get; set; }
    }
}
