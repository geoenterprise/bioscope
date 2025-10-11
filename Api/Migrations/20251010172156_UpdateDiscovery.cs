using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Api.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDiscovery : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // migrationBuilder.DropIndex(
            //     name: "ix_discoveries_top_match_name",
            //     table: "discoveries");

            migrationBuilder.RenameColumn(
                name: "top_match_name",
                table: "discoveries",
                newName: "wiki_description");

            migrationBuilder.AddColumn<string>(
                name: "plant_name",
                table: "discoveries",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "scientific_name",
                table: "discoveries",
                type: "text",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "ix_discoveries_plant_name",
                table: "discoveries",
                column: "plant_name");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_discoveries_plant_name",
                table: "discoveries");

            migrationBuilder.DropColumn(
                name: "plant_name",
                table: "discoveries");

            migrationBuilder.DropColumn(
                name: "scientific_name",
                table: "discoveries");

            migrationBuilder.RenameColumn(
                name: "wiki_description",
                table: "discoveries",
                newName: "top_match_name");

            migrationBuilder.CreateIndex(
                name: "ix_discoveries_top_match_name",
                table: "discoveries",
                column: "top_match_name");
        }
    }
}
