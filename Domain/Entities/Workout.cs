using Domain.Entities.Common;
using Domain.Enums;

namespace Domain.Entities
{
    public class Workout : BaseEntity
    {
        public Workout()
        {
            DayOfWeek = new List<Days>();
            Exercises = new List<Exercise>();
        }
        public List<Days>? DayOfWeek { get; set; }
        public List<Exercise>? Exercises { get; set; }
    }
}
