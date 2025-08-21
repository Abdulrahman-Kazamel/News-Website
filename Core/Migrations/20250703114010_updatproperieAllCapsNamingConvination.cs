using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NewsWebsite.Migrations
{
    /// <inheritdoc />
    public partial class updatproperieAllCapsNamingConvination : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_newsPosts_categories_CategoryID",
                table: "newsPosts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_teamMembers",
                table: "teamMembers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_newsPosts",
                table: "newsPosts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_contacts",
                table: "contacts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_categories",
                table: "categories");

            migrationBuilder.RenameTable(
                name: "teamMembers",
                newName: "TeamMembers");

            migrationBuilder.RenameTable(
                name: "newsPosts",
                newName: "NewsPosts");

            migrationBuilder.RenameTable(
                name: "contacts",
                newName: "Contacts");

            migrationBuilder.RenameTable(
                name: "categories",
                newName: "Categories");

            migrationBuilder.RenameColumn(
                name: "CategoryID",
                table: "NewsPosts",
                newName: "CategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_newsPosts_CategoryID",
                table: "NewsPosts",
                newName: "IX_NewsPosts_CategoryId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TeamMembers",
                table: "TeamMembers",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_NewsPosts",
                table: "NewsPosts",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Contacts",
                table: "Contacts",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Categories",
                table: "Categories",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_NewsPosts_Categories_CategoryId",
                table: "NewsPosts",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NewsPosts_Categories_CategoryId",
                table: "NewsPosts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TeamMembers",
                table: "TeamMembers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_NewsPosts",
                table: "NewsPosts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Contacts",
                table: "Contacts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Categories",
                table: "Categories");

            migrationBuilder.RenameTable(
                name: "TeamMembers",
                newName: "teamMembers");

            migrationBuilder.RenameTable(
                name: "NewsPosts",
                newName: "newsPosts");

            migrationBuilder.RenameTable(
                name: "Contacts",
                newName: "contacts");

            migrationBuilder.RenameTable(
                name: "Categories",
                newName: "categories");

            migrationBuilder.RenameColumn(
                name: "CategoryId",
                table: "newsPosts",
                newName: "CategoryID");

            migrationBuilder.RenameIndex(
                name: "IX_NewsPosts_CategoryId",
                table: "newsPosts",
                newName: "IX_newsPosts_CategoryID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_teamMembers",
                table: "teamMembers",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_newsPosts",
                table: "newsPosts",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_contacts",
                table: "contacts",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_categories",
                table: "categories",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_newsPosts_categories_CategoryID",
                table: "newsPosts",
                column: "CategoryID",
                principalTable: "categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
