using BusinessObject.Models;
using DataAccess.inter;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HTUnitTests.In
{
    public class InMemoryDbContextFactory : IDbContextFactory
    {
        public HealthTrackingDBContext CreateDbContext()
        {
            var options = new DbContextOptionsBuilder<HealthTrackingDBContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase_" + Guid.NewGuid())
                .Options;

            return new HealthTrackingDBContext(options);
        }
    }
}