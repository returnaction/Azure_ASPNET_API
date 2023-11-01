using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductsReviewsWebAPI.Data;
using ProductsReviewsWebAPI.DTOs;
using ProductsReviewsWebAPI.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ProductsReviewsWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public ProductController(ApplicationDbContext context)
        {
            _context = context;
        }
        // GET: api/<ProductController>
        [HttpGet]
        public IActionResult Get([FromQuery] int? maxPrice)
        {

            var products = _context.Products.Include(p => p.Reviews).Select(p => new ProductDTO
            {
                Id = p.Id,
                Name = p.Name,
                Price = p.Price,
                Reviews = p.Reviews.Select(r => new ReviewDTO
                {
                    Id = r.Id,
                    Rating = r.Rating,
                    Text = r.Text,
                }).ToList()
            });           
            if (maxPrice != null)
            {
                products = products.Where(p => p.Price < maxPrice);
            }
       
            return Ok(products);
        }

        // GET api/<ProductController>/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var product = _context.Products.Include(p => p.Reviews).Select(p => new ProductDTO
            {
                Id = p.Id,
                Name = p.Name,
                Price = p.Price,
                AverageRating = p.Reviews != null ? p.Reviews.Average(r => r.Rating) : 0,
                Reviews = p.Reviews != null ? p.Reviews.Select(r => new ReviewDTO
                {
                    Id = r.Id,
                    Rating = r.Rating,
                    Text = r.Text,
                }).ToList() : new List<ReviewDTO>()
            }).SingleOrDefault(p => p.Id == id);
            if (product == null)
            {
                return NotFound();
            }
          
            return Ok(product);
        }

        // POST api/<ProductController>
        [HttpPost]
        public IActionResult Post([FromBody] Product product)
        {
            if (ModelState.IsValid)
            {
                _context.Products.Add(product);
                _context.SaveChanges();

                return StatusCode(201, product);
            }
            return BadRequest();  
            
        }

        // PUT api/<ProductController>/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Product product)
        {
            var productToUpdate = _context.Products.Include(p => p.Reviews).SingleOrDefault(p => p.Id == id);
            if (productToUpdate == null)
            {
                return NotFound();
            }
            productToUpdate.Name = product.Name;
            productToUpdate.Price = product.Price;
            _context.Products.Update(productToUpdate);
            _context.SaveChanges();
            var productDTO = new ProductDTO
            {
                Id = productToUpdate.Id,
                Name = productToUpdate.Name,
                Price = productToUpdate.Price,
                Reviews = productToUpdate.Reviews.Select(r => new ReviewDTO
                {
                    Id = r.Id,
                    Rating = r.Rating,
                    Text = r.Text,
                }).ToList()
            };
            return Ok(productDTO);
        }

        // DELETE api/<ProductController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var product = _context.Products.Find(id);
            if (product == null)
            {
                return NotFound();
            }
            _context.Products.Remove(product);
            _context.SaveChanges();
            return NoContent();
        }
    }
}
