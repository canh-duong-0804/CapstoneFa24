using BusinessObject.Dto;
using BusinessObject.Dto.Register;
using BusinessObject.Models;

using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class StaffDAO
    {
        private static StaffDAO instance = null;
        private static readonly object instanceLock = new object();

        public static StaffDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new StaffDAO();
                    }
                    return instance;
                }
            }
        }

        public async Task<staff?> LoginStaff(staff loginRequestStaffDTO)
        {
            try
            {
                using (var _context = new HealthTrackingDBContext())
                {
                    var staff = await _context.staffs
                        .FirstOrDefaultAsync(st => st.Email == loginRequestStaffDTO.Email && st.Password == loginRequestStaffDTO.Password);

                    return staff; 
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public async Task<staff?> RegisterStaff(staff registerationRequesStafftDTO)
        {
            try
            {
                using (var _context = new HealthTrackingDBContext())
                {

                    _context.staffs.Add(registerationRequesStafftDTO);
                    await _context.SaveChangesAsync();

                    return registerationRequesStafftDTO;

                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);

            }
        }

        public bool IsUniquePhonenumber(string number)
        {
            try
            {
                using (var _context = new HealthTrackingDBContext())
                {

                    using (var context = new HealthTrackingDBContext())
                    {
                        var user = context.staffs.FirstOrDefault(x => x.PhoneNumber == number);
                        if (user == null)
                        {
                            return true;
                        }
                        return false;
                    }

                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);

            }
        }
    }
}
