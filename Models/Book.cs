using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ecommerce.Models
{
    [Table("Book")]
    public class Book
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(40)]
        public string? BookName { get; set; }

        public string? ImageUrl { get; set; }
        [Required]
        public double Price { get; set; }
        [Required]
        public int GenreId { get; set; }
        public Genre Genre { get; set; }
        public List<OrderDetail> OrderDetails { get; set; }
        public List<CartDetail> CartDetails { get; set; }


    }
}
