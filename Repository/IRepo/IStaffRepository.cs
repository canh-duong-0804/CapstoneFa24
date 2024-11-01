using BusinessObject.Dto.Staff;
using BusinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.IRepo
{
    public interface IStaffRepository
    {
        bool IsUniqueEmail(string email);
        bool IsUniquePhonenumber(string number);
        Task<staff> RegisterAccountStaff(staff registerationRequestDTO);
        Task<staff> Login(staff loginRequestDTO);
        Task<IEnumerable<AllStaffsResponseDTO>> GetAllAccountStaffsAsync();
        Task<GetStaffByIdResponseDTO> GetAccountStaffForAdminByIdAsync(int id);
        Task<bool> DeleteAccountStaffByIdAsync(int id);
        Task<UpdateRoleStaffRequestDTO> UpdateRoleAccountStaffByIdAsync(UpdateRoleStaffRequestDTO staffRole);
        Task<UpdateInfoAccountStaffByIdDTO> UpdateAccountStaffById(UpdateInfoAccountStaffByIdDTO staffRole);
        Task<GetStaffPersonalByIdResponseDTO> GetAccountPersonalForStaffByIdAsync(int id);
    }
}
