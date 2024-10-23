using BusinessObject.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public async Task<Member> Login(Member loginRequestDTO)
        {
            try
            {
                using (var context = new HealthTrackingDBContext())
                {
                    var user = await context.Members.FirstOrDefaultAsync(x =>
                  x.Email == loginRequestDTO.Email && x.Password == loginRequestDTO.Password);
                    
                    return user;
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            
        }

        public async Task<Member> Register(Member registerationRequestDTO)
        {
            try
            {
                using (var context = new HealthTrackingDBContext())
                {
                    context.Members.Add(registerationRequestDTO);
                    await context.SaveChangesAsync();

                    return registerationRequestDTO;
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
