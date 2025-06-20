using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TeamSlate.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BillableMasters",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Label = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BillableMasters", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DesignationMasters",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DesignationMasters", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SkillMasters",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SkillMasters", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Resources",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DesignationId = table.Column<int>(type: "int", nullable: false),
                    BillableId = table.Column<int>(type: "int", nullable: false),
                    Availability = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Resources", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Resources_BillableMasters_BillableId",
                        column: x => x.BillableId,
                        principalTable: "BillableMasters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Resources_DesignationMasters_DesignationId",
                        column: x => x.DesignationId,
                        principalTable: "DesignationMasters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ResourceSkills",
                columns: table => new
                {
                    ResourceId = table.Column<int>(type: "int", nullable: false),
                    SkillId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResourceSkills", x => new { x.ResourceId, x.SkillId });
                    table.ForeignKey(
                        name: "FK_ResourceSkills_Resources_ResourceId",
                        column: x => x.ResourceId,
                        principalTable: "Resources",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ResourceSkills_SkillMasters_SkillId",
                        column: x => x.SkillId,
                        principalTable: "SkillMasters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WeeklyHours",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WeekStartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Hours = table.Column<double>(type: "float", nullable: false),
                    ResourceId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WeeklyHours", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WeeklyHours_Resources_ResourceId",
                        column: x => x.ResourceId,
                        principalTable: "Resources",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "BillableMasters",
                columns: new[] { "Id", "Label" },
                values: new object[,]
                {
                    { 1, "Yes" },
                    { 2, "No" }
                });

            migrationBuilder.InsertData(
                table: "DesignationMasters",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Developer" },
                    { 2, "Tester" },
                    { 3, "Tech Lead" }
                });

            migrationBuilder.InsertData(
                table: "SkillMasters",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "C#" },
                    { 2, "SQL" },
                    { 3, "React" },
                    { 4, "Python" }
                });

            migrationBuilder.InsertData(
                table: "Resources",
                columns: new[] { "Id", "Availability", "BillableId", "DesignationId", "Name" },
                values: new object[,]
                {
                    { 1, "100%", 1, 1, "Khush Shah" },
                    { 2, "50%", 2, 2, "Tom Parks" },
                    { 3, "75%", 1, 1, "Charlie Brown" },
                    { 4, "100%", 1, 3, "Kendall Roy" },
                    { 5, "40%", 2, 3, "Tom Cruise" },
                    { 6, "90%", 1, 2, "Elon Musk" },
                    { 7, "60%", 1, 1, "George Clooney" }
                });

            migrationBuilder.InsertData(
                table: "ResourceSkills",
                columns: new[] { "ResourceId", "SkillId" },
                values: new object[,]
                {
                    { 1, 1 },
                    { 1, 2 },
                    { 2, 3 },
                    { 3, 1 },
                    { 4, 4 },
                    { 5, 4 },
                    { 6, 2 },
                    { 6, 4 },
                    { 7, 1 }
                });

            migrationBuilder.InsertData(
                table: "WeeklyHours",
                columns: new[] { "Id", "Hours", "ResourceId", "WeekStartDate" },
                values: new object[,]
                {
                    { 1, 40.0, 1, new DateTime(2025, 6, 10, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 2, 20.0, 2, new DateTime(2025, 6, 10, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 3, 30.0, 3, new DateTime(2025, 6, 10, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 4, 40.0, 4, new DateTime(2025, 6, 10, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 5, 16.0, 5, new DateTime(2025, 6, 10, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 6, 36.0, 6, new DateTime(2025, 6, 10, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 7, 24.0, 7, new DateTime(2025, 6, 10, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Resources_BillableId",
                table: "Resources",
                column: "BillableId");

            migrationBuilder.CreateIndex(
                name: "IX_Resources_DesignationId",
                table: "Resources",
                column: "DesignationId");

            migrationBuilder.CreateIndex(
                name: "IX_ResourceSkills_SkillId",
                table: "ResourceSkills",
                column: "SkillId");

            migrationBuilder.CreateIndex(
                name: "IX_WeeklyHours_ResourceId",
                table: "WeeklyHours",
                column: "ResourceId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ResourceSkills");

            migrationBuilder.DropTable(
                name: "WeeklyHours");

            migrationBuilder.DropTable(
                name: "SkillMasters");

            migrationBuilder.DropTable(
                name: "Resources");

            migrationBuilder.DropTable(
                name: "BillableMasters");

            migrationBuilder.DropTable(
                name: "DesignationMasters");
        }
    }
}
