using Microblink.Library.Services.Models;
using Microblink.Library.Services.Models.Dto;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System;

#nullable disable

namespace Microblink.Library.Services.Contexts
{
    public partial class DatabaseContext : DbContext
    {
        private readonly string _connectionString;

        public DatabaseContext(string connectionString) : base()
        {
            _connectionString = connectionString;
            Database.EnsureCreated();
        }

        public DatabaseContext(string connectionString, DbContextOptions<DatabaseContext> options) : base(options)
        {
            _connectionString = connectionString;
            Database.EnsureCreated();
        }

        public DbSet<Book> Books { get; set; }
        public DbSet<BookTitle> BookTitles { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<ContactType> ContactTypes { get; set; }
        public DbSet<Rent> Rents { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(_connectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Croatian_CI_AS");

            modelBuilder.Entity<Book>(entity =>
            {
                entity.HasIndex(e => e.BookTitleId, "IX_Books_BookTitleId");

                entity.HasOne(d => d.BookTitle)
                    .WithMany(p => p.Books)
                    .HasForeignKey(d => d.BookTitleId);
            });

            modelBuilder.Entity<Contact>(entity =>
            {
                entity.HasIndex(e => e.ContactTypeId, "IX_Contacts_ContactTypeId");

                entity.HasIndex(e => e.UserId, "IX_Contacts_UserId");

                entity.HasOne(d => d.ContactType)
                    .WithMany(p => p.Contacts)
                    .HasForeignKey(d => d.ContactTypeId);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Contacts)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<ContactType>(entity =>
            {
                entity.ToTable("ContactType");
            });

            modelBuilder.Entity<Rent>(entity =>
            {
                entity.HasIndex(e => e.BookId, "IX_Rents_BookId");

                entity.HasIndex(e => e.BookTitleId, "IX_Rents_BookTitleId");

                entity.HasIndex(e => e.UserId, "IX_Rents_UserId");

                entity.HasOne(d => d.Book)
                    .WithMany(p => p.Rents)
                    .HasForeignKey(d => d.BookId);

                entity.HasOne(d => d.BookTitle)
                    .WithMany(p => p.Rents)
                    .HasForeignKey(d => d.BookTitleId);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Rents)
                    .HasForeignKey(d => d.UserId);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

    }
}
