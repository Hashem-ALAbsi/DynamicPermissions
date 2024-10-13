using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations.Schema;

namespace DynamicPermissions.Models
{
    public class Permission
    {
        public int Id { get; set; }
        public string ActionFullName { get; set; }
        //[ForeignKey("RoleId")]
        //public int RoleId { get; set; }
   //     public Role Role { get; set; }
    }

    //public class PermissionConfiguration : IEntityTypeConfiguration<Permission>
    //{
    //    public void Configure(EntityTypeBuilder<Permission> builder)
    //    {
    //        builder.Property(p => p.ActionFullName).HasMaxLength(200);

    //        builder.HasOne(p => p.Role)
    //            .WithMany(p => p.Permissions)
    //            .HasForeignKey(p => p.RoleId);
    //    }
    //}
}