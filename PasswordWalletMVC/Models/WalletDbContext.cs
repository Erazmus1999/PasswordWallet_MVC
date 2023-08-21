using Microsoft.EntityFrameworkCore;

namespace PasswordWalletMVC.Models
{
    public class WalletDbContext : DbContext
    {
        public WalletDbContext(DbContextOptions<WalletDbContext> options)
            : base(options)
        { }

        public DbSet<User> Users { get; set; }
        public DbSet<Password> Passwords{ get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //FluentApi
            modelBuilder.Entity<User>()
                .Property(u => u.Login)
                .HasMaxLength(30)
                .IsRequired();

            //modelBuilder.Entity<User>()
            //    .HasMany(u => u.UserPasswords)
            //    .WithOne(u => u.User);

        }
    }
}
