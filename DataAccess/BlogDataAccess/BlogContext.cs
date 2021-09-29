using Core.Entities;
using Microsoft.EntityFrameworkCore;


namespace DataAccess.BlogDataAccess
{
    public class BlogContext : DbContext
    {
        public BlogContext(DbContextOptions<BlogContext> options) : base(options)
        {

        }
        public DbSet<User> Users { get; set; }
        public DbSet<Article> Articles { get; set; }
        public DbSet<ArticleTopic> ArticleTopics { get; set; }
        public DbSet<Topic> Topics { get; set; }
        public DbSet<UserTopic> UserTopics { get; set; }
        public DbSet<DeletedUser> DeletedUsers { get; set; }
        public DbSet<DeletedArticle> DeletedArticles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ArticleTopic>()
                .HasKey(m => new { m.ArticleID, m.TopicID });

            modelBuilder.Entity<UserTopic>()
                .HasKey(m => new { m.UserID, m.TopicID });

            modelBuilder.Entity<User>()
           .HasIndex(b => b.Email)
           .IsUnique();

            modelBuilder.Entity<User>()
           .HasIndex(b => b.UserName)
           .IsUnique();

            modelBuilder.Entity<User>()
            .HasIndex(b => b.Url)
            .IsUnique();

            modelBuilder.Entity<Topic>()
           .HasIndex(b => b.TopicName)
           .IsUnique();


        }

    }
}
