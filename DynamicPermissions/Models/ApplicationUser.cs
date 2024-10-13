using DynamicPermissions.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Collections.Generic;

namespace DynamicPermissions.Models
{
    public class ApplicationUser:IdentityUser
    {
        
        public string FullName { get; set; }

      //  public ICollection<UserRole>? UserRoles { get; set; }
    }

    //public class UserConfiguration : IEntityTypeConfiguration<User>
    //{
    //    public void Configure(EntityTypeBuilder<User> builder)
    //    {
    //        builder.Property(p => p.FullName).HasMaxLength(50);
    //        builder.Property(p => p.UserName).HasMaxLength(50);
    //    }
    //}
}