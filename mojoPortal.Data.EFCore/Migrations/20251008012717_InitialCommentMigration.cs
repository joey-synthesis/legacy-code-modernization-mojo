using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace mojoPortal.Data.EFCore.Migrations
{
    /// <inheritdoc />
    public partial class InitialCommentMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "mp_Comments",
                columns: table => new
                {
                    Guid = table.Column<Guid>(type: "TEXT", nullable: false),
                    ParentGuid = table.Column<Guid>(type: "TEXT", nullable: false),
                    SiteGuid = table.Column<Guid>(type: "TEXT", nullable: false),
                    FeatureGuid = table.Column<Guid>(type: "TEXT", nullable: false),
                    ModuleGuid = table.Column<Guid>(type: "TEXT", nullable: false),
                    ContentGuid = table.Column<Guid>(type: "TEXT", nullable: false),
                    UserGuid = table.Column<Guid>(type: "TEXT", nullable: false),
                    Title = table.Column<string>(type: "TEXT", maxLength: 255, nullable: true),
                    UserComment = table.Column<string>(type: "TEXT", nullable: true),
                    UserName = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    UserEmail = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    UserUrl = table.Column<string>(type: "TEXT", maxLength: 255, nullable: true),
                    UserIp = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    CreatedUtc = table.Column<DateTime>(type: "TEXT", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    LastModUtc = table.Column<DateTime>(type: "TEXT", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    ModerationStatus = table.Column<byte>(type: "INTEGER", nullable: false, defaultValue: (byte)1),
                    ModeratedBy = table.Column<Guid>(type: "TEXT", nullable: true),
                    ModerationReason = table.Column<string>(type: "TEXT", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mp_Comments", x => x.Guid);
                    table.ForeignKey(
                        name: "FK_mp_Comments_mp_Comments_ParentGuid",
                        column: x => x.ParentGuid,
                        principalTable: "mp_Comments",
                        principalColumn: "Guid",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_mp_Comments_Content_Status_Date",
                table: "mp_Comments",
                columns: new[] { "ContentGuid", "ModerationStatus", "CreatedUtc" });

            migrationBuilder.CreateIndex(
                name: "IX_mp_Comments_ContentGuid",
                table: "mp_Comments",
                column: "ContentGuid");

            migrationBuilder.CreateIndex(
                name: "IX_mp_Comments_CreatedUtc",
                table: "mp_Comments",
                column: "CreatedUtc");

            migrationBuilder.CreateIndex(
                name: "IX_mp_Comments_Parent_Date",
                table: "mp_Comments",
                columns: new[] { "ParentGuid", "CreatedUtc" });

            migrationBuilder.CreateIndex(
                name: "IX_mp_Comments_ParentGuid",
                table: "mp_Comments",
                column: "ParentGuid");

            migrationBuilder.CreateIndex(
                name: "IX_mp_Comments_SiteGuid",
                table: "mp_Comments",
                column: "SiteGuid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "mp_Comments");
        }
    }
}
