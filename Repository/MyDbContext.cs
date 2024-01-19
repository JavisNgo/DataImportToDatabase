using DataImportToDatabase.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataImportToDatabase.Repository
{
    public class MyDbContext : DbContext
    {
        //Singleton
        //private static MyDbContext instance;
        //MyDbContext() { }
        //public static MyDbContext Instance
        //{
        //    get
        //    {
        //        if(instance == null) { instance = new MyDbContext(); }
        //        return instance;
        //    }
        //}

        public DbSet<Student> students { get; set; }
        public DbSet<Subject> subjects { get; set; }
        public DbSet<SchoolYear> schoolYear { get; set; }
        public DbSet<Score> scores { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source=LAPTOP-MQ52Q21P\\MSSQLSERVER01;Initial Catalog=ScoreReport;User ID=sa;Password=12345;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Student>()
                .HasOne(s => s.SchoolYear)
                .WithMany(sy => sy.Students)
                .HasForeignKey(s => s.SchoolYearId);

            modelBuilder.Entity<Score>()
                .HasOne(score => score.Student)
                .WithMany(student => student.Scores)
                .HasForeignKey(score => score.StudentID);

            modelBuilder.Entity<Score>()
                .HasOne(score => score.Subject)
                .WithMany(subject => subject.Scores)
                .HasForeignKey(score => score.SubjectID);
        }
    }
}
