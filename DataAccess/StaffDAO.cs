using AutoMapper.Execution;
using BusinessObject.Dto;
using BusinessObject.Dto.Register;
using BusinessObject.Dto.Staff;
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

        public bool IsUniqueEmail(string email)
        {
            try
            {
                using (var _context = new HealthTrackingDBContext())
                {

                    using (var context = new HealthTrackingDBContext())
                    {
                        var user = context.staffs.FirstOrDefault(x => x.Email == email);
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

        public async Task<IEnumerable<AllStaffsResponseDTO>> GetAllAccountStaffsAsync()
        {
            try
            {
                using (var context = new HealthTrackingDBContext())
                {
                    var staffAccounts = await context.staffs
                        .Where(s => s.Status == true)
                        .Select(s => new AllStaffsResponseDTO
                        { StaffId=s.StaffId,
                            FullName = s.FullName,
                            PhoneNumber = s.PhoneNumber,

                            Description = s.Description,

                            StaffImage = s.StaffImage,
                            Email = s.Email,
                            Role = s.Role,
                            StartWorkingDate = s.StartWorkingDate,
                            EndWorkingDate = s.EndWorkingDate,
                            Status = s.Status
                        })
                        .ToListAsync();

                    return staffAccounts;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving staff accounts: {ex.Message}", ex);
            }
        }

        public async Task<GetStaffByIdResponseDTO> GetAccountStaffByIdAsync(int id)
        {
            try
            {
                using (var context = new HealthTrackingDBContext())
                {
                    var staff = await context.staffs
                        .Where(s => s.StaffId == id && s.Status == true)
                        .Select(s => new GetStaffByIdResponseDTO
                        {
                            StaffId = s.StaffId,
                            FullName = s.FullName,
                            PhoneNumber = s.PhoneNumber,
                            Sex = s.Sex,
                            Description = s.Description,
                            Dob = s.Dob,
                            StaffImage = s.StaffImage,
                            Email = s.Email,
                            Password = s.Password,
                            Role = s.Role,
                            StartWorkingDate = s.StartWorkingDate,
                            EndWorkingDate = s.EndWorkingDate,
                            Status = s.Status
                        })
                        .FirstOrDefaultAsync();

                   

                    return staff;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving staff account by ID: {ex.Message}", ex);
            }
        }



        public async Task<UpdateRoleStaffRequestDTO> UpdateRoleAccountStaffByIdAsync(UpdateRoleStaffRequestDTO staffRole)
        {
            try
            {
                using (var context = new HealthTrackingDBContext())
                {

                    var staff = await context.staffs
                                .Where(s => s.StaffId == staffRole.StaffId && s.Status == true)
                                 .FirstOrDefaultAsync();


                    if (staff == null)
                    {
                        throw new Exception("Staff not found or inactive.");
                    }


                    staff.Role = staffRole.Role;


                    await context.SaveChangesAsync();

                    return staffRole;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error updating staff role: {ex.Message}", ex);
            }
        }

        public async Task<bool> DeleteAccountStaffByIdAsync(int id)
        {
            try
            {
                using (var context = new HealthTrackingDBContext())
                {

                    var staff = await context.staffs.FindAsync(id);

                    if (staff == null)
                    {
                        throw new Exception("Food not found.");
                    }


                    staff.Status = false;

                    await context.SaveChangesAsync();

                    return true;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error updating food status: {ex.Message}", ex);
            }
        }
    }
}
