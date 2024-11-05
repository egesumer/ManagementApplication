using Microsoft.EntityFrameworkCore;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; } 

        public DbSet<SupportForm> SupportForms { get; set;}
        
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

    
        modelBuilder.Entity<SupportForm>()
            .HasOne(x => x.FilledBy)
            .WithMany()
            .HasForeignKey(y => y.FilledById)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<User>()
            .ToTable("User")
            .HasIndex(u => u.Username)
            .IsUnique(); 

        //  Superadmin kullanıcısını seed data olarak ekleme
        modelBuilder.Entity<User>().HasData(
            new User
            {
                Id = Guid.Parse("d2f62e3e-4db4-4ae7-a9f7-96c5d90f77fc"), 
                Username = "superadmin",
                Password = PasswordHasher.HashPassword("superadmin"),
                UserType = UserType.Superadmin
            }
        );
    }

}