using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BTL_WNC.Migrations
{
    /// <inheritdoc />
    public partial class initDbb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserIdsJson",
                table: "Tasks",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserIdsJson",
                table: "Tasks");
        }
    }
}
