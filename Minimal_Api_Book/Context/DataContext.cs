﻿using Microsoft.EntityFrameworkCore;
using Minimal_Api_Book.Data;

namespace Minimal_Api_Book.Context
{
    public class DataContext : DbContext
    {

        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer("Data Source = DAMIR ; Initial Catalog = BooksDb; \nIntegrated Security = True;TrustServerCertificate = True;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Book>()
        .HasOne(b => b.Genre)
        .WithMany(g => g.Books)
        .HasForeignKey(b => b.GenreId);
        }

        public DbSet<Book> Books => Set<Book>();
        public DbSet<Genre> Genres => Set<Genre>();
    }
}
