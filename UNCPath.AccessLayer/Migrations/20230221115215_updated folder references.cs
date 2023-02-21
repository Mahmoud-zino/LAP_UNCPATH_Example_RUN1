using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UNCPath.AccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class updatedfolderreferences : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Folders_Folders_FolderId",
                table: "Folders");

            migrationBuilder.DropIndex(
                name: "IX_Folders_FolderId",
                table: "Folders");

            migrationBuilder.CreateTable(
                name: "FolderFolder",
                columns: table => new
                {
                    FolderId = table.Column<int>(type: "int", nullable: false),
                    FoldersId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FolderFolder", x => new { x.FolderId, x.FoldersId });
                    table.ForeignKey(
                        name: "FK_FolderFolder_Folders_FolderId",
                        column: x => x.FolderId,
                        principalTable: "Folders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FolderFolder_Folders_FoldersId",
                        column: x => x.FoldersId,
                        principalTable: "Folders",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_FolderFolder_FoldersId",
                table: "FolderFolder",
                column: "FoldersId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FolderFolder");

            migrationBuilder.CreateIndex(
                name: "IX_Folders_FolderId",
                table: "Folders",
                column: "FolderId");

            migrationBuilder.AddForeignKey(
                name: "FK_Folders_Folders_FolderId",
                table: "Folders",
                column: "FolderId",
                principalTable: "Folders",
                principalColumn: "Id");
        }
    }
}
