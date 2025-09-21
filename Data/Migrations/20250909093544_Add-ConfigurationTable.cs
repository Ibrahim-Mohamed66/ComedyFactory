using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class AddConfigurationTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Configurations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ArName = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    EnName = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Facebook = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    Twitter = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    Youtube = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    LinkedIn = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    Instagram = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    Tiktok = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    Snapchat = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    DefaultEmailAddress = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    DefaultEmailName = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    EmailSender = table.Column<string>(type: "text", nullable: true),
                    PasswordEmailSender = table.Column<string>(type: "text", nullable: true),
                    Port = table.Column<int>(type: "integer", nullable: true),
                    UseSSL = table.Column<bool>(type: "boolean", nullable: false),
                    Host = table.Column<string>(type: "text", nullable: true),
                    GoogleAnalytics = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    GoogleAnalyticsEmail = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    SEOScripts = table.Column<string>(type: "text", nullable: true),
                    Deleted = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedOnUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()"),
                    UpdatedOnUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DisplayOrder = table.Column<int>(type: "integer", nullable: true),
                    Hidden = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Configurations", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Configurations");
        }
    }
}
