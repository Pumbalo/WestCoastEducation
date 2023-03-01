using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace westcoasteducation.api.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedCompetence : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Teachers_Competence_CompetenceModelId",
                table: "Teachers");

            migrationBuilder.DropIndex(
                name: "IX_Teachers_CompetenceModelId",
                table: "Teachers");

            migrationBuilder.DropColumn(
                name: "CompetenceModelId",
                table: "Teachers");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "Competence",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER")
                .OldAnnotation("Sqlite:Autoincrement", true);

            migrationBuilder.CreateTable(
                name: "CompetenceModelTeacherModel",
                columns: table => new
                {
                    CompetenceId = table.Column<Guid>(type: "TEXT", nullable: false),
                    TeacherId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompetenceModelTeacherModel", x => new { x.CompetenceId, x.TeacherId });
                    table.ForeignKey(
                        name: "FK_CompetenceModelTeacherModel_Competence_CompetenceId",
                        column: x => x.CompetenceId,
                        principalTable: "Competence",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CompetenceModelTeacherModel_Teachers_TeacherId",
                        column: x => x.TeacherId,
                        principalTable: "Teachers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CompetenceModelTeacherModel_TeacherId",
                table: "CompetenceModelTeacherModel",
                column: "TeacherId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CompetenceModelTeacherModel");

            migrationBuilder.AddColumn<int>(
                name: "CompetenceModelId",
                table: "Teachers",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Competence",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "TEXT")
                .Annotation("Sqlite:Autoincrement", true);

            migrationBuilder.CreateIndex(
                name: "IX_Teachers_CompetenceModelId",
                table: "Teachers",
                column: "CompetenceModelId");

            migrationBuilder.AddForeignKey(
                name: "FK_Teachers_Competence_CompetenceModelId",
                table: "Teachers",
                column: "CompetenceModelId",
                principalTable: "Competence",
                principalColumn: "Id");
        }
    }
}
