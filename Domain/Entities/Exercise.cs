using Domain.Entities.Common;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    public class Exercise : BaseEntity
    {
        public int Sets { get; set; }
        public int Reps { get; set; }
        public int CurrentWeight { get; set; }
        public int Order { get; set; }

        public string WorkoutId { get; set; }

        [BsonIgnore]
        [NotMapped]
        public Workout? Workout { get; set; }
    }
}
