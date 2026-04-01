using System.ComponentModel.DataAnnotations;

namespace APIwebbanhangnhe.Models;

public class PlayerCard
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

    [StringLength(260)]
    [Display(Name = "Ảnh đại diện")]
    public string ImageUrl { get; set; } = "/images/player-cards/default-card.svg";

    [Display(Name = "Danh mục")]
    public int CategoryId { get; set; }

    [Display(Name = "Ngày tạo")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public Category? Category { get; set; }

    public ICollection<PlayerCardImage> Images { get; set; } = new List<PlayerCardImage>();
}
