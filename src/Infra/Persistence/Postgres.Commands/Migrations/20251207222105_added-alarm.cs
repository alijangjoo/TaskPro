using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Suspect.TaskPro.Infra.Persistence.Postgres.Commands.Migrations
{
    /// <inheritdoc />
    public partial class addedalarm : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "AlarmTime",
                table: "WorkItems",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "HasAlarm",
                table: "WorkItems",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AlarmTime",
                table: "WorkItems");

            migrationBuilder.DropColumn(
                name: "HasAlarm",
                table: "WorkItems");
        }
    }
}
