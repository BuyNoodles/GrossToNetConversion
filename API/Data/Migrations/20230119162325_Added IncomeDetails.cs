using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedIncomeDetails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "employees",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(45)", maxLength: 45, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(45)", maxLength: 45, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GrossIncome = table.Column<decimal>(type: "decimal(9,2)", precision: 9, scale: 2, nullable: false),
                    WorkPosition = table.Column<string>(type: "nvarchar(45)", maxLength: 45, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "IncomeDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeId = table.Column<int>(type: "int", nullable: false),
                    Tax = table.Column<decimal>(type: "decimal(8,2)", nullable: false),
                    PIO = table.Column<decimal>(type: "decimal(8,2)", nullable: false),
                    HealthCare = table.Column<decimal>(type: "decimal(8,2)", nullable: false),
                    Unemployment = table.Column<decimal>(type: "decimal(8,2)", nullable: false),
                    NetIncome = table.Column<decimal>(type: "decimal(8,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IncomeDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IncomeDetails_employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "Id_UNIQUE",
                table: "employees",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_IncomeDetails_EmployeeId",
                table: "IncomeDetails",
                column: "EmployeeId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "IncomeDetails");

            migrationBuilder.DropTable(
                name: "employees");
        }
    }
}
