using Microsoft.AspNetCore.Mvc;

namespace ProductScanner.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly ProductService _productService;

        public ProductController(ProductService productService)
        {
            _productService = productService;
        }

        [HttpPost("scan/{barcode}")]
        public async Task<IActionResult> ScanProduct(string barcode)
        {
            try
            {
                await _productService.DecrementProductQuantityAsync(barcode);
                return Ok();
            }
            catch (Exception ex)
            {
                // Return a 400 error if quantity is less than or equal to 0
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        public IActionResult GetAllProducts()
        {
            // Get the list of products from the static repository
            var products = ProductRepository.GetAllProducts();

            if (products == null || products.Count == 0)
            {
                return NotFound("No products found.");
            }

            return Ok(products);
        }
    }

}
