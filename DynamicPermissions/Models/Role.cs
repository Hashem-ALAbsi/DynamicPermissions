using DynamicPermissions.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Collections.Generic;

namespace DynamicPermissions.Models
{
    public class Role:IdentityRole
    {
        public Role(string Name)
        {
            this.Name = Name;   
        }
        public Role()
        {
        }
        //public ICollection<Permission> Permissions { get; set; }
        //public ICollection<UserRole>? UserRoles { get; set; }
    }

    //public class RoleConfiguration : IEntityTypeConfiguration<Role>
    //{
    //    public void Configure(EntityTypeBuilder<Role> builder)
    //    {
    //        builder.Property(p => p.Name).HasMaxLength(50);
    //    }
    //}
}