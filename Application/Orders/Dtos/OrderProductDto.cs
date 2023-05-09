using System.ComponentModel.DataAnnotations;

namespace Application.Orders.Dtos
{
    public class OrderProductDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Order ID is required.")]
        public int OrderId { get; set; }

        [Required(ErrorMessage = "Product ID is required.")]
        public int ProductId { get; set; }

        [Required(ErrorMessage = "Price is required.")]
        [Range(0, double.MaxValue, ErrorMessage = "Price must be a positive value.")]
        public decimal Price { get; set; }

        [MaxLength(100, ErrorMessage = "MembershipName must not exceed 100 characters.")]
        public string MembershipName { get; set; }

        [Required(ErrorMessage = "Quantity is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be a positive value.")]
        public int Quantity { get; set; }
    }


    public class CreateOrderProductDto
    {
        [Required(ErrorMessage = "Product ID is required.")]
        public int ProductId { get; set; }

        [Required(ErrorMessage = "Order Product ID is required.")]
        public int OrderId { get; set; }


        [Required(ErrorMessage = "Price is required.")]
        [Range(0, double.MaxValue, ErrorMessage = "Price must be a positive value.")]
        public decimal Price { get; set; }

        [MaxLength(100, ErrorMessage = "MembershipName must not exceed 100 characters.")]
        public string MembershipName { get; set; }

        [Required(ErrorMessage = "Quantity is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be a positive value.")]
        public int Quantity { get; set; }
    }


    public class UpdateOrderProductDto
    {

        public int Id { get; set; }

        [Required(ErrorMessage = "Order Product ID is required.")]
        public int ProductId { get; set; }

        [Required(ErrorMessage = "Product ID is required.")]
        public int OrderId { get; set; }

        [Required(ErrorMessage = "Price is required.")]
        [Range(0, double.MaxValue, ErrorMessage = "Price must be a positive value.")]
        public decimal Price { get; set; }

        [MaxLength(100, ErrorMessage = "MembershipName must not exceed 100 characters.")]
        public string MembershipName { get; set; }

        [Required(ErrorMessage = "Quantity is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be a positive value.")]
        public int Quantity { get; set; }
    }

}
