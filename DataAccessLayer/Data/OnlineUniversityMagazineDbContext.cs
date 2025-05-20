using DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

using System.Text;
using System.Threading.Tasks;
using System.Reflection.Emit;
using System.Diagnostics;



namespace DataAccessLayer.Data
{
    public class OnlineUniversityMagazineDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Contribution> Contributions { get; set; }
        public DbSet<Faculty> Faculties { get; set;}
     
        public DbSet<Comment> Comments { get; set; }
        public DbSet<AcademicTerm> AcademicTerms { get; set; }

        public OnlineUniversityMagazineDbContext(DbContextOptions<OnlineUniversityMagazineDbContext> options) : base(options)
        {


        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
          
            base.OnModelCreating(modelBuilder);
     
            
            modelBuilder.Entity<User>()
                .HasKey(u => u.UserId);
            modelBuilder.Entity<User>()
                .HasOne(u => u.Faculty)
                .WithMany(f => f.Users)
                .HasForeignKey(u => u.FacultyId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<User>()
                .HasMany(u => u.Contributions)
                .WithOne(c => c.User)
                .HasForeignKey(c => c.UserId);
           

            
            modelBuilder.Entity<Faculty>()
                .HasKey(f => f.FacultyId);
            
            modelBuilder.Entity<Faculty>()
                .HasMany(f => f.Contributions)
                .WithOne(c => c.Faculty)
                .HasForeignKey(c => c.FacultyId)
                .OnDelete(DeleteBehavior.Restrict); 
           

            modelBuilder.Entity<Contribution>()
                .HasKey(c => c.ContributionId);
            modelBuilder.Entity<Contribution>()
                .HasMany(c => c.Comments)
                .WithOne(cm => cm.Contribution)
                .HasForeignKey(cm => cm.ContributionId);
            modelBuilder.Entity<Contribution>()
                .HasOne(c => c.Faculty)
                .WithMany(f => f.Contributions)
                .HasForeignKey(c => c.FacultyId)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Contribution>()
               .HasOne(c => c.User)
               .WithMany(u => u.Contributions)
               .HasForeignKey(at => at.UserId)
               .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Contribution>()
              .HasOne(c => c.AcademicTerm)
              .WithMany(at => at.Contributions)
              .HasForeignKey(at => at.AcademicTermId)
              .OnDelete(DeleteBehavior.Restrict);


            modelBuilder.Entity<Comment>()
                .HasKey(cm => cm.CommentId);

            modelBuilder.Entity<AcademicTerm>()
                .HasKey(at => at.AcademicTermId);
           
        }


    }
}
