﻿using BusinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.IRepo
{
    public interface ICommunityCategoryRepo
    {
        Task<CommunityPostCategory> CreateCategoryAsync(CommunityPostCategory category);
        Task<List<CommunityPostCategory>> GetAllCategoriesAsync();
        Task<CommunityPostCategory> GetCategoryByIdAsync(int id);
        Task<CommunityPostCategory> UpdateCategoryAsync(CommunityPostCategory category);
        Task<bool> DeleteCategoryAsync(int id);
    }
}