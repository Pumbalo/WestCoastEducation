using Microsoft.EntityFrameworkCore;
using westcoast_education.api.Models;

namespace westcoast_education.api.Data;

public class WestcoastEducationsContext : DbContext
{
    public DbSet<CourseModel> Courses => Set<CourseModel>();
    public DbSet<StudentModel> Students => Set<StudentModel>();
    public DbSet<TeacherModel> Teachers => Set<TeacherModel>();
    public DbSet<StudentCourse> StudentCourse => Set<StudentCourse>();
    public DbSet<CompetenceModel> Competence => Set<CompetenceModel>();
    public WestcoastEducationsContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<StudentCourse>()
            .HasKey(sc => new { sc.CourseId, sc.StudentId });

        modelBuilder.Entity<StudentCourse>()
            .HasOne(sc => sc.Student)
            .WithMany(c => c.StudentCourses)
            .HasForeignKey(sc => sc.StudentId);

        modelBuilder.Entity<StudentCourse>()
            .HasOne(sc => sc.Course)
            .WithMany(c => c.StudentCourse)
            .HasForeignKey(sc => sc.CourseId);
    }
}
