using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BTL_WNC.Migrations
{
    /// <inheritdoc />
    public partial class initDbThi : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Decription",
                table: "Projects",
                type: "nvarchar(1000)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(1000)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Decription",
                table: "Projects",
                type: "varchar(1000)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(1000)");
        }
    }
}
