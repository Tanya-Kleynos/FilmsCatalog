using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using FilmsCatalog.Models;

namespace FilmsCatalog.Data
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Film> Films { get; set; }

        public DbSet<Catalog> Catalogs { get; set; }

        public DbSet<CatalogFilm> CatalogFilms { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<CatalogFilm>()
                .HasKey(x => new { x.CatalogId, x.FilmId });

            modelBuilder.Entity<Film>().HasOne(x => x.Creator).WithMany().OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<Catalog>().HasOne(x => x.Creator).WithMany().OnDelete(DeleteBehavior.NoAction);
        }
    }
}
