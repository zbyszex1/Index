using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace TeczkaCore.Models.Interfaces;

public class TeczkaCoreContext : DbContext
{
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
  public TeczkaCoreContext(DbContextOptions<TeczkaCoreContext> options) : base(options)
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
  {
  }
  public DbSet<Role> Roles { get; set; }
  public DbSet<User> Users { get; set; }
  public DbSet<Article> Articles { get; set; }
  public DbSet<Section> Sections { get; set; }
  public DbSet<Class> Classes { get; set; }
  public DbSet<Person> Persons { get; set; }
  public DbSet<Scan> Scans { get; set; }
  public DbSet<Indeks> Indexes { get; set; }
  public DbSet<UserRole> UsersRoles { get; set; }
  public DbSet<PersonClass> PersonsClasses { get; set; }
  public DbSet<PersonGroup> PersonsGroups { get; set; }
  public DbSet<RefreshToken> RefreshTokens { get; set; }

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    base.OnModelCreating(modelBuilder);


    modelBuilder.Entity<Role>(eb =>
    {
      eb.HasIndex(r => r.Name);
      eb.Property(r => r.Name)
      .IsRequired()
      .HasMaxLength(64)
      .HasDefaultValue(String.Empty);
    });

    modelBuilder.Entity<User>(eb =>
    {
      eb.HasIndex(u => u.Name);
      eb.Property(u => u.Name)
      .IsRequired()
      .HasMaxLength(64)
      .HasDefaultValue(String.Empty);
      eb.HasIndex(u => u.Email)
      .IsUnique(true);
      eb.Property(u => u.Email)
      .IsRequired()
      .HasMaxLength(64)
      .HasDefaultValue("name@domain.country");
      eb.HasIndex(u => u.Phone);
      eb.Property(u => u.Phone)
      .IsRequired()
      .HasMaxLength(64)
      .HasDefaultValue("123456789");
      eb.Property(u => u.TempPassword)
      .HasMaxLength(32)
      .HasDefaultValue("");
      eb.Property(u => u.PasswordHash)
      .HasMaxLength(1024)
      .HasDefaultValue("");
      eb.HasIndex(u => u.DateOfBirth);
      eb.Property(u => u.DateOfBirth)
      .HasDefaultValue(new DateTime(1900, 1, 1));
      eb.HasOne<Role>(u => u.Role)
        .WithMany(r => r.Users)
        .HasForeignKey(u => u.RoleId)
        .HasForeignKey("RoleId")
        .HasConstraintName("FK_User_RoleId")
        .OnDelete(DeleteBehavior.NoAction);
    });

    modelBuilder.Entity<RefreshToken>(eb =>
    {
      eb.HasIndex(c => c.Token);
      eb.Property(c => c.Token)
      .IsRequired()
      .HasMaxLength(512)
      .HasDefaultValue("");
      eb.Property(c => c.Expires)
      .IsRequired();
      //.HasDefaultValue(new DateTime());
      eb.HasOne<User>(t => t.User)
          .WithOne(u => u.RefreshToken)
          //.HasForeignKey<RefreshToken>(t => t.UserId)
          .HasForeignKey("RefreshToken")
          .HasConstraintName("FK_RefreshToken_UserId")
          .OnDelete(DeleteBehavior.NoAction);
    });

    modelBuilder.Entity<Article>(eb =>
    {
      eb.HasIndex(a => a.Name);
      eb.Property(a => a.Name)
      .IsRequired()
      .HasMaxLength(64)
      .HasDefaultValue("article");
    });

    modelBuilder.Entity<Section>(eb =>
    {
      eb.HasIndex(s => s.Name);
      eb.Property(s => s.Name)
      .IsRequired()
      .HasMaxLength(64)
      .HasDefaultValue("section");
      eb.HasIndex(s => s.Thumbs);
      eb.Property(s => s.Thumbs)
      .IsRequired()
      .HasMaxLength(64)
      .HasDefaultValue(String.Empty);
      eb.HasIndex(s => s.Pages);
      eb.Property(s => s.Pages)
      .IsRequired()
      .HasMaxLength(64)
      .HasDefaultValue(String.Empty);
      eb.Property(s => s.Header)
      .IsRequired()
      .HasMaxLength(256)
      .HasDefaultValue(String.Empty);
      eb.Property(s => s.Description)
      .IsRequired()
      .HasMaxLength(2048)
      .HasDefaultValue(String.Empty);
      eb.Property(s => s.Offset)
      .IsRequired()
      .HasDefaultValue(1);
      eb.Property(s => s.Min)
      .IsRequired()
      .HasDefaultValue(1);
      eb.Property(s => s.Max)
      .IsRequired()
      .HasDefaultValue(1);
      eb.HasOne<Article>(s => s.Article)
        .WithMany(a => a.Sections)
        .HasForeignKey(s => s.ArticleId)
        .HasForeignKey("ArticleId")
        .HasConstraintName("FK_Section_ArticleId")
        .OnDelete(DeleteBehavior.NoAction);
    });

    modelBuilder.Entity<Class>(eb =>
    {
      eb.HasIndex(c => c.Name);
      eb.Property(c => c.Name)
      .IsRequired()
      .HasMaxLength(2)
      .HasDefaultValue("PZ");
    });

    modelBuilder.Entity<Person>(eb =>
    {
      eb.HasIndex(p => p.Last);
      eb.Property(p => p.Last)
      .HasColumnName("Last")
      .IsRequired()
      .HasMaxLength(64)
      .HasDefaultValue(String.Empty);
      eb.HasIndex(p => p.First);
      eb.Property(p => p.First)
      .HasColumnName("First")
      .IsRequired()
      .HasMaxLength(64)
      .HasDefaultValue(String.Empty);
      eb.HasIndex(p => new { p.Last, p.First })
      .IsUnique();
      eb.HasOne<Class>(p => p.Class)
        .WithMany(c => c.Persons)
        .HasForeignKey(p => p.ClassId)
        .HasForeignKey("ClassId")
        .HasConstraintName("FK_Person_ClassId")
        .OnDelete(DeleteBehavior.NoAction);
      eb.Property(p => p.ClassId)
      .HasDefaultValue(1);
    });

    modelBuilder.Entity<Scan>(eb =>
    {
      eb.Property(s => s.Done)
          .IsRequired()
          .HasDefaultValue(false);
      eb.Property(s => s.Page)
          .IsRequired()
          .HasDefaultValue(1);
      eb.Property(s => s.SectionId)
          .IsRequired()
          .HasDefaultValue(1);
      eb.HasOne<Section>(s => s.Section)
          .WithMany(s => s.Scans)
          .HasForeignKey(s => s.SectionId)
          .HasForeignKey("SectionId")
          .HasConstraintName("FK_Scan_SectionId")
          .OnDelete(DeleteBehavior.NoAction);
      eb.Property(s => s.UserId)
          .IsRequired()
          .HasDefaultValue(1);
      eb.HasOne<User>(s => s.User)
          .WithMany(u => u.Scans)
          .HasForeignKey(s => s.UserId)
          .HasForeignKey("UserId")
          .HasConstraintName("FK_Scan_UserId")
          .OnDelete(DeleteBehavior.NoAction);
    });
    modelBuilder.Entity<Indeks>(eb =>
    {
      eb.Property(i => i.ScanId)
          .IsRequired()
          .HasDefaultValue(1);
      eb.HasOne<Scan>(i => i.Scan)
          .WithMany(s => s.Indexes)
          .HasForeignKey(i => i.ScanId)
          .HasForeignKey("ScanId")
          .HasConstraintName("FK_Index_ScanId")
          .OnDelete(DeleteBehavior.NoAction);
      eb.Property(i => i.UserId)
          .IsRequired()
          .HasDefaultValue(1);
      eb.HasOne<User>(i => i.User)
          .WithMany(u => u.Indexes)
          .HasForeignKey(i => i.UserId)
          .HasForeignKey("UserId")
          .HasConstraintName("FK_Index_UserId")
          .OnDelete(DeleteBehavior.NoAction);
      eb.Property(i => i.PersonId)
          .IsRequired()
          .HasDefaultValue(1);
      eb.HasOne<Person>(i => i.Person)
          .WithMany(s => s.Indexes)
          .HasForeignKey(i => i.PersonId)
          .HasForeignKey("PersonId")
          .HasConstraintName("FK_Index_PersonId")
          .OnDelete(DeleteBehavior.NoAction);
    });
    modelBuilder.Entity<UserRole>(eb =>
    {
      eb.HasNoKey();
      eb.Metadata.SetIsTableExcludedFromMigrations(true);
    });
    modelBuilder.Entity<PersonClass>(eb =>
    {
      eb.HasNoKey();
      eb.Metadata.SetIsTableExcludedFromMigrations(true);
    });
    modelBuilder.Entity<PersonGroup>(eb =>
    {
      eb.HasNoKey();
      eb.Metadata.SetIsTableExcludedFromMigrations(true);
    });

  }
  protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
  }
  public virtual int SaveChanges()
    {
        var entries = ChangeTracker
            .Entries()
            .Where(e => e.Entity is CreatedModel && (
                    e.State == EntityState.Added
                    || e.State == EntityState.Modified));

        foreach (var entityEntry in entries)
        {
      if (entityEntry.State == EntityState.Modified)
      {
        ((CreatedModel)entityEntry.Entity).Updated = DateTime.UtcNow;
      }

      if (entityEntry.State == EntityState.Added)
            {
                ((CreatedModel)entityEntry.Entity).Created = DateTime.UtcNow;
            }
        }

        return base.SaveChanges();
    }

    public virtual async Task<int> SaveChangesAsync()
    {
        var entries = ChangeTracker
            .Entries()
            .Where(e => e.Entity is CreatedModel && (
                    e.State == EntityState.Added
                    || e.State == EntityState.Modified));

        foreach (var entityEntry in entries)
        {
            if (entityEntry.State == EntityState.Modified)
            {
                ((CreatedModel)entityEntry.Entity).Updated = DateTime.UtcNow;
            }

            if (entityEntry.State == EntityState.Added)
            {
                ((CreatedModel)entityEntry.Entity).Created = DateTime.UtcNow;
            }
        }
        return await base.SaveChangesAsync();
    }
}
