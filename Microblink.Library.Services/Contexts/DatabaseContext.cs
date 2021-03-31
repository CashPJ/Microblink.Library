using System;
using Microblink.Library.Services.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace Microblink.Library.Services.Contexts
{
    public partial class DatabaseContext : DbContext
    {
        private readonly string _connectionString;

        public DatabaseContext(string connectionString)
        {
            this.LazyLoadingEnabled = true;
            _connectionString = connectionString;
        }

        public DatabaseContext(string connectionString, DbContextOptions<DatabaseContext> options) : base(options)
        {
            this.Configuration.LazyLoadingEnabled = true;
            _connectionString = connectionString;
        }

        public virtual DbSet<BookDto> Books { get; set; }
        public virtual DbSet<BookTitleDto> BookTitles { get; set; }
        public virtual DbSet<Contact> Contacts { get; set; }
        public virtual DbSet<ContactTypeDto> ContactTypes { get; set; }
        public virtual DbSet<Rent> Rents { get; set; }
        public virtual DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer(_connectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Croatian_CI_AS");

            modelBuilder.Entity<BookDto>(entity =>
            {
                entity.ToTable("Book");

                entity.HasIndex(e => e.Id, "IX_Book");

                entity.HasOne(d => d.IdBookTitleNavigation)
                    .WithMany(p => p.Books)
                    .HasForeignKey(d => d.IdBookTitle)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Book_BookTitle");
            });

            modelBuilder.Entity<BookTitleDto>(entity =>
            {
                entity.ToTable("BookTitle");

                entity.HasIndex(e => e.Isbn, "IX_BookTitles")
                    .IsUnique();

                entity.Property(e => e.Author)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Isbn)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Contact>(entity =>
            {
                entity.ToTable("Contact");

                entity.Property(e => e.Value)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.HasOne(d => d.ContactType)
                    .WithMany(p => p.Contacts)
                    .HasForeignKey(d => d.ContactTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Contact_ContactType");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Contacts)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Contact_User");
            });

            modelBuilder.Entity<ContactTypeDto>(entity =>
            {
                entity.ToTable("ContactType");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.SortOrder).HasDefaultValueSql("((1000))");
            });

            modelBuilder.Entity<Rent>(entity =>
            {
                entity.ToTable("Rent");

                entity.Property(e => e.RentDate).HasColumnType("date");

                entity.Property(e => e.ReturnDate).HasColumnType("date");

                entity.HasOne(d => d.Book)
                    .WithMany(p => p.Rents)
                    .HasForeignKey(d => d.BookId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Rent_Book");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Rents)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Rent_User");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("User");

                entity.Property(e => e.DateOfBirth).HasColumnType("date");

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
