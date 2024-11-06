using BusinessObject.Dto.Login;
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
        private static readonly string KeyString = "YourSecretKey12345"; // Đảm bảo đủ độ dài cho AES-256
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
                    return !context.Members.Any(x => x.Email == email);
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
                    return !context.Members.Any(x => x.PhoneNumber == number);
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
                    return !context.Members.Any(x => x.Username == username);
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
                    // Mã hóa mật khẩu đã nhập để so sánh
                    string encryptedPassword = EncryptPassword(password);
                    var user = await context.Members
                        .FirstOrDefaultAsync(x => x.Email == loginRequestDTO.Email && x.EncryptedPassword == encryptedPassword);

                    return user; // Trả về null nếu không tìm thấy hoặc không khớp
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
                    // Mã hóa mật khẩu trước khi lưu vào cơ sở dữ liệu
                    registerationRequestDTO.EncryptedPassword = EncryptPassword(registerationRequestDTO.EncryptedPassword);
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

        private string EncryptPassword(string password)
        {
            using (Aes aes = Aes.Create())
            {
                byte[] key = Encoding.UTF8.GetBytes(KeyString);
                Array.Resize(ref key, 32); // Đảm bảo khóa dài 32 byte (256-bit)

                aes.Key = key;
                aes.GenerateIV(); // Tạo IV ngẫu nhiên

                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    // Ghi IV vào đầu chuỗi mã hóa
                    msEncrypt.Write(aes.IV, 0, aes.IV.Length);

                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, aes.CreateEncryptor(), CryptoStreamMode.Write))
                    using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                    {
                        swEncrypt.Write(password);
                    }

                    return Convert.ToBase64String(msEncrypt.ToArray());
                }
            }
        }

        private string DecryptPassword(string encryptedPassword)
        {
            byte[] fullCipher = Convert.FromBase64String(encryptedPassword);

            using (Aes aes = Aes.Create())
            {
                byte[] key = Encoding.UTF8.GetBytes(KeyString);
                Array.Resize(ref key, 32); // Đảm bảo khóa dài 32 byte

                // Tách IV từ đầu chuỗi mã hóa
                byte[] iv = new byte[aes.BlockSize / 8];
                Array.Copy(fullCipher, 0, iv, 0, iv.Length);

                aes.Key = key;
                aes.IV = iv;

                using (MemoryStream msDecrypt = new MemoryStream(fullCipher, iv.Length, fullCipher.Length - iv.Length))
                using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, aes.CreateDecryptor(), CryptoStreamMode.Read))
                using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                {
                    return srDecrypt.ReadToEnd();
                }
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
