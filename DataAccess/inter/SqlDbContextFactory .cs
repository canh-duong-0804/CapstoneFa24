using BusinessObject.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.inter
{
    public class SqlDbContextFactory : IDbContextFactory
    {
        private readonly string _connectionString;

        public SqlDbContextFactory(string connectionString)
        {
            _connectionString = connectionString;
        }

        public HealthTrackingDBContext CreateDbContext()
        {
            var options = new DbContextOptionsBuilder<HealthTrackingDBContext>()
                .UseSqlServer(_connectionString)
                .Options;

            return new HealthTrackingDBContext(options);
        }
    }
}
