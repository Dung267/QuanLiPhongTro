using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuanLiPhongTro.Migrations
{
    /// <inheritdoc />
    public partial class NewMigration : Migration
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

            migrationBuilder.AddColumn<int>(
                name: "SuDungDichVuId",
                table: "ChiTietThanhToan",
                type: "int",
                nullable: true);

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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChiTietThanhToan_SuDungDichVu_SuDungDichVuId",
                table: "ChiTietThanhToan");

            migrationBuilder.DropIndex(
                name: "IX_ChiTietThanhToan_SuDungDichVuId",
                table: "ChiTietThanhToan");

            migrationBuilder.DropColumn(
                name: "ThangNam",
                table: "ThanhToans");

            migrationBuilder.DropColumn(
                name: "SuDungDichVuId",
                table: "ChiTietThanhToan");
        }
    }
}
