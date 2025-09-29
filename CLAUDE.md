# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

mojoPortal is a .NET Framework 4.8.1 CMS and web application framework written in C# ASP.NET. It supports multiple databases (MSSQL, MySQL, PostgreSQL, SQLite) through separate data layer projects.

## Build Commands

**Primary Build:**
```bash
# Build the full solution with all features (.NET 4.8.1)
msbuild mojoportal.sln -p:Configuration=Release

# Build the core solution with minimal features
msbuild mojoportal-core.sln -p:Configuration=Release
```

**Development Build:**
- Open solution in Visual Studio 2017 or later
- Choose "Rebuild Solution" from Build menu
- Select appropriate build profile for your database:
  - Debug/Release: MS SQL Server
  - Debug - MySql/Release - MySql: MySQL
  - Similar patterns for PostgreSQL and SQLite

**Custom Build Tool:**
```bash
# Build with version update using custom MSBuild project
msbuild Build.proj -p:Version=2.9.0.1
```

## Architecture

### Solution Structure
- **mojoportal.sln**: Complete solution with all features and database providers
- **mojoportal-core.sln**: Minimal CMS core without optional features
- **Web/**: Main web application project (startup project)
- **mojoPortal.Core/**: Core business logic and utilities
- **mojoPortal.Business/**: Business layer with domain models
- **mojoPortal.Data.*/**: Database-specific data access layers (MSSQL, MySQL, PostgreSQL, SQLite)
- **mojoPortal.Features.*/**: Feature modules (blog, forum, gallery, etc.)
- **mojoPortal.Web.*/**: Web-specific controls and frameworks
- **Plugins/**: Extended feature plugins (SuperFlexi, HtmlInclude)

### Multi-Database Support
The codebase uses a provider pattern for database abstraction:
- Each database has its own data layer project
- Build configuration determines which data provider is used
- Connection strings are configured per database type in user.config

### Feature Module Architecture
- Features are implemented as separate projects
- Feature files are copied to main Web project via post-build events
- All features must be deployed through the main mojoPortal.Web project
- Individual feature projects should not be run directly

## Development Setup

1. **Database Configuration**: Set up database according to mojoportal.com/docs/database-configuration
2. **Connection String**: Add database connection string to user.config
3. **Build Profile**: Select build profile matching your database (Debug/Release for MSSQL, Debug - MySql for MySQL, etc.)
4. **Rebuild**: Rebuild entire solution
5. **Startup Project**: Ensure mojoPortal.Web is set as startup project
6. **Initial Setup**: Navigate to /Setup/Default.aspx to complete database setup

## Key Files
- **user.config**: Database connection strings and environment settings
- **Web.config**: Main web application configuration
- **CommonAssemblyInfo.cs**: Shared assembly version information
- **Directory.Build.props**: MSBuild properties for all projects

## Visual Studio Configuration
- **Startup Project**: Always use mojoPortal.Web as the startup project
- **Multiple Web Servers**: Disable extra web servers for feature projects to avoid confusion
- **IIS Debugging**: Run Visual Studio as Administrator when debugging with IIS
- **Framework Version**: Requires .NET Framework 4.8.1 and Visual Studio 2017+

## Important Notes
- Never modify mojoPortal source code directly - use custom projects for customizations
- Feature projects spawn additional web servers in debug mode but should not be run independently
- All debugging and execution must go through the main Web project
- Database provider is determined by build configuration, not runtime switching