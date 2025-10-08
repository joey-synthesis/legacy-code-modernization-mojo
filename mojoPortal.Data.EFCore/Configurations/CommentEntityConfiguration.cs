using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using mojoPortal.Data.EFCore.Entities;

namespace mojoPortal.Data.EFCore.Configurations
{
    /// <summary>
    /// Entity Framework Core Fluent API configuration for CommentEntity.
    /// Maps to the existing mp_Comments table with all constraints, relationships, and indexes.
    /// </summary>
    public class CommentEntityConfiguration : IEntityTypeConfiguration<CommentEntity>
    {
        public void Configure(EntityTypeBuilder<CommentEntity> builder)
        {
            // Table mapping to existing mp_Comments table
            builder.ToTable("mp_Comments");

            // Primary key configuration
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id)
                .HasColumnName("Guid")
                .IsRequired()
                .ValueGeneratedOnAdd(); // Guid auto-generation

            // Guid columns - all required except ModeratedBy
            builder.Property(e => e.ParentGuid)
                .HasColumnName("ParentGuid")
                .IsRequired();

            builder.Property(e => e.SiteGuid)
                .HasColumnName("SiteGuid")
                .IsRequired();

            builder.Property(e => e.FeatureGuid)
                .HasColumnName("FeatureGuid")
                .IsRequired();

            builder.Property(e => e.ModuleGuid)
                .HasColumnName("ModuleGuid")
                .IsRequired();

            builder.Property(e => e.ContentGuid)
                .HasColumnName("ContentGuid")
                .IsRequired();

            builder.Property(e => e.UserGuid)
                .HasColumnName("UserGuid")
                .IsRequired();

            // String properties with length constraints
            builder.Property(e => e.Title)
                .HasColumnName("Title")
                .HasMaxLength(255)
                .IsRequired(false); // Optional field

            builder.Property(e => e.UserComment)
                .HasColumnName("UserComment")
                .IsRequired(false); // Optional field, unlimited length

            builder.Property(e => e.UserName)
                .HasColumnName("UserName")
                .HasMaxLength(50)
                .IsRequired(false); // Optional field

            builder.Property(e => e.UserEmail)
                .HasColumnName("UserEmail")
                .HasMaxLength(100)
                .IsRequired(false); // Optional field

            builder.Property(e => e.UserUrl)
                .HasColumnName("UserUrl")
                .HasMaxLength(255)
                .IsRequired(false); // Optional field

            builder.Property(e => e.UserIp)
                .HasColumnName("UserIp")
                .HasMaxLength(50)
                .IsRequired(false); // Optional field

            // DateTime properties with default values
            // Note: Database-specific defaults will be applied in migrations
            // SQLite uses: CURRENT_TIMESTAMP
            // SQL Server uses: GETUTCDATE()
            builder.Property(e => e.CreatedUtc)
                .HasColumnName("CreatedUtc")
                .IsRequired()
                .ValueGeneratedOnAdd();

            builder.Property(e => e.LastModUtc)
                .HasColumnName("LastModUtc")
                .IsRequired()
                .ValueGeneratedOnAdd();

            // Moderation properties
            builder.Property(e => e.ModerationStatus)
                .HasColumnName("ModerationStatus")
                .IsRequired()
                .HasDefaultValue((byte)1) // Default: Approved
                .ValueGeneratedOnAdd(); // Ensures EF Core uses database-generated value

            builder.Property(e => e.ModeratedBy)
                .HasColumnName("ModeratedBy")
                .IsRequired(false); // Nullable Guid

            builder.Property(e => e.ModerationReason)
                .HasColumnName("ModerationReason")
                .HasMaxLength(255)
                .IsRequired(false); // Optional field

            // Self-referencing relationship: Parent-Children hierarchy
            builder.HasOne(e => e.Parent)
                .WithMany(e => e.Children)
                .HasForeignKey(e => e.ParentGuid)
                .HasPrincipalKey(e => e.Id)
                .OnDelete(DeleteBehavior.Restrict); // Prevent cascade delete, handle manually

            // Performance indexes
            // Index on ContentGuid for retrieving all comments for a specific content item
            builder.HasIndex(e => e.ContentGuid)
                .HasDatabaseName("IX_mp_Comments_ContentGuid");

            // Index on SiteGuid for site-level operations
            builder.HasIndex(e => e.SiteGuid)
                .HasDatabaseName("IX_mp_Comments_SiteGuid");

            // Index on ParentGuid for hierarchical queries
            builder.HasIndex(e => e.ParentGuid)
                .HasDatabaseName("IX_mp_Comments_ParentGuid");

            // Index on CreatedUtc for date-based queries and sorting
            builder.HasIndex(e => e.CreatedUtc)
                .HasDatabaseName("IX_mp_Comments_CreatedUtc");

            // Composite index for common query pattern: content + moderation status + date
            builder.HasIndex(e => new { e.ContentGuid, e.ModerationStatus, e.CreatedUtc })
                .HasDatabaseName("IX_mp_Comments_Content_Status_Date");

            // Composite index for parent hierarchy queries
            builder.HasIndex(e => new { e.ParentGuid, e.CreatedUtc })
                .HasDatabaseName("IX_mp_Comments_Parent_Date");
        }
    }
}
