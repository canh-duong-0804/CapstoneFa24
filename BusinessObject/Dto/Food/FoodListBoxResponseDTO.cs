﻿using BusinessObject.Dto.CategoryExerice;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Dto.Food
{
    public class FoodListBoxResponseDTO : ListBoxResponseDTO
    {
        public double Calories { get; set; }
        public string Portion { get; set; } = null!;
        public string Serving { get; set; } = null!;
    }
}
