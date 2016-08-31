using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using EF_Web_Test.Models.Entity;

namespace EF_Web_Test.Models.Mapping
{
    public class TopicArticleMap : EntityTypeConfiguration<TopicArticle>
    {
        public TopicArticleMap()
        {
            // Primary Key
            this.HasKey(t => t.ArticleId);

            // Properties
            this.Property(t => t.Caption)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.Title)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.Content)
                .IsRequired();

            this.Property(t => t.Author)
                .IsRequired()
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("TopicArticle");
            this.Property(t => t.ArticleId).HasColumnName("ArticleId");
            this.Property(t => t.Caption).HasColumnName("Caption");
            this.Property(t => t.Title).HasColumnName("Title");
            this.Property(t => t.Content).HasColumnName("Content");
            this.Property(t => t.CreateTime).HasColumnName("CreateTime");
            this.Property(t => t.Author).HasColumnName("Author");
        }
    }
}
