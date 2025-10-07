using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace mojoPortal.Data.EFCore.Context
{
    /// <summary>
    /// Provides configuration helpers for setting up MojoPortalDbContext with different database providers.
    /// Supports SQL Server, SQLite, and development vs production configurations.
    /// </summary>
    public static class DbContextConfiguration
    {
        /// <summary>
        /// Database provider types supported by mojoPortal.
        /// </summary>
        public enum DatabaseProvider
        {
            /// <summary>SQL Server database provider</summary>
            SqlServer,
            /// <summary>SQLite database provider</summary>
            Sqlite
        }

        /// <summary>
        /// Configures MojoPortalDbContext with SQL Server provider.
        /// </summary>
        /// <param name="services">The service collection to add the context to.</param>
        /// <param name="connectionString">The SQL Server connection string.</param>
        /// <param name="isDevelopment">Whether this is a development environment (enables additional logging).</param>
        /// <returns>The service collection for chaining.</returns>
        public static IServiceCollection AddMojoPortalDbContextWithSqlServer(
            this IServiceCollection services,
            string connectionString,
            bool isDevelopment = false)
        {
            services.AddDbContext<MojoPortalDbContext>(options =>
            {
                options.UseSqlServer(connectionString, sqlOptions =>
                {
                    // Enable retry on failure for production resilience
                    sqlOptions.EnableRetryOnFailure(
                        maxRetryCount: 3,
                        maxRetryDelay: TimeSpan.FromSeconds(5),
                        errorNumbersToAdd: null);

                    // Command timeout for long-running queries
                    sqlOptions.CommandTimeout(30);
                });

                ConfigureDevelopmentOptions(options, isDevelopment);
            });

            return services;
        }

        /// <summary>
        /// Configures MojoPortalDbContext with SQLite provider.
        /// </summary>
        /// <param name="services">The service collection to add the context to.</param>
        /// <param name="connectionString">The SQLite connection string.</param>
        /// <param name="isDevelopment">Whether this is a development environment (enables additional logging).</param>
        /// <returns>The service collection for chaining.</returns>
        public static IServiceCollection AddMojoPortalDbContextWithSqlite(
            this IServiceCollection services,
            string connectionString,
            bool isDevelopment = false)
        {
            services.AddDbContext<MojoPortalDbContext>(options =>
            {
                options.UseSqlite(connectionString, sqliteOptions =>
                {
                    // Command timeout for long-running queries
                    sqliteOptions.CommandTimeout(30);
                });

                ConfigureDevelopmentOptions(options, isDevelopment);
            });

            return services;
        }

        /// <summary>
        /// Configures MojoPortalDbContext with automatic provider detection based on connection string.
        /// </summary>
        /// <param name="services">The service collection to add the context to.</param>
        /// <param name="connectionString">The database connection string.</param>
        /// <param name="provider">The database provider type.</param>
        /// <param name="isDevelopment">Whether this is a development environment (enables additional logging).</param>
        /// <returns>The service collection for chaining.</returns>
        public static IServiceCollection AddMojoPortalDbContext(
            this IServiceCollection services,
            string connectionString,
            DatabaseProvider provider,
            bool isDevelopment = false)
        {
            return provider switch
            {
                DatabaseProvider.SqlServer => services.AddMojoPortalDbContextWithSqlServer(connectionString, isDevelopment),
                DatabaseProvider.Sqlite => services.AddMojoPortalDbContextWithSqlite(connectionString, isDevelopment),
                _ => throw new ArgumentException($"Unsupported database provider: {provider}", nameof(provider))
            };
        }

        /// <summary>
        /// Creates DbContextOptions for SQL Server (useful for testing or manual instantiation).
        /// </summary>
        /// <param name="connectionString">The SQL Server connection string.</param>
        /// <param name="isDevelopment">Whether this is a development environment.</param>
        /// <returns>Configured DbContextOptions.</returns>
        public static DbContextOptions<MojoPortalDbContext> CreateSqlServerOptions(
            string connectionString,
            bool isDevelopment = false)
        {
            var optionsBuilder = new DbContextOptionsBuilder<MojoPortalDbContext>();

            optionsBuilder.UseSqlServer(connectionString, sqlOptions =>
            {
                sqlOptions.EnableRetryOnFailure(
                    maxRetryCount: 3,
                    maxRetryDelay: TimeSpan.FromSeconds(5),
                    errorNumbersToAdd: null);
                sqlOptions.CommandTimeout(30);
            });

            ConfigureDevelopmentOptions(optionsBuilder, isDevelopment);

            return optionsBuilder.Options;
        }

        /// <summary>
        /// Creates DbContextOptions for SQLite (useful for testing or manual instantiation).
        /// </summary>
        /// <param name="connectionString">The SQLite connection string.</param>
        /// <param name="isDevelopment">Whether this is a development environment.</param>
        /// <returns>Configured DbContextOptions.</returns>
        public static DbContextOptions<MojoPortalDbContext> CreateSqliteOptions(
            string connectionString,
            bool isDevelopment = false)
        {
            var optionsBuilder = new DbContextOptionsBuilder<MojoPortalDbContext>();

            optionsBuilder.UseSqlite(connectionString, sqliteOptions =>
            {
                sqliteOptions.CommandTimeout(30);
            });

            ConfigureDevelopmentOptions(optionsBuilder, isDevelopment);

            return optionsBuilder.Options;
        }

        /// <summary>
        /// Creates in-memory SQLite options for testing purposes.
        /// </summary>
        /// <param name="databaseName">Optional database name for isolation between tests.</param>
        /// <returns>Configured DbContextOptions for in-memory SQLite.</returns>
        public static DbContextOptions<MojoPortalDbContext> CreateInMemoryOptions(string? databaseName = null)
        {
            var optionsBuilder = new DbContextOptionsBuilder<MojoPortalDbContext>();

            var connectionString = string.IsNullOrEmpty(databaseName)
                ? "Data Source=:memory:"
                : $"Data Source={databaseName};Mode=Memory;Cache=Shared";

            optionsBuilder.UseSqlite(connectionString);

            // Always enable development options for testing
            ConfigureDevelopmentOptions(optionsBuilder, isDevelopment: true);

            return optionsBuilder.Options;
        }

        /// <summary>
        /// Configures development-specific options like sensitive data logging and detailed errors.
        /// </summary>
        /// <param name="optionsBuilder">The options builder to configure.</param>
        /// <param name="isDevelopment">Whether to enable development features.</param>
        private static void ConfigureDevelopmentOptions(DbContextOptionsBuilder optionsBuilder, bool isDevelopment)
        {
            if (isDevelopment)
            {
                optionsBuilder
                    .EnableSensitiveDataLogging()
                    .EnableDetailedErrors()
                    .LogTo(Console.WriteLine, LogLevel.Information);
            }
        }
    }
}
