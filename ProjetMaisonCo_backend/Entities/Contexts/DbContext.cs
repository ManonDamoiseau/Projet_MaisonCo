using System;
using Microsoft.EntityFrameworkCore;
using Projet_MaisonCo_backend.Entities;

namespace ProjetMaisonCo_backend.Entities.Contexts
{
	public class MyDbContext : DbContext
	{
		public DbSet<User> Users => Set<User>();

		public MyDbContext(DbContextOptions<MyDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(MyDbContext).Assembly);
        }
    }
}
