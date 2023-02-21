using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UNCPath.AccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class addedrootFolderoptiontofolders : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FolderId",
                table: "Folders");

            migrationBuilder.AddColumn<bool>(
                name: "RootFolder",
                table: "Folders",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RootFolder",
                table: "Folders");

            migrationBuilder.AddColumn<int>(
                name: "FolderId",
                table: "Folders",
                type: "int",
                nullable: true);
        }
    }
}
