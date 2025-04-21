using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuanLiPhongTro.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "AspNetUserTokens",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(128)",
                oldMaxLength: 128);

            migrationBuilder.AlterColumn<string>(
                name: "LoginProvider",
                table: "AspNetUserTokens",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(128)",
                oldMaxLength: 128);

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

            migrationBuilder.AlterColumn<string>(
                name: "ProviderKey",
                table: "AspNetUserLogins",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(128)",
                oldMaxLength: 128);

            migrationBuilder.AlterColumn<string>(
                name: "LoginProvider",
                table: "AspNetUserLogins",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(128)",
                oldMaxLength: 128);

            migrationBuilder.CreateTable(
                name: "DichVu",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenDichVu = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DonGia = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DichVu", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NguoiThue",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CCCD = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SDT = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NguoiThue", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NguoiThue_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ToaNha",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenToa = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ToaNha", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Phong",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenPhong = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SoNguoiToiDa = table.Column<int>(type: "int", nullable: false),
                    GiaTien = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DaThue = table.Column<bool>(type: "bit", nullable: false),
                    ToaNhaId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Phong", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Phong_ToaNha_ToaNhaId",
                        column: x => x.ToaNhaId,
                        principalTable: "ToaNha",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HopDong",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NgayBatDau = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NgayKetThuc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TienCoc = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DaTra = table.Column<bool>(type: "bit", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PhongId = table.Column<int>(type: "int", nullable: false),
                    NguoiThueId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HopDong", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HopDong_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HopDong_NguoiThue_NguoiThueId",
                        column: x => x.NguoiThueId,
                        principalTable: "NguoiThue",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_HopDong_Phong_PhongId",
                        column: x => x.PhongId,
                        principalTable: "Phong",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SuCo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MoTa = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NgayBaoCao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DaGiaiQuyet = table.Column<bool>(type: "bit", nullable: false),
                    PhongId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SuCo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SuCo_Phong_PhongId",
                        column: x => x.PhongId,
                        principalTable: "Phong",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SuDungDichVu",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ChiSoCu = table.Column<int>(type: "int", nullable: false),
                    ChiSoMoi = table.Column<int>(type: "int", nullable: false),
                    ThangNam = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DichVuId = table.Column<int>(type: "int", nullable: false),
                    PhongId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SuDungDichVu", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SuDungDichVu_DichVu_DichVuId",
                        column: x => x.DichVuId,
                        principalTable: "DichVu",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SuDungDichVu_Phong_PhongId",
                        column: x => x.PhongId,
                        principalTable: "Phong",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ThanhToans",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NguoiThueId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    HopDongId = table.Column<int>(type: "int", nullable: true),
                    trangThaiThanhToan = table.Column<int>(type: "int", nullable: false),
                    NgayThanhToan = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TongTien = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ThanhToans", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ThanhToans_AspNetUsers_NguoiThueId",
                        column: x => x.NguoiThueId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ThanhToans_HopDong_HopDongId",
                        column: x => x.HopDongId,
                        principalTable: "HopDong",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "TraHopDong",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NgayTra = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LyDo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HopDongId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TraHopDong", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TraHopDong_HopDong_HopDongId",
                        column: x => x.HopDongId,
                        principalTable: "HopDong",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ChiTietThanhtoan",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ThanhToanId = table.Column<int>(type: "int", nullable: false),
                    Loai = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MoTa = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SoTien = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChiTietThanhtoan", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChiTietThanhtoan_ThanhToans_ThanhToanId",
                        column: x => x.ThanhToanId,
                        principalTable: "ThanhToans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HoaDon",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ThanhToanId = table.Column<int>(type: "int", nullable: false),
                    NgayLap = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NguoiLap = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GhiChu = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HopDongId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HoaDon", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HoaDon_HopDong_HopDongId",
                        column: x => x.HopDongId,
                        principalTable: "HopDong",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_HoaDon_ThanhToans_ThanhToanId",
                        column: x => x.ThanhToanId,
                        principalTable: "ThanhToans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietThanhtoan_ThanhToanId",
                table: "ChiTietThanhtoan",
                column: "ThanhToanId");

            migrationBuilder.CreateIndex(
                name: "IX_HoaDon_HopDongId",
                table: "HoaDon",
                column: "HopDongId");

            migrationBuilder.CreateIndex(
                name: "IX_HoaDon_ThanhToanId",
                table: "HoaDon",
                column: "ThanhToanId");

            migrationBuilder.CreateIndex(
                name: "IX_HopDong_NguoiThueId",
                table: "HopDong",
                column: "NguoiThueId");

            migrationBuilder.CreateIndex(
                name: "IX_HopDong_PhongId",
                table: "HopDong",
                column: "PhongId");

            migrationBuilder.CreateIndex(
                name: "IX_HopDong_UserId",
                table: "HopDong",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_NguoiThue_UserId",
                table: "NguoiThue",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Phong_ToaNhaId",
                table: "Phong",
                column: "ToaNhaId");

            migrationBuilder.CreateIndex(
                name: "IX_SuCo_PhongId",
                table: "SuCo",
                column: "PhongId");

            migrationBuilder.CreateIndex(
                name: "IX_SuDungDichVu_DichVuId",
                table: "SuDungDichVu",
                column: "DichVuId");

            migrationBuilder.CreateIndex(
                name: "IX_SuDungDichVu_PhongId",
                table: "SuDungDichVu",
                column: "PhongId");

            migrationBuilder.CreateIndex(
                name: "IX_ThanhToans_HopDongId",
                table: "ThanhToans",
                column: "HopDongId");

            migrationBuilder.CreateIndex(
                name: "IX_ThanhToans_NguoiThueId",
                table: "ThanhToans",
                column: "NguoiThueId");

            migrationBuilder.CreateIndex(
                name: "IX_TraHopDong_HopDongId",
                table: "TraHopDong",
                column: "HopDongId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChiTietThanhtoan");

            migrationBuilder.DropTable(
                name: "HoaDon");

            migrationBuilder.DropTable(
                name: "SuCo");

            migrationBuilder.DropTable(
                name: "SuDungDichVu");

            migrationBuilder.DropTable(
                name: "TraHopDong");

            migrationBuilder.DropTable(
                name: "ThanhToans");

            migrationBuilder.DropTable(
                name: "DichVu");

            migrationBuilder.DropTable(
                name: "HopDong");

            migrationBuilder.DropTable(
                name: "NguoiThue");

            migrationBuilder.DropTable(
                name: "Phong");

            migrationBuilder.DropTable(
                name: "ToaNha");

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
                name: "Name",
                table: "AspNetUserTokens",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "LoginProvider",
                table: "AspNetUserTokens",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "ProviderKey",
                table: "AspNetUserLogins",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "LoginProvider",
                table: "AspNetUserLogins",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }
    }
}
