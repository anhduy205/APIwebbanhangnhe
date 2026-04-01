using APIwebbanhangnhe.Models;
using Microsoft.EntityFrameworkCore;

namespace APIwebbanhangnhe.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Category> Categories => Set<Category>();

    public DbSet<PlayerCard> PlayerCards => Set<PlayerCard>();

    public DbSet<PlayerCardImage> PlayerCardImages => Set<PlayerCardImage>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Category>()
            .HasIndex(category => category.Name)
            .IsUnique();

        modelBuilder.Entity<PlayerCard>()
            .Property(playerCard => playerCard.Price)
            .HasColumnType("decimal(18,2)");

        modelBuilder.Entity<PlayerCard>()
            .HasOne(playerCard => playerCard.Category)
            .WithMany(category => category.PlayerCards)
            .HasForeignKey(playerCard => playerCard.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<PlayerCardImage>()
            .HasOne(image => image.PlayerCard)
            .WithMany(playerCard => playerCard.Images)
            .HasForeignKey(image => image.PlayerCardId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Category>().HasData(
            new Category
            {
                Id = 1,
                Name = "National Team Stars",
                Description = "Những thẻ cầu thủ nổi bật của đội tuyển Việt Nam.",
                DisplayOrder = 1
            },
            new Category
            {
                Id = 2,
                Name = "V.League Icons",
                Description = "Các thẻ được săn đón từ những ngôi sao V.League.",
                DisplayOrder = 2
            },
            new Category
            {
                Id = 3,
                Name = "Limited Edition",
                Description = "Phiên bản hiếm với thiết kế đặc biệt dành cho người sưu tầm.",
                DisplayOrder = 3
            },
            new Category
            {
                Id = 4,
                Name = "Rising Talents",
                Description = "Những gương mặt trẻ đầy triển vọng của bóng đá Việt Nam.",
                DisplayOrder = 4
            });

        modelBuilder.Entity<PlayerCard>().HasData(
            new PlayerCard
            {
                Id = 1,
                Title = "Golden Captain 2026",
                PlayerName = "Nguyen Quang Hai",
                Club = "Cong An Ha Noi",
                Position = "Midfielder",
                Rarity = "Legendary",
                Price = 790000,
                Description = "Phiên bản thẻ phủ vàng lấy cảm hứng từ lối chơi sáng tạo và khả năng tạo đột biến của Quang Hai.",
                ImageUrl = "/images/player-cards/players/quang-hai.svg",
                CategoryId = 1,
                CreatedAt = new DateTime(2026, 3, 1, 0, 0, 0, DateTimeKind.Utc)
            },
            new PlayerCard
            {
                Id = 2,
                Title = "Shining Striker 2026",
                PlayerName = "Nguyen Tien Linh",
                Club = "Becamex Binh Duong",
                Position = "Forward",
                Rarity = "Ultra Rare",
                Price = 650000,
                Description = "Thẻ nổi bật với thông số ghi bàn ấn tượng, dành cho fan của các tiền đạo săn bàn.",
                ImageUrl = "/images/player-cards/players/tien-linh.svg",
                CategoryId = 2,
                CreatedAt = new DateTime(2026, 3, 5, 0, 0, 0, DateTimeKind.Utc)
            },
            new PlayerCard
            {
                Id = 3,
                Title = "Engine Room 2026",
                PlayerName = "Nguyen Hoang Duc",
                Club = "Phu Dong Ninh Binh",
                Position = "Central Midfielder",
                Rarity = "Collector",
                Price = 560000,
                Description = "Phiên bản dành cho người thích kiểm soát tuyến giữa, thiết kế lấy tông đỏ vàng chủ đạo.",
                ImageUrl = "/images/player-cards/players/hoang-duc.svg",
                CategoryId = 1,
                CreatedAt = new DateTime(2026, 3, 8, 0, 0, 0, DateTimeKind.Utc)
            },
            new PlayerCard
            {
                Id = 4,
                Title = "Future Flash 2026",
                PlayerName = "Khuat Van Khang",
                Club = "The Cong Viettel",
                Position = "Attacking Midfielder",
                Rarity = "Limited",
                Price = 480000,
                Description = "Thiết kế trẻ trung, tốc độ cao, phù hợp cho bộ sưu tập những tài năng mới.",
                ImageUrl = "/images/player-cards/players/van-khang.svg",
                CategoryId = 4,
                CreatedAt = new DateTime(2026, 3, 12, 0, 0, 0, DateTimeKind.Utc)
            });

        modelBuilder.Entity<PlayerCardImage>().HasData(
            new PlayerCardImage
            {
                Id = 1,
                PlayerCardId = 1,
                Url = "/images/player-cards/gallery/quang-hai-1.svg"
            },
            new PlayerCardImage
            {
                Id = 2,
                PlayerCardId = 1,
                Url = "/images/player-cards/gallery/quang-hai-2.svg"
            },
            new PlayerCardImage
            {
                Id = 3,
                PlayerCardId = 2,
                Url = "/images/player-cards/gallery/tien-linh-1.svg"
            },
            new PlayerCardImage
            {
                Id = 4,
                PlayerCardId = 3,
                Url = "/images/player-cards/gallery/hoang-duc-1.svg"
            },
            new PlayerCardImage
            {
                Id = 5,
                PlayerCardId = 4,
                Url = "/images/player-cards/gallery/van-khang-1.svg"
            });
    }
}
