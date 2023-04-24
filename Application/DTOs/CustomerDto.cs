using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;


namespace Application.DTOs
{
    public class CustomerDto
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public bool IsActive { get; set; }

        [Required]
        public string Phone { get; set; }
    }

    public class CreateCustomerDto
    {
        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [Phone]
        public string Phone { get; set; }

        [Required]
        public bool IsActive { get; set; }
    }

    public class UpdateCustomerDto
    {
        [JsonIgnore]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [Phone]
        public string Phone { get; set; }

        [Required]
        public bool IsActive { get; set; }
    }


}
