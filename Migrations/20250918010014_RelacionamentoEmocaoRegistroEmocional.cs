using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VibeCheckAPI_Dotnet8.Migrations
{
    /// <inheritdoc />
    public partial class RelacionamentoEmocaoRegistroEmocional : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RegistrosEmocionais_Emocoes_EmocaoId",
                table: "RegistrosEmocionais");

            migrationBuilder.CreateIndex(
                name: "IX_Emocoes_ValorNumerico",
                table: "Emocoes",
                column: "ValorNumerico",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_RegistrosEmocionais_Emocoes_EmocaoId",
                table: "RegistrosEmocionais",
                column: "EmocaoId",
                principalTable: "Emocoes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RegistrosEmocionais_Emocoes_EmocaoId",
                table: "RegistrosEmocionais");

            migrationBuilder.DropIndex(
                name: "IX_Emocoes_ValorNumerico",
                table: "Emocoes");

            migrationBuilder.AddForeignKey(
                name: "FK_RegistrosEmocionais_Emocoes_EmocaoId",
                table: "RegistrosEmocionais",
                column: "EmocaoId",
                principalTable: "Emocoes",
                principalColumn: "Id");
        }
    }
}
