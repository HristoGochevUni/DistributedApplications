using System;
using System.IO;
using Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Data.Context
{
    public class RigsSystemDbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var apiSettings = GetApiSettings().GetSection("ConnectionStrings");
            var databasePath = apiSettings["ApiDbLocation"];
            optionsBuilder.UseSqlite(databasePath);
        }

        private static IConfiguration GetApiSettings()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");

            return builder.Build();
        }

        public DbSet<Cooler> Coolers { get; set; }
        public DbSet<CPU> CPUs { get; set; }
        public DbSet<Drive> Drives { get; set; }
        public DbSet<GPU> GPUs { get; set; }
        public DbSet<Motherboard> Motherboards { get; set; }
        public DbSet<PCCase> PCCases { get; set; }
        public DbSet<PSU> PSUs { get; set; }
        public DbSet<RAM> RAMs { get; set; }
        public DbSet<Rig> RIGs { get; set; }
    }
}