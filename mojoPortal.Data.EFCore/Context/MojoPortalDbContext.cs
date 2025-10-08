using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using mojoPortal.Data.EFCore.Entities;

namespace mojoPortal.Data.EFCore.Context
{
    /// <summary>
    /// Entity Framework Core DbContext for mojoPortal data access.
    /// Supports multiple database providers (SQL Server, SQLite) with configuration-based provider selection.
    /// </summary>
    public class MojoPortalDbContext : DbContext
    {
        /// <summary>
        /// Gets or sets the Comments DbSet for accessing comment entities.
        /// </summary>
        public DbSet<CommentEntity> Comments { get; set; } = null!;

        /// <summary>
        /// Initializes a new instance of the <see cref="MojoPortalDbContext"/> class.
        /// </summary>
        /// <param name="options">The options for this context.</param>
        public MojoPortalDbContext(DbContextOptions<MojoPortalDbContext> options)
            : base(options)
        {
        }

        /// <summary>
        /// Configures the schema needed for the mojoPortal application.
        /// Applies entity configurations using Fluent API.
        /// </summary>
        /// <param name="modelBuilder">The builder being used to construct the model for this context.</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Apply all entity configurations from the Configurations assembly
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(MojoPortalDbContext).Assembly);
        }

        /// <summary>
        /// Configures additional context options if not already configured.
        /// Primarily used for design-time tooling and development scenarios.
        /// </summary>
        /// <param name="optionsBuilder">The options builder to configure.</param>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            // Only configure if not already configured (e.g., from dependency injection)
            if (!optionsBuilder.IsConfigured)
            {
                // Development default: SQLite in-memory database
                // This allows EF Core tools to work without explicit configuration
                optionsBuilder.UseSqlite("Data Source=:memory:");
            }

            // Enable detailed logging in development environments
            #if DEBUG
            optionsBuilder
                .EnableSensitiveDataLogging()
                .EnableDetailedErrors()
                .LogTo(Console.WriteLine, LogLevel.Information);
            #endif
        }
    }
}
