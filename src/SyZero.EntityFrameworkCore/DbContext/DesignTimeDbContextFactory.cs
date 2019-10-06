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
        private IConfigurationSection _configurationSection { get; set; }
        public DesignTimeDbContextFactory()
        {
        }
        public SyDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<SyDbContext>();
            var connectionString = _configurationSection.GetConnectionString("sqlConnection");
            if (_configurationSection.GetConnectionString("type").ToLower() == "mysql")
                builder.UseMySql(_configurationSection.GetConnectionString("sqlConnection"));
            else
                builder.UseSqlServer(_configurationSection.GetConnectionString("sqlConnection"));
            return new SyDbContext(builder.Options);
        }
    }
}
