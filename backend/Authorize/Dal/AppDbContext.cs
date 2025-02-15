﻿using Authorize.Dal.Configurations;
using Authorize.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Authorize.Dal
{
    public class AppDbContext : DbContext
    {
        private readonly IConfiguration configuration;

        public AppDbContext(
            DbContextOptions<AppDbContext> options,
            IConfiguration configuration) :
            base(options)
        {
            this.configuration = configuration;
        }

        public DbSet<User> Users { get; set; }

        public DbSet<Roles> Roles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new RoleConfigurations());
            modelBuilder.ApplyConfiguration(new UserConfigurations());
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("SQLServerConnection"));
        }
    }
}
