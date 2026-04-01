using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace APIwebbanhangnhe.Models.ViewModels;

public class PlayerCardFormViewModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Tên bộ thẻ là bắt buộc.")]
    [StringLength(120)]
    [Display(Name = "Tên bộ thẻ")]
    public string Title { get; set; } = string.Empty;

    [Required(ErrorMessage = "Tên cầu thủ là bắt buộc.")]
    [StringLength(100)]
    [Display(Name = "Cầu thủ")]
    public string PlayerName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Tên câu lạc bộ là bắt buộc.")]
    [StringLength(100)]
    [Display(Name = "Câu lạc bộ")]
    public string Club { get; set; } = string.Empty;

    [Required(ErrorMessage = "Vị trí thi đấu là bắt buộc.")]
    [StringLength(60)]
    [Display(Name = "Vị trí")]
    public string Position { get; set; } = string.Empty;

    [Required(ErrorMessage = "Độ hiếm là bắt buộc.")]
    [StringLength(40)]
    [Display(Name = "Độ hiếm")]
    public string Rarity { get; set; } = string.Empty;

    [Range(typeof(decimal), "1000", "999999999")]
    [Display(Name = "Giá bán (VNĐ)")]
    public decimal Price { get; set; }

    [StringLength(600)]
    [Display(Name = "Mô tả")]
    public string? Description { get; set; }

    [Display(Name = "Danh mục")]
    public int CategoryId { get; set; }

    [Display(Name = "Tải ảnh đại diện")]
    public IFormFile? ThumbnailFile { get; set; }

    [Display(Name = "Hoặc nhập URL ảnh")]
    [StringLength(260)]
    public string? ImageUrl { get; set; }

    [Display(Name = "Ảnh gallery")]
    public List<IFormFile> GalleryFiles { get; set; } = new();

    public string? CurrentImageUrl { get; set; }

    public IReadOnlyCollection<PlayerCardImage> ExistingImages { get; set; } = Array.Empty<PlayerCardImage>();

    public IEnumerable<SelectListItem> Categories { get; set; } = Array.Empty<SelectListItem>();

    public PlayerCard ToEntity(string resolvedImageUrl)
    {
        return new PlayerCard
        {
            Title = Title.Trim(),
            PlayerName = PlayerName.Trim(),
            Club = Club.Trim(),
            Position = Position.Trim(),
            Rarity = Rarity.Trim(),
            Price = Price,
            Description = Description?.Trim(),
            CategoryId = CategoryId,
            ImageUrl = resolvedImageUrl,
            CreatedAt = DateTime.UtcNow
        };
    }

    public void ApplyToEntity(PlayerCard playerCard, string resolvedImageUrl)
    {
        playerCard.Title = Title.Trim();
        playerCard.PlayerName = PlayerName.Trim();
        playerCard.Club = Club.Trim();
        playerCard.Position = Position.Trim();
        playerCard.Rarity = Rarity.Trim();
        playerCard.Price = Price;
        playerCard.Description = Description?.Trim();
        playerCard.CategoryId = CategoryId;
        playerCard.ImageUrl = resolvedImageUrl;
    }

    public static PlayerCardFormViewModel FromEntity(
        PlayerCard playerCard,
        IEnumerable<SelectListItem> categories)
    {
        return new PlayerCardFormViewModel
        {
            Id = playerCard.Id,
            Title = playerCard.Title,
            PlayerName = playerCard.PlayerName,
            Club = playerCard.Club,
            Position = playerCard.Position,
            Rarity = playerCard.Rarity,
            Price = playerCard.Price,
            Description = playerCard.Description,
            CategoryId = playerCard.CategoryId,
            ImageUrl = playerCard.ImageUrl,
            CurrentImageUrl = playerCard.ImageUrl,
            ExistingImages = playerCard.Images.ToList(),
            Categories = categories.ToList()
        };
    }
}
