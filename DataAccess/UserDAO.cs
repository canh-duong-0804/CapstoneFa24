using BusinessObject.Dto.Login;
using BusinessObject.Dto.Register;
using BusinessObject.Dto.ResetPassword;
using BusinessObject.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class UserDAO
    {
        private static UserDAO instance = null;
        private static readonly object instanceLock = new object();

        public static UserDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new UserDAO();
                    }
                    return instance;
                }
            }
        }

        public bool IsUniqueEmail(string email)
        {
            try
            {
                using (var context = new HealthTrackingDBContext())
                {
                    var user = context.Members.FirstOrDefault(x => x.Email == email);
                    if (user == null)
                    {
                        return true;
                    }
                    return false;
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public bool IsUniquePhonenumber(string number)
        {
            try
            {
                using (var context = new HealthTrackingDBContext())
                {
                    var user = context.Members.FirstOrDefault(x => x.PhoneNumber == number);
                    if (user == null)
                    {
                        return true;
                    }
                    return false;
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public bool IsUniqueUser(string username)
        {
            try
            {
                using (var context = new HealthTrackingDBContext())
                {
                    var user = context.Members.FirstOrDefault(x => x.Username == username);
                    if (user == null)
                    {
                        return true;
                    }
                    return false;
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<Member> Login(Member loginRequestDTO, string password)
        {
            try
            {
                using (var context = new HealthTrackingDBContext())
                {

                    var user = await context.Members.FirstOrDefaultAsync(x => x.PhoneNumber == loginRequestDTO.PhoneNumber);

                    if (user == null)
                        return null;


                    if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
                        return null;

                    return user;
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<Member> Register(Member registerationRequestDTO, RegisterationMobileRequestDTO member)
        {
            try
            {
                using (var context = new HealthTrackingDBContext())
                {

                    CreatePasswordHash(member.Password, out byte[] passwordHash, out byte[] passwordSalt);


                    registerationRequestDTO.PasswordHash = passwordHash;
                    registerationRequestDTO.PasswordSalt = passwordSalt;


                    context.Members.Add(registerationRequestDTO);
                    await context.SaveChangesAsync();
                    var savedMember = await context.Members
                    .FirstOrDefaultAsync(m => m.Email == registerationRequestDTO.Email);
                    var bodyMeasureChange = new BodyMeasureChange
                    {
                        MemberId = savedMember.MemberId,
                        Weight = member.Weight,
                        DateChange = DateTime.UtcNow
                    };
                    double currentWeight = member.Weight;
                    double targetWeight = (member.TargetWeight.HasValue ? member.TargetWeight : member.Weight) ?? 0.0;
                    double weeklyGoal = Convert.ToDouble(member.weightPerWeek);
                    //double weeklyGoal = Convert.ToDouble(member.weightPerWeek); 
                    DateTime targetDate = DateTime.UtcNow;
                    if (weeklyGoal != 0)
                    {
                        
                        int weeksNeeded = (int)Math.Ceiling(Math.Abs((targetWeight - currentWeight) / weeklyGoal));
                        targetDate = DateTime.Now.AddDays(weeksNeeded * 7);
                    }

                    var goal = new Goal
                    {
                        MemberId = savedMember.MemberId,
                        TargetValue = (member.TargetWeight.HasValue ? member.TargetWeight : member.Weight) ?? 0.0,

                        ChangeDate = DateTime.UtcNow,   
                       
                        TargetDate = targetDate,

                        ExerciseLevel = member.ExerciseLevel,
                        // dang goal type 1 2 3 
                        GoalType = member.weightPerWeek.ToString(),
                        //  GoalType=member.weightPerWeek.ToString(),
                    };

                    await context.Goals.AddAsync(goal);
                    await context.SaveChangesAsync();

                    context.BodyMeasureChanges.Add(bodyMeasureChange);
                    await context.SaveChangesAsync();

                    return registerationRequestDTO;
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        private bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
        {
            using (var hmac = new HMACSHA512(storedSalt))
            {
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(storedHash);
            }
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }
        public async Task<Member> GetMemberByIdAsync(int userId)
        {
            using (var context = new HealthTrackingDBContext())
            {
                return await context.Members.Include(x => x.BodyMeasureChanges).FirstOrDefaultAsync(x => x.MemberId == userId);
            }
        }

        public async Task UpdateMemberProfileAsync(Member user, double weight)
        {
            try
            {
                using (var context = new HealthTrackingDBContext())
                {
                    
                    var existingMember = await context.Members
                        .FirstOrDefaultAsync(m => m.MemberId == user.MemberId);

                    if (existingMember == null)
                    {
                        throw new Exception("Member not found.");
                    }

                   
                    var latestGoal = await context.Goals
                        .Where(d => d.MemberId == user.MemberId)
                        .OrderByDescending(d => d.GoalId)
                        .FirstOrDefaultAsync();

                    if (latestGoal == null)
                    {
                        throw new Exception("Goal not found.");
                    }

                
                    existingMember.Username = user.Username;
                    existingMember.PhoneNumber = user.PhoneNumber;
                    existingMember.ImageMember = user.ImageMember;
                    existingMember.Dob = user.Dob;

                    
                    var addNewWeightCurrent = new BodyMeasureChange
                    {
                        MemberId = user.MemberId,
                        Weight = weight,
                        DateChange = DateTime.Now,
                        BodyFat = 0,
                        Muscles = 0,
                    };

                    
                    var newGoal = new Goal
                    {
                        MemberId = latestGoal.MemberId,
                        GoalType = latestGoal.GoalType,
                        ExerciseLevel = latestGoal.ExerciseLevel,
                        TargetValue = latestGoal.TargetValue,
                        TargetDate = latestGoal.TargetDate,
                        ChangeDate = DateTime.Now
                    };

                  
                    context.BodyMeasureChanges.Add(addNewWeightCurrent);
                    context.Goals.Add(newGoal);

                  
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception e)
            {
                throw new Exception("An error occurred while updating the member profile.", e);
            }
        }


        public async Task<bool> ResetPasswordAsync(ChangePasswordRequestDTO request, int memberId)
        {

            try
            {
                using (var context = new HealthTrackingDBContext())
                {

                    var user = await context.Members.FirstOrDefaultAsync(u => u.MemberId == memberId);
                    if (user == null)
                    {
                        throw new Exception("User not found.");
                    }
                    /*if (!VerifyPasswordHash(request.OldPassword, user.PasswordHash, user.PasswordSalt))
                        return false;*/

                    CreatePasswordHash(request.NewPassword, out byte[] passwordHash, out byte[] passwordSalt);


                    user.PasswordHash = passwordHash;
                    user.PasswordSalt = passwordSalt;


                    context.Members.Update(user);
                    await context.SaveChangesAsync();

                    return true;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while resetting the password: {ex.Message}");
            }


        }

        public async Task<bool> ResetPasswordOtpAsync(ChangePasswordRequestDTO request)
        {
            try
            {
                using (var context = new HealthTrackingDBContext())
                {

                    var user = await context.Members.FirstOrDefaultAsync(u => u.PhoneNumber == request.PhoneNumber);
                   

                    CreatePasswordHash(request.NewPassword, out byte[] passwordHash, out byte[] passwordSalt);


                    user.PasswordHash = passwordHash;
                    user.PasswordSalt = passwordSalt;


                    context.Members.Update(user);
                    await context.SaveChangesAsync();

                    return true;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while resetting the password: {ex.Message}");
            }
        }

        public async Task<Member> DeleteAccount(Member loginRequestDTO, string password)
        {
            try
            {
                using (var context = new HealthTrackingDBContext())
                {

                    var user = await context.Members.FirstOrDefaultAsync(x => x.PhoneNumber == loginRequestDTO.PhoneNumber);

                    if (user == null)
                        return null;


                    if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
                        return null;

                    return user;
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<bool> UploadImageForMember(string urlImage, int memberId)
        {
            try
            {
                using var context = new HealthTrackingDBContext();


                var mealMember = await context.Members
                    .FirstOrDefaultAsync(m => m.MemberId == memberId);

                if (mealMember == null)
                {
                    throw new Exception("MealMember not found.");
                }


                mealMember.ImageMember = urlImage;


                await context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error uploading image for meal member: {ex.Message}", ex);
            }
        }
    }
}
