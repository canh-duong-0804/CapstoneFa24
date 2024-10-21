using System;
using System.Collections.Generic;

namespace BusinessObject.Entities
{
    public partial class Tag
    {
        public Tag()
        {
            Foods = new HashSet<Food>();
        }

        public int TagId { get; set; }
        public string? FoodTagName { get; set; }

        public virtual ICollection<Food> Foods { get; set; }
    }
}
