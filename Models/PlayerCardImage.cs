using System.ComponentModel.DataAnnotations;

namespace APIwebbanhangnhe.Models;

public class PlayerCardImage
{
    public int Id { get; set; }

    [Required]
    [StringLength(260)]
    [Display(Name = "Ảnh bổ sung")]
    public string Url { get; set; } = string.Empty;

    public int PlayerCardId { get; set; }

    public PlayerCard? PlayerCard { get; set; }
}
