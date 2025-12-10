using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Suspect.TaskPro.Infra.Persistence.Postgres.Commands.Migrations
{
    /// <inheritdoc />
    public partial class updateentities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AssignedResourceBusinessKey",
                table: "WorkItems");

            migrationBuilder.RenameColumn(
                name: "ResourceId",
                table: "WorkItemLogWork",
                newName: "ResourceBusinessKey");

            migrationBuilder.CreateTable(
                name: "WorkItemResourceAssignment",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    WorkItemBusinessKey = table.Column<Guid>(type: "uuid", nullable: false),
                    ResourceBusinessKey = table.Column<Guid>(type: "uuid", nullable: false),
                    AllocatedHours = table.Column<decimal>(type: "numeric", nullable: false),
                    AssignedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    RateType = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    CreatedByUserId = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    CreatedDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ModifiedByUserId = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    ModifiedDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    WorkItemId = table.Column<long>(type: "bigint", nullable: true),
                    BusinessKey = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkItemResourceAssignment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkItemResourceAssignment_WorkItems_WorkItemId",
                        column: x => x.WorkItemId,
                        principalTable: "WorkItems",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "WorkItemResourceMaterialUsage",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    WorkItemBusinessKey = table.Column<Guid>(type: "uuid", nullable: false),
                    ResourceBusinessKey = table.Column<Guid>(type: "uuid", nullable: false),
                    Quantity = table.Column<decimal>(type: "numeric", nullable: false),
                    UsageDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Unit = table.Column<int>(type: "integer", nullable: false),
                    CreatedByUserId = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    CreatedDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ModifiedByUserId = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    ModifiedDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    WorkItemId = table.Column<long>(type: "bigint", nullable: true),
                    BusinessKey = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkItemResourceMaterialUsage", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkItemResourceMaterialUsage_WorkItems_WorkItemId",
                        column: x => x.WorkItemId,
                        principalTable: "WorkItems",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_WorkItemResourceAssignment_WorkItemId",
                table: "WorkItemResourceAssignment",
                column: "WorkItemId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkItemResourceMaterialUsage_WorkItemId",
                table: "WorkItemResourceMaterialUsage",
                column: "WorkItemId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WorkItemResourceAssignment");

            migrationBuilder.DropTable(
                name: "WorkItemResourceMaterialUsage");

            migrationBuilder.RenameColumn(
                name: "ResourceBusinessKey",
                table: "WorkItemLogWork",
                newName: "ResourceId");

            migrationBuilder.AddColumn<Guid>(
                name: "AssignedResourceBusinessKey",
                table: "WorkItems",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }
    }
}
