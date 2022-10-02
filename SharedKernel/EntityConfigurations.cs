using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SharedKernel
{
    public class EntityConfigurations
    {
        public class QuizConfiguration : IEntityTypeConfiguration<Quiz>
        {
            public void Configure(EntityTypeBuilder<Quiz> quiz)
            {
                quiz.HasKey(q => q.Id);
                //quiz.HasIndex(q => q.Code)
                //    .IsUnique();
            }
        }
        public class QuizResultQuestionsConfiguration : IEntityTypeConfiguration<QuizResultQuestions>
        {
            public void Configure(EntityTypeBuilder<QuizResultQuestions> quizResultQuestions)
            {
                quizResultQuestions.HasOne(qrq => qrq.QuizResult)
                    .WithMany(q => q.QuizResultQuestions) 
                    .OnDelete(DeleteBehavior.Restrict);
                 
                    
            }
        }

        public class QuizResultQuestionsAnswersConfiguration : IEntityTypeConfiguration<QuizResultQuestionsAnswer>
        {
            public void Configure(EntityTypeBuilder<QuizResultQuestionsAnswer> quizResultQuestionsAnswers)
            {
                quizResultQuestionsAnswers.HasOne(qrq => qrq.QuizResultQuestion).WithMany(q => q.QuizResultQuestionsAnswers).OnDelete(DeleteBehavior.Restrict);
            }
        }


        public class UserConfiguration : IEntityTypeConfiguration<User>
        {
            public void Configure(EntityTypeBuilder<User> user)
            {
                user.ToTable("User");
            }
        }



    }
}