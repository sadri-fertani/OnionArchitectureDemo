using Application.DTOs;
using Application.Interfaces.Authentication;
using Application.Interfaces.Services;
using Application.POCOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly ILogger<ProductController> _logger;
        private readonly IProductService _productService;

        public ProductController(
            ILogger<ProductController> logger,
            IProductService productService)
        {
            _logger = logger;
            _productService = productService;
        }

        [AllowAnonymous]
        [ActionName("GetOneAsync")]
        [HttpGet("{id:long}")]
        public async Task<ActionResult<ProductDto>> GetOneAsync(long id)
        {
            try
            {
                var product = await _productService.GetProduct(id);
                if (product == null) return NotFound($"Produit ayant l'id: {id} est introuvable.");

                return product;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error product-get-one {id}");

                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [Authorize(Roles = IRoleProvider.BASIC_USER + "," + IRoleProvider.ADMIN)]
        public async Task<ActionResult<List<ProductDto>>> GetAll()
        {
            try
            {
                var products = await _productService.GetProducts();
                if (products == null) return NotFound($"Aucun produit trouvé.");

                return products.ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error product-get-all");

                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [Authorize(Roles = IRoleProvider.ADMIN)]
        [HttpPost()]
        public async Task<ActionResult<ProductDto>> Post(ProductPoco model)
        {
            try
            {
                var newModel = await _productService.InsertProduct(model.Data);

                return CreatedAtAction(nameof(GetOneAsync), new { id = newModel.Id }, newModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error product-add");

                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [Authorize(Roles = IRoleProvider.ADMIN)]
        [HttpPut()]
        public async Task<IActionResult> Put(ProductDto model)
        {
            try
            {
                var product = await _productService.GetProduct(model.Id);
                if (product == null) return NotFound($"Le produit ayant l'id: {model.Id} est introuvable.");

                await _productService.UpdateProduct(model);

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error product-update {model.Id}");

                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [Authorize(Roles = IRoleProvider.ADMIN)]
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var product = await _productService.GetProduct(id);
                if (product == null) return StatusCode(StatusCodes.Status204NoContent);

                await _productService.DeleteProduct(id);

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error product-delete {id}");

                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
