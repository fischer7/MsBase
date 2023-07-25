using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fischer.Audit.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Audit");

            migrationBuilder.CreateTable(
                name: "Audit",
                schema: "Audit",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: true),
                    AuditKind = table.Column<string>(type: "varchar(50)", nullable: false),
                    EntityId = table.Column<string>(type: "varchar(250)", nullable: true),
                    EntityName = table.Column<string>(type: "varchar(200)", nullable: false),
                    TableName = table.Column<string>(type: "varchar(200)", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Audit", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AuditProp",
                schema: "Audit",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    PropertyName = table.Column<string>(type: "varchar(200)", nullable: false),
                    OldValue = table.Column<string>(type: "text", nullable: true),
                    NewValue = table.Column<string>(type: "text", nullable: true),
                    IsPrimaryKey = table.Column<bool>(type: "boolean", nullable: true),
                    AuditId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditProp", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AuditProp_Audit_AuditId",
                        column: x => x.AuditId,
                        principalSchema: "Audit",
                        principalTable: "Audit",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_AuditProp_AuditId",
                schema: "Audit",
                table: "AuditProp",
                column: "AuditId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AuditProp",
                schema: "Audit");

            migrationBuilder.DropTable(
                name: "Audit",
                schema: "Audit");
        }
    }
}
