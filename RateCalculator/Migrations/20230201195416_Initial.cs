using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RateCalculator.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TaxSchedules",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Municipality = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TaxDateStart = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TaxDateEnd = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TaxRate = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaxSchedules", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TaxSchedules");
        }
    }
}
