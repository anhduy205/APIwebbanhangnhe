using System.ComponentModel.DataAnnotations;

namespace APIwebbanhangnhe.Models;

public class Product
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Product name is required.")]
    [StringLength(120)]
    public string Name { get; set; } = string.Empty;

    [Range(typeof(decimal), "0.01", "999999999")]
    public decimal Price { get; set; }

    [StringLength(600)]
    public string Description { get; set; } = string.Empty;
}
