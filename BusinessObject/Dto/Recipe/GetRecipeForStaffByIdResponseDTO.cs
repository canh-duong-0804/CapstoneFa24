﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Dto.Recipe
{
    public class GetRecipeForStaffByIdResponseDTO
    {
        public int RecipeId { get; set; }
        public string FoodName { get; set; }
        public string CreateBy { get; set; }
        public string RecipeImage { get; set; } = null!;
        public DateTime CreateDate { get; set; }
        public string? ChangeBy { get; set; }
        public DateTime? ChangeDate { get; set; }
        public string RecipeName { get; set; } = null!;
        public string? Description { get; set; }
        public string Instructions { get; set; } = null!;
        public int? PrepTime { get; set; }
        public int? CookTime { get; set; }
        public int? Servings { get; set; }
        public bool? Status { get; set; }
    }
}
