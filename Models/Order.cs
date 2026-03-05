using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ecommerce.Models
{
    [Table("Order")]
    public class Order
    {
        public int Id { get; set; }
        [Required]
        public string UserId { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.UtcNow;
        public bool IsDeleted { get; set; } = false;
        [Required]
        public int OrderStatusId { get; set; }
        public OrderStatus OrderStatus { get; set; }

        public List<OrderDetail> OrderDetails { get; set; }
    }
}
