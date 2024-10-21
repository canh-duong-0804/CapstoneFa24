using System;
using System.Collections.Generic;

namespace BusinessObject.Models
{
    public partial class ExerciseCategory
    {
        public ExerciseCategory()
        {
            Exercises = new HashSet<Exercise>();
        }

        public int ExerciseCategoryId { get; set; }
        public string ExerciseCategoryName { get; set; } = null!;

        public virtual ICollection<Exercise> Exercises { get; set; }
    }
}
