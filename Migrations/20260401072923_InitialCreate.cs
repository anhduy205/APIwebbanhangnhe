using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace APIwebbanhangnhe.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(240)", maxLength: 240, nullable: true),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PlayerCards",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: false),
                    PlayerName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Club = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Position = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    Rarity = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(600)", maxLength: 600, nullable: true),
                    ImageUrl = table.Column<string>(type: "nvarchar(260)", maxLength: 260, nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayerCards", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlayerCards_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PlayerCardImages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Url = table.Column<string>(type: "nvarchar(260)", maxLength: 260, nullable: false),
                    PlayerCardId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayerCardImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlayerCardImages_PlayerCards_PlayerCardId",
                        column: x => x.PlayerCardId,
                        principalTable: "PlayerCards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Description", "DisplayOrder", "Name" },
                values: new object[,]
                {
                    { 1, "Những thẻ cầu thủ nổi bật của đội tuyển Việt Nam.", 1, "National Team Stars" },
                    { 2, "Các thẻ được săn đón từ những ngôi sao V.League.", 2, "V.League Icons" },
                    { 3, "Phiên bản hiếm với thiết kế đặc biệt dành cho người sưu tầm.", 3, "Limited Edition" },
                    { 4, "Những gương mặt trẻ đầy triển vọng của bóng đá Việt Nam.", 4, "Rising Talents" }
                });

            migrationBuilder.InsertData(
                table: "PlayerCards",
                columns: new[] { "Id", "CategoryId", "Club", "CreatedAt", "Description", "ImageUrl", "PlayerName", "Position", "Price", "Rarity", "Title" },
                values: new object[,]
                {
                    { 1, 1, "Cong An Ha Noi", new DateTime(2026, 3, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Phiên bản thẻ phủ vàng lấy cảm hứng từ lối chơi sáng tạo và khả năng tạo đột biến của Quang Hai.", "/images/player-cards/players/quang-hai.svg", "Nguyen Quang Hai", "Midfielder", 790000m, "Legendary", "Golden Captain 2026" },
                    { 2, 2, "Becamex Binh Duong", new DateTime(2026, 3, 5, 0, 0, 0, 0, DateTimeKind.Utc), "Thẻ nổi bật với thông số ghi bàn ấn tượng, dành cho fan của các tiền đạo săn bàn.", "/images/player-cards/players/tien-linh.svg", "Nguyen Tien Linh", "Forward", 650000m, "Ultra Rare", "Shining Striker 2026" },
                    { 3, 1, "Phu Dong Ninh Binh", new DateTime(2026, 3, 8, 0, 0, 0, 0, DateTimeKind.Utc), "Phiên bản dành cho người thích kiểm soát tuyến giữa, thiết kế lấy tông đỏ vàng chủ đạo.", "/images/player-cards/players/hoang-duc.svg", "Nguyen Hoang Duc", "Central Midfielder", 560000m, "Collector", "Engine Room 2026" },
                    { 4, 4, "The Cong Viettel", new DateTime(2026, 3, 12, 0, 0, 0, 0, DateTimeKind.Utc), "Thiết kế trẻ trung, tốc độ cao, phù hợp cho bộ sưu tập những tài năng mới.", "/images/player-cards/players/van-khang.svg", "Khuat Van Khang", "Attacking Midfielder", 480000m, "Limited", "Future Flash 2026" }
                });

            migrationBuilder.InsertData(
                table: "PlayerCardImages",
                columns: new[] { "Id", "PlayerCardId", "Url" },
                values: new object[,]
                {
                    { 1, 1, "/images/player-cards/gallery/quang-hai-1.svg" },
                    { 2, 1, "/images/player-cards/gallery/quang-hai-2.svg" },
                    { 3, 2, "/images/player-cards/gallery/tien-linh-1.svg" },
                    { 4, 3, "/images/player-cards/gallery/hoang-duc-1.svg" },
                    { 5, 4, "/images/player-cards/gallery/van-khang-1.svg" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Categories_Name",
                table: "Categories",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PlayerCardImages_PlayerCardId",
                table: "PlayerCardImages",
                column: "PlayerCardId");

            migrationBuilder.CreateIndex(
                name: "IX_PlayerCards_CategoryId",
                table: "PlayerCards",
                column: "CategoryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PlayerCardImages");

            migrationBuilder.DropTable(
                name: "PlayerCards");

            migrationBuilder.DropTable(
                name: "Categories");
        }
    }
}
