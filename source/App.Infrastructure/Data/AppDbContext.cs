using App.Core.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext()
        {
        }
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }


        public DbSet<User> Users => Set<User>();
        public DbSet<Role> Roles => Set<Role>();
        public DbSet<Permission> Permissions => Set<Permission>();
        public DbSet<UserRole> UserRoles => Set<UserRole>();
        public DbSet<RolePermission> RolePermissions => Set<RolePermission>();

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=DESKTOP-TN280R6\\SQLEXPRESS\\SQLEXPRESS;Database=AppDb;Trusted_Connection=True;TrustServerCertificate=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.UserName).HasMaxLength(150);
                entity.HasIndex(e => e.UserName, "UK_UserName")
                    .IsUnique();
                entity.Property(e => e.FirstName).HasMaxLength(150);
                entity.Property(e => e.LastName).HasMaxLength(150);
                entity.Property(e => e.Email).HasMaxLength(150);
                entity.HasIndex(e => e.Email, "UK_Email")
                  .IsUnique();
                entity.Property(e => e.PasswordHash).HasMaxLength(100);

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.Property(e => e.Name).HasMaxLength(150);
                entity.Property(e => e.Description).HasMaxLength(250);
                entity.Property(e => e.CreatedAt)
                   .HasColumnType("datetime")
                   .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.HasOne(x => x.CreatedBy)
              .WithMany(x => x.RoleCreatedByUsers)
              .HasForeignKey(r => r.CreatedById)
              .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(x => x.UpdatedBy)
                 .WithMany(x => x.RoleUpdatedByUsers)
                 .HasForeignKey(r => r.UpdatedById)
                 .OnDelete(DeleteBehavior.Restrict);

            });

            modelBuilder.Entity<Permission>(entity =>
            {
                entity.Property(x => x.Key).HasMaxLength(200);
                entity.Property(x => x.Description).HasMaxLength(200);

                entity.HasOne(x => x.CreatedBy)
               .WithMany(x => x.PermissionCreatedByUsers)
               .HasForeignKey(r => r.CreatedById)
                .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(x => x.UpdatedBy)
                 .WithMany(x => x.PermissionUpdatedByUsers)
                 .HasForeignKey(r => r.UpdatedById)
                  .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<UserRole>(entity =>
            {
                entity.HasOne(ur => ur.User)
              .WithMany(u => u.UserRoles)
              .HasForeignKey(ur => ur.UserId);

                entity.HasOne(ur => ur.Role)
             .WithMany(u => u.UserRoles)
             .HasForeignKey(ur => ur.RoleId);

                entity.HasOne(x => x.CreatedBy)
              .WithMany(x => x.UserRoleCreatedByUsers)
              .HasForeignKey(r => r.CreatedById)
               .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(x => x.UpdatedBy)
                 .WithMany(x => x.UserRoleUpdatedByUsers)
                 .HasForeignKey(r => r.UpdatedById)
                  .OnDelete(DeleteBehavior.Restrict);
            });
            modelBuilder.Entity<RolePermission>(entity =>
            {
                entity.HasOne(rp => rp.Role)
           .WithMany(r => r.RolePermissions)
           .HasForeignKey(rp => rp.RoleId);

                entity.HasOne(rp => rp.Permission)
                    .WithMany(p => p.RolePermissions)
                    .HasForeignKey(rp => rp.PermissionId);

                entity.HasOne(x => x.CreatedBy)
             .WithMany(x => x.RolePermissionCreatedByUsers)
             .HasForeignKey(r => r.CreatedById)
              .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(x => x.UpdatedBy)
                 .WithMany(x => x.RolePermissionUpdatedByUsers)
                 .HasForeignKey(r => r.UpdatedById)
                  .OnDelete(DeleteBehavior.Restrict);

            });

        }

    }
}
