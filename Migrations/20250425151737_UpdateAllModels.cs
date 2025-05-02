using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuanLiPhongTro.Migrations
{
    /// <inheritdoc />
    public partial class UpdateAllModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "ThangNam",
                table: "ThanhToans",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<string>(
                name: "PhongId",
                table: "SuCo",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<int>(
                name: "SuDungDichVuId",
                table: "ChiTietThanhToan",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SuCo_PhongId",
                table: "SuCo",
                column: "PhongId");

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietThanhToan_SuDungDichVuId",
                table: "ChiTietThanhToan",
                column: "SuDungDichVuId");

            migrationBuilder.AddForeignKey(
                name: "FK_ChiTietThanhToan_SuDungDichVu_SuDungDichVuId",
                table: "ChiTietThanhToan",
                column: "SuDungDichVuId",
                principalTable: "SuDungDichVu",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SuCo_Phong_PhongId",
                table: "SuCo",
                column: "PhongId",
                principalTable: "Phong",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChiTietThanhToan_SuDungDichVu_SuDungDichVuId",
                table: "ChiTietThanhToan");

            migrationBuilder.DropForeignKey(
                name: "FK_SuCo_Phong_PhongId",
                table: "SuCo");

            migrationBuilder.DropIndex(
                name: "IX_SuCo_PhongId",
                table: "SuCo");

            migrationBuilder.DropIndex(
                name: "IX_ChiTietThanhToan_SuDungDichVuId",
                table: "ChiTietThanhToan");

            migrationBuilder.DropColumn(
                name: "ThangNam",
                table: "ThanhToans");

            migrationBuilder.DropColumn(
                name: "SuDungDichVuId",
                table: "ChiTietThanhToan");

            migrationBuilder.AlterColumn<string>(
                name: "PhongId",
                table: "SuCo",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }
    }
}
