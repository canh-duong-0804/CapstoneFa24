using BusinessObject.Models;
using DataAccess;
using Repository.IRepo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repo
{
    public class CommunityCategoryRepository : ICommunityCategoryRepo
    {
        // Create a new CommunityPostCategory
        public Task<CommunityPostCategory> CreateCategoryAsync(CommunityPostCategory category)
            => CommunityCategoryDAO.Instance.CreateCategoryAsync(category);

        // Delete a CommunityPostCategory by ID
        public Task<bool> DeleteCategoryAsync(int categoryId)
            => CommunityCategoryDAO.Instance.DeleteCategoryAsync(categoryId);

        // Get all CommunityPostCategories
        public Task<List<CommunityPostCategory>> GetAllCategoriesAsync(int pageNumber, int pageSize)
			=> CommunityCategoryDAO.Instance.GetAllCategoriesAsync(pageNumber, pageSize);

        // Get a CommunityPostCategory by ID
        public Task<CommunityPostCategory?> GetCategoryByIdAsync(int id)
            => CommunityCategoryDAO.Instance.GetCategoryByIdAsync(id);

        // Update an existing CommunityPostCategory
        public Task<CommunityPostCategory?> UpdateCategoryAsync(CommunityPostCategory category)
            => CommunityCategoryDAO.Instance.UpdateCategoryAsync(category);
    }
}
