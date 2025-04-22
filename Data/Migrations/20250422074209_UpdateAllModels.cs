using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuanLiPhongTro.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateAllModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HopDong_NguoiThue_NguoiThueId",
                table: "HopDong");

            migrationBuilder.DropForeignKey(
                name: "FK_ThanhToans_AspNetUsers_NguoiThueId",
                table: "ThanhToans");

            migrationBuilder.DropPrimaryKey(
                name: "PK_NguoiThue",
                table: "NguoiThue");

            migrationBuilder.DropIndex(
                name: "IX_NguoiThue_UserId",
                table: "NguoiThue");

            migrationBuilder.DropIndex(
                name: "IX_HopDong_NguoiThueId",
                table: "HopDong");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "NguoiThue");

            migrationBuilder.DropColumn(
                name: "NguoiThueId",
                table: "HopDong");

            migrationBuilder.DropColumn(
                name: "DiaChi",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "HoTen",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "SoDienThoai",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<string>(
                name: "NguoiThueId",
                table: "ThanhToans",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "ThanhToans",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "NguoiThueUserId",
                table: "HopDong",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_NguoiThue",
                table: "NguoiThue",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_HopDong_NguoiThueUserId",
                table: "HopDong",
                column: "NguoiThueUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_HopDong_NguoiThue_NguoiThueUserId",
                table: "HopDong",
                column: "NguoiThueUserId",
                principalTable: "NguoiThue",
                principalColumn: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ThanhToans_AspNetUsers_NguoiThueId",
                table: "ThanhToans",
                column: "NguoiThueId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HopDong_NguoiThue_NguoiThueUserId",
                table: "HopDong");

            migrationBuilder.DropForeignKey(
                name: "FK_ThanhToans_AspNetUsers_NguoiThueId",
                table: "ThanhToans");

            migrationBuilder.DropPrimaryKey(
                name: "PK_NguoiThue",
                table: "NguoiThue");

            migrationBuilder.DropIndex(
                name: "IX_HopDong_NguoiThueUserId",
                table: "HopDong");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "ThanhToans");

            migrationBuilder.DropColumn(
                name: "NguoiThueUserId",
                table: "HopDong");

            migrationBuilder.AlterColumn<string>(
                name: "NguoiThueId",
                table: "ThanhToans",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "NguoiThue",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<int>(
                name: "NguoiThueId",
                table: "HopDong",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DiaChi",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "AspNetUsers",
                type: "nvarchar(21)",
                maxLength: 21,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "HoTen",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SoDienThoai",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_NguoiThue",
                table: "NguoiThue",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_NguoiThue_UserId",
                table: "NguoiThue",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_HopDong_NguoiThueId",
                table: "HopDong",
                column: "NguoiThueId");

            migrationBuilder.AddForeignKey(
                name: "FK_HopDong_NguoiThue_NguoiThueId",
                table: "HopDong",
                column: "NguoiThueId",
                principalTable: "NguoiThue",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ThanhToans_AspNetUsers_NguoiThueId",
                table: "ThanhToans",
                column: "NguoiThueId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
