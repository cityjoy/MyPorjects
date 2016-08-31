using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using EF_Web_Test.Models.Entity;
namespace EF_Web_Test.Models.Mapping
{
    public class SubjectCommentMap : EntityTypeConfiguration<SubjectComment>
    {
        public SubjectCommentMap()
        {
            // Primary Key
            this.HasKey(t => t.CommentId);

            // Properties
            this.Property(t => t.CommentId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.Content)
                .IsRequired();

            // Table & Column Mappings
            this.ToTable("SubjectComment");
            this.Property(t => t.CommentId).HasColumnName("CommentId");
            this.Property(t => t.SubjectId).HasColumnName("SubjectId");
            this.Property(t => t.Content).HasColumnName("Content");
        }
    }
}
