using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TestSystem.API.Core.Entities;

namespace TestSystem.API
{
    public class ApplicationContext : IdentityDbContext<User>
    {
        public DbSet<Test> Tests { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Answer> Answers { get; set; }
        public DbSet<Attempt> Attempts { get; set; }

        public ApplicationContext()
        {
            Database.EnsureCreated();
        }

        public ApplicationContext(DbContextOptions options) : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Test>(e =>
            {
                e.HasKey(t => t.Id);
                e.HasIndex(t => t.Id);
                e.HasData(
                    new Test { Id = 1, Name = "History of Ukraine", Category = "History" },
                    new Test { Id = 2, Name = "Famous Places", Category = "Travelling" },
                    new Test { Id = 3, Name = "Test 3", Category = "In Progress" },
                    new Test { Id = 4, Name = "Test 4", Category = "In Progress" },
                    new Test { Id = 5, Name = "Test 5", Category = "In Progress" },
                    new Test { Id = 6, Name = "Test 6", Category = "In Progress" }
                    );
            });

            builder.Entity<Question>(e =>
            {
                e.HasKey(q => q.Id);
                e.HasIndex(q => q.Id);
                e.Ignore(q => q.State);
                e.HasOne(q => q.Test)
                    .WithMany(t => t.Questions)
                    .HasForeignKey(q => q.TestId)
                    .OnDelete(DeleteBehavior.Cascade);
                e.HasData(
                    new Question { Id = 1, TestId = 1, Text = "When is the independence day?", PointsWorth = 7 },
                    new Question { Id = 2, TestId = 1, Text = "Which of these people were presidents?", PointsWorth = 8, IsMultipleChoice = true }
                    );
            });

            builder.Entity<Answer>(e =>
            {
                e.HasKey(an => an.Id);
                e.HasIndex(an => an.Id);
                e.Ignore(an => an.State);
                e.HasOne(an => an.Question)
                    .WithMany(q => q.Answers)
                    .HasForeignKey(an => an.QuestionId)
                    .OnDelete(DeleteBehavior.Cascade);
                e.HasData(
                    new Answer { Id = 1, QuestionId = 1, Text = "May 14" },
                    new Answer { Id = 2, QuestionId = 1, Text = "August 24", IsCorrect = true },
                    new Answer { Id = 3, QuestionId = 1, Text = "July 4" },
                    new Answer { Id = 4, QuestionId = 2, Text = "Leonid Kravchuk", IsCorrect = true },
                    new Answer { Id = 5, QuestionId = 2, Text = "Taras Shevchenko" },
                    new Answer { Id = 6, QuestionId = 2, Text = "Bogdan Khmelnytskyi" },
                    new Answer { Id = 7, QuestionId = 2, Text = "Petro Poroshenko", IsCorrect = true }
                    );
            });

            builder.Entity<Attempt>(e =>
            {
                e.HasKey(at => at.Id);
                e.HasIndex(at => at.Id);

                e.HasOne(at => at.Test)
                    .WithMany(t => t.Attempts)
                    .HasForeignKey(at => at.TestId)
                    .OnDelete(DeleteBehavior.Cascade);

                e.HasOne(at => at.User)
                    .WithMany(u => u.Attempts)
                    .HasForeignKey(at => at.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}
