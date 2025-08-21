using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NewsWebsite.Migrations
{
    /// <inheritdoc />
    public partial class categoreIcons : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TitleIcon",
                table: "categories",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TitleIcon",
                table: "categories");
        }
    }
}
