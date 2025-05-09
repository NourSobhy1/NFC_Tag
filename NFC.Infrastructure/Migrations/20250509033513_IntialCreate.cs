using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NFC.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class IntialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ChemicalItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    TradeName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ChemicalFormula = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Casnumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Manufacturer = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StorageInstruction = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Disposalmethod = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BoilingPoint = table.Column<decimal>(type: "decimal(10,4)", precision: 10, scale: 4, nullable: false),
                    MeltingPoint = table.Column<decimal>(type: "decimal(10,4)", precision: 10, scale: 4, nullable: false),
                    Density = table.Column<decimal>(type: "decimal(10,4)", precision: 10, scale: 4, nullable: false),
                    SpecialWarning = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Solubility = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChemicalItems", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NfcTags",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ChemicalItemId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NfcTags", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NfcTags_ChemicalItems_ChemicalItemId",
                        column: x => x.ChemicalItemId,
                        principalTable: "ChemicalItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "SafetyInstructions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Instruction = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ChemicalItemId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SafetyInstructions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SafetyInstructions_ChemicalItems_ChemicalItemId",
                        column: x => x.ChemicalItemId,
                        principalTable: "ChemicalItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_NfcTags_ChemicalItemId",
                table: "NfcTags",
                column: "ChemicalItemId",
                unique: true,
                filter: "[ChemicalItemId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_SafetyInstructions_ChemicalItemId",
                table: "SafetyInstructions",
                column: "ChemicalItemId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NfcTags");

            migrationBuilder.DropTable(
                name: "SafetyInstructions");

            migrationBuilder.DropTable(
                name: "ChemicalItems");
        }
    }
}
