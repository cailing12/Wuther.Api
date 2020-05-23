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

        public virtual DbSet<Menus> Menus { get; set; }
        public virtual DbSet<Users> Users { get; set; }

//         protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//         {
//             if (!optionsBuilder.IsConfigured)
//             {
// #warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
//                 optionsBuilder.UseMySql("server=127.0.0.1;database=wuther;user id=root;password=wuther123", x => x.ServerVersion("5.6.41-mysql"));
//             }
//         }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Menus>(entity =>
            {
                entity.ToTable("menus");

                entity.HasComment("功能模块表");

                entity.Property(e => e.Id)
                    .HasColumnType("int(11)")
                    .HasComment("功能模块流水号");

                entity.Property(e => e.CascadeId)
                    .IsRequired()
                    .HasColumnType("varchar(255)")
                    .HasComment("节点语义ID")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.IconName)
                    .IsRequired()
                    .HasColumnType("varchar(255)")
                    .HasComment("节点图标文件名称")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.IsLeaf)
                    .HasColumnType("tinyint(4)")
                    .HasComment("是否叶子节点");

                entity.Property(e => e.IsSys)
                    .HasColumnType("tinyint(4)")
                    .HasComment("是否为系统模块");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnType("varchar(255)")
                    .HasComment("功能模块名称")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.ParentId)
                    .HasColumnType("int(11)")
                    .HasComment("父节点流水号");

                entity.Property(e => e.ParentName)
                    .IsRequired()
                    .HasColumnType("varchar(255)")
                    .HasComment("父节点名称")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.SortNo)
                    .HasColumnType("int(11)")
                    .HasComment("排序号");

                entity.Property(e => e.Status)
                    .HasColumnType("int(11)")
                    .HasComment("当前状态");

                entity.Property(e => e.Url)
                    .IsRequired()
                    .HasColumnType("varchar(255)")
                    .HasComment("主页面URL")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");
            });

            modelBuilder.Entity<Users>(entity =>
            {
                entity.ToTable("users");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Account)
                    .IsRequired()
                    .HasColumnName("account")
                    .HasColumnType("varchar(50)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Department)
                    .HasColumnName("department")
                    .HasColumnType("varchar(100)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Email)
                    .HasColumnName("email")
                    .HasColumnType("varchar(100)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Password)
                    .HasColumnName("password")
                    .HasColumnType("varchar(50)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Phone)
                    .HasColumnName("phone")
                    .HasColumnType("varchar(20)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Sex)
                    .HasColumnName("sex")
                    .HasColumnType("int(1)");

                entity.Property(e => e.Username)
                    .HasColumnName("username")
                    .HasColumnType("varchar(50)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
