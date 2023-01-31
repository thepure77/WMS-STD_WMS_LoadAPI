using GIDataAccess.Models;
using LoadDataAccess.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PlanGIDataAccess.Models;
using System.Collections.Generic;
using System.IO;

namespace DataAccess
{
    public class MasterDbContext : DbContext
    {
        public virtual DbSet<ms_RollCage> ms_RollCage { get; set; }
        public virtual DbSet<Ms_Dock> Ms_Dock { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var builder = new ConfigurationBuilder();
                builder.AddJsonFile(Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json"), optional: false);

                var configuration = builder.Build();

                var connectionString = configuration.GetConnectionString("MasterConnection").ToString();

                optionsBuilder.UseSqlServer(connectionString);
            }
        }
    }
    
}
