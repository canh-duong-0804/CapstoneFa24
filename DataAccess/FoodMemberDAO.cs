using BusinessObject.Dto.Diet;
using BusinessObject.Dto.FoodMember;
using BusinessObject.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class FoodMemberDAO
    {
        private static FoodMemberDAO instance = null;
        private static readonly object instanceLock = new object();

        public static FoodMemberDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new FoodMemberDAO();
                    }
                    return instance;
                }
            }
        }

        public async Task<IEnumerable<GetAllFoodMemberResponseDTO>> GetAllFoodMemberByIdAsync(int memberId)
        {
            try
            {
                using (var context = new HealthTrackingDBContext())
                {
                    return await context.FoodMembers
                    .Where(fm => fm.CreatedBy == memberId)
                    .Select(fm => new GetAllFoodMemberResponseDTO
                    {
                        FoodId = fm.FoodId,
                        FoodName = fm.FoodName,
                        FoodImage = fm.FoodImage,
                        Calories = fm.Calories,
                        Protein = fm.Protein,
                        Carbs = fm.Carbs,
                        Fat = fm.Fat,

                    })
                    .ToListAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving food members: {ex.Message}", ex);
            }
        }

        public async Task<GetFoodMemberDetailResponseDTO> GetFoodMemberDetailByIdAsync(int foodMemberId, int memberId)
        {
            try
            {
                using (var context = new HealthTrackingDBContext())
                {
                    var foodMember = await context.FoodMembers
                    .Where(fm => fm.FoodId == foodMemberId && fm.CreatedBy == memberId)
                    .Select(fm => new GetFoodMemberDetailResponseDTO
                    {
                        FoodId = fm.FoodId,
                        FoodName = fm.FoodName,
                        FoodImage = fm.FoodImage,
                        Calories = fm.Calories,
                        Protein = fm.Protein,
                        Carbs = fm.Carbs,
                        Fat = fm.Fat,
                        VitaminA = fm.VitaminA,
                        VitaminC = fm.VitaminC,
                        VitaminD = fm.VitaminD,
                        VitaminE = fm.VitaminE,
                        VitaminB1 = fm.VitaminB1,
                        VitaminB2 = fm.VitaminB2,
                        VitaminB3 = fm.VitaminB3,



                    })
                    .FirstOrDefaultAsync();

                    return foodMember;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving food members: {ex.Message}", ex);
            }
        }

        public async Task<bool> CreateFoodMemberAsync(FoodMember foodMemberModel)
        {
            try
            {
                using (var context = new HealthTrackingDBContext())
                {
                    await context.FoodMembers.AddAsync(foodMemberModel);


                    await context.SaveChangesAsync();
                    return true;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error creating food member: {ex.Message}", ex);

            }

        }

        public async Task<bool> UpdateFoodMemberAsync(UpdateFoodMemberRequestDTO updateFoodMemberDto, int memberId)
        {
            try
            {
                using (var context = new HealthTrackingDBContext())
                {
                    var existingFoodMember = await context.FoodMembers
                    .FirstOrDefaultAsync(fm => fm.FoodId == updateFoodMemberDto.FoodId && fm.CreatedBy == memberId);
                    if (existingFoodMember == null)
                    {
                        return false;
                    }


                    existingFoodMember.FoodName = updateFoodMemberDto.FoodName;
                    existingFoodMember.FoodImage = updateFoodMemberDto.FoodImage;

                    existingFoodMember.Calories = updateFoodMemberDto.Calories;
                    existingFoodMember.Protein = updateFoodMemberDto.Protein;
                    existingFoodMember.Carbs = updateFoodMemberDto.Carbs;
                    existingFoodMember.Fat = updateFoodMemberDto.Fat;
                    existingFoodMember.Portion = updateFoodMemberDto.Portion;
                    existingFoodMember.VitaminA = updateFoodMemberDto?.VitaminA;
                    existingFoodMember.VitaminC = updateFoodMemberDto?.VitaminC;
                    existingFoodMember.VitaminD = updateFoodMemberDto?.VitaminD;
                    existingFoodMember.VitaminE = updateFoodMemberDto?.VitaminE;
                    existingFoodMember.VitaminB1 = updateFoodMemberDto?.VitaminB1;
                    existingFoodMember.VitaminB2 = updateFoodMemberDto?.VitaminB2;
                    existingFoodMember.VitaminB3 = updateFoodMemberDto?.VitaminB3;





                    await context.SaveChangesAsync();

                    return true;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error updating food member: {ex.Message}", ex);
            }
        }

        public async Task<bool> DeleteFoodMemberAsync(int foodMemberId, int memberId)
        {
            try
            {
                using (var context = new HealthTrackingDBContext())
                {

                    var existingFoodMember = await context.FoodMembers
                 .FirstOrDefaultAsync(fm => fm.FoodId == foodMemberId && fm.CreatedBy == memberId);

                    if (existingFoodMember == null)
                    {
                        return false;
                    }


                    context.FoodMembers.Remove(existingFoodMember);


                    await context.SaveChangesAsync();

                    return true;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error deleting food member: {ex.Message}", ex);
            }
        }
    }
}