using BusinessObject.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class CommunityCategoryDAO
    {
        private static CommunityCategoryDAO instance = null;
        private static readonly object instanceLock = new object();

        public static CommunityCategoryDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new CommunityCategoryDAO();
                    }
                    return instance;
                }
            }
        }

        public async Task<CommunityPostCategory> CreateCategoryAsync(CommunityPostCategory category)
        {
            try
            {
                using (var context = new HealthTrackingDBContext())
                {

                    await context.CommunityPostCategories.AddAsync(category);
                    await context.SaveChangesAsync();
                    return category;
                }
            }
            catch (Exception e)
            {
                throw new Exception("Error creating category: " + e.Message);
            }
        }

        // Read all CommunityPostCategories
        public async Task<List<CommunityPostCategory>> GetAllCategoriesAsync()
        {
            try
            {
                using (var context = new HealthTrackingDBContext())
                {
                    return await context.CommunityPostCategories.ToListAsync();
                }
            }
            catch (Exception e)
            {
                throw new Exception("Error retrieving categories: " + e.Message);
            }
        }

        // Read a single CommunityPostCategory by ID
        public async Task<CommunityPostCategory> GetCategoryByIdAsync(int id)
        {
            try
            {
                using (var context = new HealthTrackingDBContext())
                {
                    return await context.CommunityPostCategories.FindAsync(id);
                }
            }
            catch (Exception e)
            {
                throw new Exception("Error retrieving category: " + e.Message);
            }
        }

        // Update an existing CommunityPostCategory
        public async Task<CommunityPostCategory> UpdateCategoryAsync(CommunityPostCategory category)
        {
            try
            {
                using (var context = new HealthTrackingDBContext())
                {
                    var existingCategory = await context.CommunityPostCategories.FindAsync(category.CommunityCategoryId);
                    if (existingCategory == null)
                    {
                        throw new Exception("Category not found.");
                    }

                    existingCategory.CommunityCategoryName = category.CommunityCategoryName;
                    context.CommunityPostCategories.Update(existingCategory);
                    await context.SaveChangesAsync();
                    return existingCategory;
                }
            }
            catch (Exception e)
            {
                throw new Exception("Error updating category: " + e.Message);
            }
        }

        // Delete a CommunityPostCategory by ID
        public async Task<bool> DeleteCategoryAsync(int id)
        {
            try
            {
                using (var context = new HealthTrackingDBContext())
                {
                    var category = await context.CommunityPostCategories.FindAsync(id);
                    if (category == null)
                    {
                        throw new Exception("Category not found.");
                    }

                    context.CommunityPostCategories.Remove(category);
                    await context.SaveChangesAsync();
                    return true;
                }
            }
            catch (Exception e)
            {
                throw new Exception("Error deleting category: " + e.Message);
            }
        }

    }
}

