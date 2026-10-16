using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NFC.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddAdminIdToModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AdminId",
                table: "NfcTags",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "AdminId",
                table: "ChemicalItems",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AdminId",
                table: "NfcTags");

            migrationBuilder.DropColumn(
                name: "AdminId",
                table: "ChemicalItems");
        }
    }
}
