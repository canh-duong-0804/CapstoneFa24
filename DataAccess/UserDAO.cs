using BusinessObject.Dto.Register;
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

        public async Task<Member> Register(Member registerationRequestDTO, string password, double weight)
        {
            try
            {
                using (var context = new HealthTrackingDBContext())
                {
                    
                    CreatePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);

                   
                    registerationRequestDTO.PasswordHash = passwordHash;
                    registerationRequestDTO.PasswordSalt = passwordSalt;

                   
                    context.Members.Add(registerationRequestDTO);
                    await context.SaveChangesAsync();
                    var savedMember = await context.Members
                .FirstOrDefaultAsync(m => m.Email == registerationRequestDTO.Email);
                    var bodyMeasureChange = new BodyMeasureChange
                    {
                        MemberId = savedMember.MemberId,  
                        Weight = weight,
                        DateChange = DateTime.UtcNow  
                    };

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
                return await context.Members.FirstOrDefaultAsync(x => x.MemberId == userId);
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
