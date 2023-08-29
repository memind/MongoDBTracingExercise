using Domain.Entities;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.VMs.WorkoutVMs
{
    public class WorkoutVM
    {
        public string Name { get; set; }
        public string Description { get; set; }

        public IEnumerable<Days> DayOfWeek { get; set; }
        public IEnumerable<Exercise> Exercises { get; set; }
    }
}
