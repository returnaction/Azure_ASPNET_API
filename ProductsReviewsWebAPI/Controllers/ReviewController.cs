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
    public class ReviewController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public ReviewController(ApplicationDbContext context)
        {
            _context = context;
        }
        // GET: api/<ReviewController>
        [HttpGet]
        public IActionResult Get()
        {
            var reviews = _context.Reviews.Select(r => new ReviewDTO
            {
                Id = r.Id,
                Text = r.Text,
                Rating = r.Rating,
            }).ToList();
            return Ok(reviews);
        }

        // GET api/<ReviewController>/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var review = _context.Reviews.Find(id);

            if (review == null)
            {
                return NotFound();
            }            
            var reviewDTO = new ReviewDTO
            {
                Id = id,
                Text = review.Text,
                Rating = review.Rating,
            };
            return Ok(reviewDTO);
        }

        // POST api/<ReviewController>
        [HttpPost]
        public IActionResult Post([FromBody] Review review)
        {
            var product = _context.Products.Find(review.ProductId);
            if (product == null)
            {
                return BadRequest("Invalid Product ID");
            }
                review.Product = product;
                _context.Reviews.Add(review);
                _context.SaveChanges();
            var reviewDTO = new ReviewDTO
            {
                Id = review.Id,
                Text = review.Text,
                Rating = review.Rating,
            };
                return StatusCode(201, reviewDTO);
            
        }

        // PUT api/<ReviewController>/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Review review)
        {
            var reviewToUpdate = _context.Reviews.Find(id);
            if (reviewToUpdate == null) 
            {
                return NotFound();
            }
            reviewToUpdate.Text = review.Text;
            reviewToUpdate.Rating = review.Rating;
            _context.Reviews.Update(reviewToUpdate);
            _context.SaveChanges();
            var reviewDTO = new ReviewDTO
            {
                Id = id,
                Text = reviewToUpdate.Text,
                Rating = reviewToUpdate.Rating,
            };
            return Ok(reviewDTO);
        }

        // DELETE api/<ReviewController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var review = _context.Reviews.Find(id);
            if (review == null)
            {
                return NotFound();
            }
            _context.Reviews.Remove(review);
            _context.SaveChanges();
            return NoContent();
        }
        // GET api/<ReviewController>/ReviewsByProduct/5
        [HttpGet("ReviewsByProduct/{id}")]
        public IActionResult GetByProductId(int id)
        {
            var productReviews = _context.Reviews.Where(r => r.ProductId == id).Select(r => new ReviewDTO
            {
                Id=r.Id,
                Text = r.Text,
                Rating = r.Rating,
            }).ToList();
            return Ok(productReviews);
        }
    }
}
