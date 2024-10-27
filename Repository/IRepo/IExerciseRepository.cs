﻿using BusinessObject.Dto.Exericse;
using BusinessObject.Dto.SearchFilter;
using BusinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.IRepo
{
    public interface IExerciseRepository
    {

        Task<IEnumerable<AllExerciseResponseDTO>> GetAllExercisesAsync(); 
        Task<ExerciseDetailDTO> GetExerciseByIdAsync(int id);
        Task<IEnumerable<AllExerciseResponseDTO>> SearchAndFilterExerciseByIdAsync(SearchFilterObjectDTO searchName); 
        Task<Exercise> CreateExerciseAsync(Exercise exercise); 
        Task<ExerciseCategory> CreateExerciseCategoryAsync(ExerciseCategory exercise); 
        Task<UpdateExerciseRequestDTO> UpdateExerciseAsync(UpdateExerciseRequestDTO exercise); 
        Task<bool> DeleteExerciseAsync(int id); 


    }
}