using BusinessObject.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HTUnitTests.Db
{
    public class DatabaseFixture : IDisposable
    {
        public HealthTrackingDBContext Context { get; private set; }

        public DatabaseFixture()
        {
            // Khởi tạo DbContext với chuỗi kết nối tới cơ sở dữ liệu thực
            var options = new DbContextOptionsBuilder<HealthTrackingDBContext>()
                .UseSqlServer("server=DESKTOP-5DR1P1T; database =HealthTrackingDB;uid=sa;pwd=123;TrustServerCertificate=true") // Thay bằng chuỗi kết nối của bạn
                .Options;

            Context = new HealthTrackingDBContext(options);
        }

        // Clear dữ liệu sau khi test
        public void Dispose()
        {
            // Xóa dữ liệu test trong bảng Members
            Context.Members.RemoveRange(Context.Members.Where(m => m.PhoneNumber.StartsWith("Test_")));
            Context.SaveChanges();
            Context.Dispose();
        }
    }

}
