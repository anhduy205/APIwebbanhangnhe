using System.ComponentModel.DataAnnotations;

namespace APIwebbanhangnhe.Models;

public class Category
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Tên danh mục là bắt buộc.")]
    [StringLength(60)]
    [Display(Name = "Tên danh mục")]
    public string Name { get; set; } = string.Empty;

    [StringLength(240)]
    [Display(Name = "Mô tả ngắn")]
    public string? Description { get; set; }

    [Range(1, 99)]
    [Display(Name = "Thứ tự hiển thị")]
    public int DisplayOrder { get; set; } = 1;

    public ICollection<PlayerCard> PlayerCards { get; set; } = new List<PlayerCard>();
}
