using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Wuther.Entities.Models
{
    public partial class wutherContext : DbContext
    {
        public wutherContext()
        {
        }

        public wutherContext(DbContextOptions<wutherContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Blogs> Blogs { get; set; }
        public virtual DbSet<Menus> Menus { get; set; }
        public virtual DbSet<Users> Users { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Blogs>(entity =>
            {
                entity.ToTable("blogs");

                entity.HasIndex(e => e.MenuId)
                    .HasName("blog_menu");

                entity.HasIndex(e => e.UserId)
                    .HasName("blog_user");

                entity.Property(e => e.Id).HasColumnType("int(11)");

                entity.Property(e => e.Abstract)
                    .HasColumnType("varchar(300)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Comment).HasColumnType("int(11)");

                entity.Property(e => e.CreateTime).HasColumnType("datetime");

                entity.Property(e => e.IsPublic)
                    .HasColumnType("int(1)")
                    .HasComment("是否公开：0是，1否");

                entity.Property(e => e.Like).HasColumnType("int(11)");

                entity.Property(e => e.MenuId).HasColumnType("int(11)");

                entity.Property(e => e.ModifyTime).HasColumnType("datetime");

                entity.Property(e => e.Path)
                    .HasColumnType("varchar(100)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasColumnType("varchar(100)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Trend).HasColumnType("int(11)");

                entity.Property(e => e.UserId).HasColumnType("int(11)");

                entity.HasOne(d => d.Menu)
                    .WithMany(p => p.Blogs)
                    .HasForeignKey(d => d.MenuId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("blog_menu");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Blogs)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("blog_user");
            });

            modelBuilder.Entity<Menus>(entity =>
            {
                entity.ToTable("menus");

                entity.Property(e => e.Id).HasColumnType("int(11)");

                entity.Property(e => e.Icon)
                    .HasColumnType("varchar(20)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Name)
                    .HasColumnType("varchar(20)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.ParentId).HasColumnType("int(11)");

                entity.Property(e => e.Path)
                    .HasColumnType("varchar(100)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Position)
                    .HasColumnType("int(1)")
                    .HasDefaultValueSql("'0'")
                    .HasComment("0:横栏，1：竖栏");

                entity.Property(e => e.Seqno).HasColumnType("int(11)");
            });

            modelBuilder.Entity<Users>(entity =>
            {
                entity.ToTable("users");

                entity.Property(e => e.Id).HasColumnType("int(11)");

                entity.Property(e => e.Account)
                    .IsRequired()
                    .HasColumnType("varchar(15)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Department)
                    .HasColumnType("varchar(20)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Email)
                    .HasColumnType("varchar(50)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Password)
                    .HasColumnType("varchar(20)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Phone)
                    .HasColumnType("varchar(15)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Sex).HasColumnType("int(1)");

                entity.Property(e => e.Username)
                    .HasColumnType("varchar(20)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.WrittenOffTime).HasColumnType("datetime");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
