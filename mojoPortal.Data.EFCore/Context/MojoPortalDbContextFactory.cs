using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace mojoPortal.Data.EFCore.Context
{
    /// <summary>
    /// Design-time factory for creating MojoPortalDbContext instances.
    /// Used by EF Core tools (migrations, scaffolding) when the context cannot be created through dependency injection.
    /// </summary>
    public class MojoPortalDbContextFactory : IDesignTimeDbContextFactory<MojoPortalDbContext>
    {
        /// <summary>
        /// Creates a new instance of MojoPortalDbContext for design-time operations.
        /// Uses SQLite in-memory database by default for tool operations.
        /// </summary>
        /// <param name="args">Command-line arguments passed to the tool.</param>
        /// <returns>A new instance of MojoPortalDbContext configured for design-time use.</returns>
        public MojoPortalDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<MojoPortalDbContext>();

            // Default to SQLite for design-time operations
            // This allows migrations and other EF tools to work without a live database
            optionsBuilder.UseSqlite("Data Source=mojoportal_design.db");

            // Enable detailed errors for design-time troubleshooting
            optionsBuilder.EnableDetailedErrors();
            optionsBuilder.EnableSensitiveDataLogging();

            return new MojoPortalDbContext(optionsBuilder.Options);
        }
    }
}
