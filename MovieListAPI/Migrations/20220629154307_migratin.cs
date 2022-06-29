using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MovieListAPI.Migrations
{
    public partial class migratin : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<float>(
                name: "Rating",
                table: "Reviews",
                type: "real",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<float>(
                name: "Rating",
                table: "Movies",
                type: "real",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(1)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Rating",
                table: "Reviews",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(float),
                oldType: "real");

            migrationBuilder.AlterColumn<decimal>(
                name: "Rating",
                table: "Movies",
                type: "decimal(1)",
                nullable: false,
                oldClrType: typeof(float),
                oldType: "real");
        }
    }
}
