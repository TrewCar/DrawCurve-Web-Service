using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace DrawCurve.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class EditPrimaryKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_VideoInfo",
                table: "VideoInfo");

            migrationBuilder.DropIndex(
                name: "IX_VideoInfo_RenderCnfId",
                table: "VideoInfo");

            migrationBuilder.DropColumn(
                name: "ID",
                table: "VideoInfo");

            migrationBuilder.AddPrimaryKey(
                name: "PK_VideoInfo",
                table: "VideoInfo",
                column: "RenderCnfId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_VideoInfo",
                table: "VideoInfo");

            migrationBuilder.AddColumn<int>(
                name: "ID",
                table: "VideoInfo",
                type: "integer",
                nullable: false,
                defaultValue: 0)
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_VideoInfo",
                table: "VideoInfo",
                column: "ID");

            migrationBuilder.CreateIndex(
                name: "IX_VideoInfo_RenderCnfId",
                table: "VideoInfo",
                column: "RenderCnfId");
        }
    }
}
