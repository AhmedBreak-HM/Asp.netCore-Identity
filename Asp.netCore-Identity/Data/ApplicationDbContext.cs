using Asp.netCore_Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Asp.netCore_Identity.Data
{
    public class ApplicationDbContext : IdentityDbContext<User, Role, int, IdentityUserClaim<int>, UserRole,
                                                          IdentityUserLogin<int>, IdentityRoleClaim<int>,
                                                          IdentityUserToken<int>>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<UserRole>().HasKey(k => new { k.UserId, k.RoleId });
            builder.Entity<User>().HasMany(u => u.UserRoles)
                   .WithOne(u => u.User).HasForeignKey(u => u.UserId)
                   .IsRequired();
            builder.Entity<Role>().HasMany(r => r.UserRoles)
                   .WithOne(r => r.Role).HasForeignKey(r => r.RoleId)
                   .IsRequired();

            builder.Entity<Role>().HasData(new Role[] {
                    new Role{ Id=1, Name="Admin",NormalizedName="ADMIN"},
                    new Role{ Id=2, Name="Moderator", NormalizedName="MODERATOR"},
                    new Role{ Id=3, Name="Member", NormalizedName="MEMBER"},
                    new Role{ Id=4, Name="VIP", NormalizedName="VIP"}});




        }
    }
}