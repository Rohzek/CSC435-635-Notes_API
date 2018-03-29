using API.Classes.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace API.Scaffolding
{
    /**
     * Auto generated from a Database First approach using the following command in the Nuget PMC:
     * Scaffold-DbContext {connectionString} Pomelo.EntityFrameworkCore.MySql -OutputDir Scaffolding -f
     */
    public partial class DatabaseContext : DbContext
    {
        public virtual DbSet<Category> Category { get; set; }
        public virtual DbSet<Notes> Notes { get; set; }
        public virtual DbSet<User> User { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseMySql(Settings.Connection);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>(entity =>
            {
                entity.ToTable("category");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(15)");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(45);
            });

            modelBuilder.Entity<Notes>(entity =>
            {
                entity.ToTable("notes");

                entity.HasIndex(e => e.Categoryid)
                    .HasName("category_fk_idx");

                entity.HasIndex(e => e.Userid)
                    .HasName("user_fk_idx");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(15)");

                entity.Property(e => e.Categoryid)
                    .HasColumnName("categoryid")
                    .HasColumnType("int(10)");

                entity.Property(e => e.CreatedOn)
                    .HasColumnName("created_on")
                    .HasColumnType("datetime");

                entity.Property(e => e.IsDeleted)
                    .HasColumnName("is_deleted")
                    .HasColumnType("tinyint(1)");

                entity.Property(e => e.Note)
                    .IsRequired()
                    .HasColumnName("note")
                    .HasColumnType("text");

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasColumnName("title")
                    .HasMaxLength(45);

                entity.Property(e => e.Userid)
                    .HasColumnName("userid")
                    .HasColumnType("int(10)");

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.Notes)
                    .HasForeignKey(d => d.Categoryid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("category_fk");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Notes)
                    .HasForeignKey(d => d.Userid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("user_fk");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("user");

                entity.HasIndex(e => e.Email)
                    .HasName("email_UNIQUE")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(10)");

                entity.Property(e => e.CreatedOn)
                    .HasColumnName("created_on")
                    .HasColumnType("datetime");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasColumnName("email")
                    .HasMaxLength(45);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(45);
            });
        }
    }
}
