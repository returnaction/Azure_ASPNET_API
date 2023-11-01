using System.ComponentModel.DataAnnotations;

namespace ProductsReviewsWebAPI.DTOs
{
    public class ProductDTO
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public ICollection<ReviewDTO> Reviews { get; set; }
        public double AverageRating { get; set; }
    }
}
