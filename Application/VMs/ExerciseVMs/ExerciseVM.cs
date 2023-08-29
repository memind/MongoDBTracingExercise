namespace Application.VMs.ExerciseVMs
{
    public class ExerciseVM
    {
        public string Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public int Sets { get; set; }
        public int Reps { get; set; }
        public int CurrentWeight { get; set; }
        public int Order { get; set; }

        public string WorkoutId { get; set; }
    }
}
