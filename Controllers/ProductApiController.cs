using APIwebbanhangnhe.Models;
using APIwebbanhangnhe.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace APIwebbanhangnhe.Controllers;

[ApiController]
[Route("api/products")]
[Route("api/ProductApi")]
public class ProductApiController : ControllerBase
{
    private readonly IProductRepository _productRepository;

    public ProductApiController(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    [HttpGet]
    public async Task<IActionResult> GetProducts()
    {
        try
        {
            var products = await _productRepository.GetProductsAsync();
            return Ok(products);
        }
        catch
        {
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetProductById(int id)
    {
        try
        {
            var product = await _productRepository.GetProductByIdAsync(id);
            if (product is null)
            {
                return NotFound();
            }

            return Ok(product);
        }
        catch
        {
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpPost]
    public async Task<IActionResult> AddProduct([FromBody] Product product)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return ValidationProblem(ModelState);
            }

            await _productRepository.AddProductAsync(product);
            return CreatedAtAction(nameof(GetProductById), new { id = product.Id }, product);
        }
        catch
        {
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateProduct(int id, [FromBody] Product product)
    {
        try
        {
            if (id != product.Id)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return ValidationProblem(ModelState);
            }

            var currentProduct = await _productRepository.GetProductByIdAsync(id);
            if (currentProduct is null)
            {
                return NotFound();
            }

            await _productRepository.UpdateProductAsync(product);
            return NoContent();
        }
        catch
        {
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        try
        {
            var currentProduct = await _productRepository.GetProductByIdAsync(id);
            if (currentProduct is null)
            {
                return NotFound();
            }

            await _productRepository.DeleteProductAsync(id);
            return NoContent();
        }
        catch
        {
            return StatusCode(500, "Internal server error");
        }
    }
}
