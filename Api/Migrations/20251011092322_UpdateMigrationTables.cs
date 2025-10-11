using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Api.Migrations
{
    /// <inheritdoc />
    public partial class UpdateMigrationTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // migrationBuilder.DropIndex(
            //     name: "ix_discoveries_is_public_created_at",
            //     table: "discoveries");

            // migrationBuilder.DropIndex(
            //     name: "ix_discoveries_latitude_longitude",
            //     table: "discoveries");

            // migrationBuilder.DropIndex(
            //     name: "ix_discoveries_user_id_created_at",
            //     table: "discoveries");

            migrationBuilder.DropColumn(
                name: "api_result",
                table: "discoveries");

            migrationBuilder.DropColumn(
                name: "common_names",
                table: "discoveries");

            migrationBuilder.DropColumn(
                name: "created_at",
                table: "discoveries");

            migrationBuilder.DropColumn(
                name: "is_public",
                table: "discoveries");

            migrationBuilder.DropColumn(
                name: "latitude",
                table: "discoveries");

            migrationBuilder.DropColumn(
                name: "longitude",
                table: "discoveries");

            migrationBuilder.DropColumn(
                name: "taken_at",
                table: "discoveries");

            migrationBuilder.DropColumn(
                name: "updated_at",
                table: "discoveries");

            migrationBuilder.RenameColumn(
                name: "plant_name",
                table: "discoveries",
                newName: "common_name");

            migrationBuilder.RenameColumn(
                name: "api_provider",
                table: "discoveries",
                newName: "asset_url");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "discoveries",
                newName: "discovery_id");

            migrationBuilder.RenameIndex(
                name: "ix_discoveries_plant_name",
                table: "discoveries",
                newName: "ix_discoveries_common_name");

            migrationBuilder.AddColumn<Guid>(
                name: "reaction_id",
                table: "reactions",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "user_id1",
                table: "reactions",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "ix_reactions_user_id1",
                table: "reactions",
                column: "user_id1");

            migrationBuilder.CreateIndex(
                name: "ix_discoveries_asset_url",
                table: "discoveries",
                column: "asset_url");

            migrationBuilder.CreateIndex(
                name: "ix_discoveries_scientific_name",
                table: "discoveries",
                column: "scientific_name");

            migrationBuilder.CreateIndex(
                name: "ix_discoveries_user_id",
                table: "discoveries",
                column: "user_id");

            migrationBuilder.AddForeignKey(
                name: "fk_reactions_users_user_id1",
                table: "reactions",
                column: "user_id1",
                principalTable: "users",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_reactions_users_user_id1",
                table: "reactions");

            migrationBuilder.DropIndex(
                name: "ix_reactions_user_id1",
                table: "reactions");

            migrationBuilder.DropIndex(
                name: "ix_discoveries_asset_url",
                table: "discoveries");

            migrationBuilder.DropIndex(
                name: "ix_discoveries_scientific_name",
                table: "discoveries");

            migrationBuilder.DropIndex(
                name: "ix_discoveries_user_id",
                table: "discoveries");

            migrationBuilder.DropColumn(
                name: "reaction_id",
                table: "reactions");

            migrationBuilder.DropColumn(
                name: "user_id1",
                table: "reactions");

            migrationBuilder.RenameColumn(
                name: "common_name",
                table: "discoveries",
                newName: "plant_name");

            migrationBuilder.RenameColumn(
                name: "asset_url",
                table: "discoveries",
                newName: "api_provider");

            migrationBuilder.RenameColumn(
                name: "discovery_id",
                table: "discoveries",
                newName: "id");

            migrationBuilder.RenameIndex(
                name: "ix_discoveries_common_name",
                table: "discoveries",
                newName: "ix_discoveries_plant_name");

            migrationBuilder.AddColumn<string>(
                name: "api_result",
                table: "discoveries",
                type: "jsonb",
                nullable: true);

            migrationBuilder.AddColumn<List<string>>(
                name: "common_names",
                table: "discoveries",
                type: "text[]",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "created_at",
                table: "discoveries",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<bool>(
                name: "is_public",
                table: "discoveries",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<double>(
                name: "latitude",
                table: "discoveries",
                type: "double precision",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "longitude",
                table: "discoveries",
                type: "double precision",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "taken_at",
                table: "discoveries",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "updated_at",
                table: "discoveries",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.CreateIndex(
                name: "ix_discoveries_is_public_created_at",
                table: "discoveries",
                columns: new[] { "is_public", "created_at" });

            migrationBuilder.CreateIndex(
                name: "ix_discoveries_latitude_longitude",
                table: "discoveries",
                columns: new[] { "latitude", "longitude" });

            migrationBuilder.CreateIndex(
                name: "ix_discoveries_user_id_created_at",
                table: "discoveries",
                columns: new[] { "user_id", "created_at" });
        }
    }
}
