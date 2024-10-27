﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Dto.Recipe
{
    public class AllRecipeForStaffResponseDTO
    {
        public int RecipeId { get; set; }
        public string FoodName { get; set; }
        public string RecipeImage { get; set; } = null!;
        public string CreateBy { get; set; }
        public DateTime CreateDate { get; set; }
        public string? ChangeBy { get; set; }
        public DateTime? ChangeDate { get; set; }
        public string RecipeName { get; set; } = null!;
     
        
        
        public bool? Status { get; set; }
    }
}
