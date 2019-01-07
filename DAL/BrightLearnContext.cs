using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using Models.DataModels;

namespace DAL
{
    public class BrightLearnContext : DbContext
    {
        public BrightLearnContext() : base("BrightLearnContext")
        {
            Database.SetInitializer<BrightLearnContext>(null);
        }

        public DbSet<Chart> Chart { get; set; }
        public DbSet<ChartGroup> ChartGroup { get; set; }
        public DbSet<ChartValues> ChartValues { get; set; }
        
        public DbSet<Game> Game { get; set; }
        public DbSet<GameScore> GameScore { get; set; }
        public DbSet<GameStatistics> GameStatistics { get; set; }
        public DbSet<HighScore> HighScore { get; set; }

        public DbSet<ImageQuestion> ImageQuestion { get; set; }
        public DbSet<MultipleChoiseQuestion> MultipleChoiseQuestion { get; set; }
        public DbSet<MultipleChoiseQuestionAnswer> MultipleChoiseQuestionAnswer { get; set; }
        public DbSet<Question> Question { get; set; }

        public DbSet<Login> Login { get; set; }
        public DbSet<User> User { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}