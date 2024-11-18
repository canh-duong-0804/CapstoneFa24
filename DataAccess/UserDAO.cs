﻿using BusinessObject.Dto.Register;
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
                  
                    var user = await context.Members.FirstOrDefaultAsync(x => x.Email == loginRequestDTO.Email);

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

                    var goal = new Goal
                    {
                        MemberId = savedMember.MemberId,
                        TargetValue = (member.TargetWeight.HasValue ? member.TargetWeight : member.Weight) ?? 0.0,


                        TargetDate = DateTime.Now.AddMonths(1),
                        ExerciseLevel = member.ExerciseLevel,
                        // dang goal type 1 2 3 
                        GoalType=member.Goal,
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
                return await context.Members.Include(x=>x.BodyMeasureChanges).FirstOrDefaultAsync(x => x.MemberId == userId);
            }
        }

        public async Task UpdateMemberProfileAsync(Member user)
        {
            using (var context = new HealthTrackingDBContext())
            {
                context.Members.Update(user);
                await context.SaveChangesAsync();
            }
        }
    }
}
