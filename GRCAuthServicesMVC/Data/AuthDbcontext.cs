using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GRCAuthServicesMVC.Data
{
    public class AuthDbcontext :IdentityDbContext
    {
        public AuthDbcontext(DbContextOptions<AuthDbcontext> options): base(options)
        {
            
        }


        //protected override void OnModelCreating(ModelBuilder builder)
        //{
        //    base.OnModelCreating(builder);
        //    var ReaderId = "b3f6bc63-a255-43a1-99f8-2ac4cb8b3a66";
        //    var WriteId = "905e8fef-8adb-42dc-8640-1b3bda133998";
        //    var roles = new List<IdentityRole>
        //    {
        //        new IdentityRole
        //        {
        //            Id = ReaderId,
        //            ConcurrencyStamp = ReaderId,
        //            Name = "Reader",
        //            NormalizedName = "Reader".ToUpper()

        //        },

        //        new IdentityRole
        //        {
        //            Id = WriteId,
        //            ConcurrencyStamp = WriteId,
        //            Name = "Writer",
        //            NormalizedName = "Writer".ToUpper()
        //        }
        //    };



            //builder.Entity<IdentityRole>().HasData(roles);

        }










    }
