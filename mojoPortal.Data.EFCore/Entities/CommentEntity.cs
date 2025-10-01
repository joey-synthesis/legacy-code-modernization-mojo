using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace mojoPortal.Data.EFCore.Entities
{
    /// <summary>
    /// Entity Framework Core entity representing a comment in the mojoPortal system.
    /// Maps to the mp_Comments table in the database.
    /// </summary>
    [Table("mp_Comments")]
    public class CommentEntity
    {
        /// <summary>
        /// Gets or sets the unique identifier for the comment.
        /// This is the primary key.
        /// </summary>
        [Key]
        [Column("Guid")]
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier of the parent comment.
        /// Used for hierarchical comment threading. Empty Guid if this is a top-level comment.
        /// </summary>
        [Column("ParentGuid")]
        public Guid ParentGuid { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier of the site this comment belongs to.
        /// </summary>
        [Required]
        [Column("SiteGuid")]
        public Guid SiteGuid { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier of the feature this comment belongs to.
        /// </summary>
        [Required]
        [Column("FeatureGuid")]
        public Guid FeatureGuid { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier of the module this comment belongs to.
        /// </summary>
        [Required]
        [Column("ModuleGuid")]
        public Guid ModuleGuid { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier of the content this comment is attached to.
        /// </summary>
        [Required]
        [Column("ContentGuid")]
        public Guid ContentGuid { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier of the user who created the comment.
        /// </summary>
        [Column("UserGuid")]
        public Guid UserGuid { get; set; }

        /// <summary>
        /// Gets or sets the title of the comment.
        /// </summary>
        [MaxLength(255)]
        [Column("Title")]
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the comment text content.
        /// </summary>
        [Column("UserComment")]
        public string UserComment { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the name of the user who created the comment.
        /// </summary>
        [MaxLength(50)]
        [Column("UserName")]
        public string UserName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the email address of the user who created the comment.
        /// </summary>
        [MaxLength(100)]
        [Column("UserEmail")]
        public string UserEmail { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the URL provided by the user who created the comment.
        /// </summary>
        [MaxLength(255)]
        [Column("UserUrl")]
        public string UserUrl { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the IP address of the user who created the comment.
        /// </summary>
        [MaxLength(50)]
        [Column("UserIp")]
        public string UserIp { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the UTC date and time when the comment was created.
        /// </summary>
        [Required]
        [Column("CreatedUtc")]
        public DateTime CreatedUtc { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Gets or sets the UTC date and time when the comment was last modified.
        /// </summary>
        [Required]
        [Column("LastModUtc")]
        public DateTime LastModUtc { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Gets or sets the moderation status of the comment.
        /// 0 = Pending, 1 = Approved, 2 = Spam, 3 = Rejected
        /// </summary>
        [Required]
        [Column("ModerationStatus")]
        public byte ModerationStatus { get; set; } = 1;

        /// <summary>
        /// Gets or sets the unique identifier of the user who moderated this comment.
        /// </summary>
        [Column("ModeratedBy")]
        public Guid? ModeratedBy { get; set; }

        /// <summary>
        /// Gets or sets the reason provided for the moderation action.
        /// </summary>
        [MaxLength(255)]
        [Column("ModerationReason")]
        public string ModerationReason { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the parent comment entity for this comment.
        /// Used for hierarchical comment threading.
        /// </summary>
        [ForeignKey(nameof(ParentGuid))]
        public virtual CommentEntity? Parent { get; set; }

        /// <summary>
        /// Gets or sets the collection of child comments.
        /// Used for hierarchical comment threading.
        /// </summary>
        [InverseProperty(nameof(Parent))]
        public virtual ICollection<CommentEntity> Children { get; set; } = new List<CommentEntity>();
    }
}
