using Assignment2_CurdOperation.Identity;
using Assignment2_CurdOperation.Modals;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Security.Principal;

namespace Assignment2_CurdOperation.Data
{

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Student> Students { get; set; }
        public DbSet<Hobby> Hobbies { get; set; }
        public DbSet<StudentHobby> StudentHobbies { get; set; }
        public DbSet<ApplicationRole> ApplicationRoles { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<StudentHobby>().HasKey(SH => new { SH.StudentId, SH.HobbyId });
            modelBuilder.Entity<Hobby>().HasData(
            new Hobby { Id = 7, Name = "Reading" },
            new Hobby { Id = 8, Name = "Playing" },
            new Hobby { Id = 9, Name = "Writting" },
            new Hobby { Id = 10, Name = "Cooking" }

            );
           // modelBuilder.Entity<StudentHobby>().HasOne(x => x.Student).WithMany(x => x.StudentHobby).HasForeignKey(x => x.StudentId);
            //modelBuilder.Entity<StudentHobby>().HasOne(x => x.Hobby).WithMany(x => x.StudentHobby).HasForeignKey(x => x.HobbyId);
               
        }

    }
}
