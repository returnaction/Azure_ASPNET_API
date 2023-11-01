using System.ComponentModel.DataAnnotations;

namespace ProductsReviewsWebAPI.DTOs
{
    public class ReviewDTO
    {
        [Key]
        public int Id { get; set; }
        public string Text { get; set; }
        public int Rating { get; set; }
    }
}
