using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OrganizerBlazorApp.Infrastructure.Migrations;

/// <inheritdoc />
public partial class InitialCreate : Migration
{
  /// <inheritdoc />
  protected override void Up(MigrationBuilder migrationBuilder)
  {
    migrationBuilder.CreateTable(
        name: "TodoUnits",
        columns: table => new
        {
          Id = table.Column<Guid>(type: "TEXT", nullable: false),
          Title = table.Column<string>(type: "TEXT", nullable: false),
          Description = table.Column<string>(type: "TEXT", nullable: true),
          Deadline = table.Column<DateTime>(type: "TEXT", nullable: false),
          Status = table.Column<int>(type: "INTEGER", nullable: false),
          IsRequired = table.Column<bool>(type: "INTEGER", nullable: false),
          ParentUnitId = table.Column<Guid>(type: "TEXT", nullable: true),
          CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
          UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: true)
        },
        constraints: table =>
        {
          table.PrimaryKey("PK_TodoUnits", x => x.Id);
          table.ForeignKey(
                    name: "FK_TodoUnits_TodoUnits_ParentUnitId",
                    column: x => x.ParentUnitId,
                    principalTable: "TodoUnits",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
        });

    migrationBuilder.CreateTable(
        name: "Attachments",
        columns: table => new
        {
          Id = table.Column<Guid>(type: "TEXT", nullable: false),
          FileName = table.Column<string>(type: "TEXT", nullable: false),
          FilePath = table.Column<string>(type: "TEXT", nullable: false),
          Extension = table.Column<string>(type: "TEXT", nullable: false),
          Type = table.Column<int>(type: "INTEGER", nullable: false),
          ParentUnitId = table.Column<Guid>(type: "TEXT", nullable: false),
          CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
          UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: true)
        },
        constraints: table =>
        {
          table.PrimaryKey("PK_Attachments", x => x.Id);
          table.ForeignKey(
                    name: "FK_Attachments_TodoUnits_ParentUnitId",
                    column: x => x.ParentUnitId,
                    principalTable: "TodoUnits",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
        });

    migrationBuilder.CreateIndex(
        name: "IX_Attachments_ParentUnitId",
        table: "Attachments",
        column: "ParentUnitId");

    migrationBuilder.CreateIndex(
        name: "IX_TodoUnits_ParentUnitId",
        table: "TodoUnits",
        column: "ParentUnitId");
  }

  /// <inheritdoc />
  protected override void Down(MigrationBuilder migrationBuilder)
  {
    migrationBuilder.DropTable(
        name: "Attachments");

    migrationBuilder.DropTable(
        name: "TodoUnits");
  }
}
