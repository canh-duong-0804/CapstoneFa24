using BusinessObject.Dto.MealPlanDetailMember;
using BusinessObject.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class MealPlanMemberDetailsDAO
    {
        private static MealPlanMemberDetailsDAO instance = null;
        private static readonly object instanceLock = new object();

        public static MealPlanMemberDetailsDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new MealPlanMemberDetailsDAO();
                    }
                    return instance;
                }
            }
        }

        public async Task CreateMealPlanDetailsOfMemberAsync(MealsMemberDetail mealMemberModel, int memberid, DateTime date)
        {
           /* try
            {
                using (var context = new HealthTrackingDBContext())
                {


                    var mealPlanMember = await context.MealPlanMembers
                  .FirstOrDefaultAsync(fd => fd.MemberId == memberid &&
                                             fd.MealDate.HasValue &&
                                             fd.MealDate.Value.Date == date.Date);

                    if (mealPlanMember == null)
                    {

                        var mealPlan = new MealPlanMember
                        {
                            MemberId = memberid,
                            Image = "",
                            NameMealPlanMember = "",
                            TotalCalories = 0,
                            TotalProtein = 0,
                            TotalCarb = 0,
                            TotalFat = 0,
                            MealDate = DateTime.Now


                        };
                        context.MealPlanMembers.Add(mealPlan);
                        context.SaveChanges();
                    }
                    mealPlanMember = await context.MealPlanMembers
                  .FirstOrDefaultAsync(fd => fd.MemberId == memberid &&
                                             fd.MealDate.HasValue &&
                                             fd.MealDate.Value.Date == date.Date);
                    mealMemberModel.MealPlanId = mealPlanMember.MealPlanId;
                    mealPlanMember.MemberId = memberid;

                    context.MealsPlanMemberDetails.Add(mealMemberModel);
                    context.SaveChanges();
                    var getMealPlanMember = await context.MealPlanMembers.FirstOrDefaultAsync(mp => mp.MemberId == memberid && mp.MealDate.HasValue &&
                                             mp.MealDate.Value.Date == date.Date);
                    getMealPlanMember.TotalCalories += mealMemberModel.Calories;
                    getMealPlanMember.TotalFat += mealMemberModel.Fat;
                    getMealPlanMember.TotalCarb += mealMemberModel.Carb;
                    getMealPlanMember.TotalProtein += mealMemberModel.Protein;
                    await context.SaveChangesAsync();




                }
            }
            catch (Exception ex)
            {


            }*/
        }
    }
}
