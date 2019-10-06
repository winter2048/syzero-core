using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SyZero.EntityFrameworkCore
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<SyDbContext>
    {
        public SyDbContext CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();
            var builder = new DbContextOptionsBuilder<SyDbContext>();
            var connectionString = configuration.GetConnectionString("sqlConnection");
            //  builder.UseSqlServer(connectionString);
            builder.UseMySql(connectionString);
            return new SyDbContext(builder.Options);
        }
    }
}
