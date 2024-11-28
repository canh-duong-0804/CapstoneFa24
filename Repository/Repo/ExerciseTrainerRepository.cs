﻿using BusinessObject.Dto.ExerciseTrainer;
using DataAccess;
using Repository.IRepo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repo
{
    public class ExerciseTrainerRepository : IExerciseTrainerRepository
    {
        public Task<int> CreateExerciseTrainerAsync(CreateExerciseRequestDTO request, int memberId) => ExerciseTrainerDAO.Instance.CreateExerciseTrainerAsync(request,memberId);

        public Task<bool> DeleteExerciseAsync(int exerciseId) => ExerciseTrainerDAO.Instance.DeleteExerciseAsync(exerciseId);

        public Task<ExerciseRequestDTO> GetExerciseDetailAsync(int exerciseId) => ExerciseTrainerDAO.Instance.GetExerciseDetailAsync(exerciseId);


        public Task<ExerciseRequestDTO> UpdateExerciseAsync(int exerciseId, ExerciseRequestDTO updateRequest) => ExerciseTrainerDAO.Instance.UpdateExerciseAsync(exerciseId, updateRequest);

    }
}