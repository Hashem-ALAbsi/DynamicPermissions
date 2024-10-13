using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DynamicPermissions.Models
{
    public class UserRole: IdentityUserRole<string>
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int RoleId { get; set; }

        public ApplicationUser User { get; set; }
        public Role Role { get; set; }
    }

    //public class UserRoleConfiguration : IEntityTypeConfiguration<UserRole>
    //{
    //    public void Configure(EntityTypeBuilder<UserRole> builder)
    //    {
    //        builder.HasOne(p => p.User)
    //            .WithMany(p => p.UserRoles)
    //            .HasForeignKey(p => p.UserId);

    //        builder.HasOne(p => p.Role)
    //            .WithMany(p => p.UserRoles)
    //            .HasForeignKey(p => p.RoleId);
    //    }
    //}
}