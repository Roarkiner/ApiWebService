using System.ComponentModel.DataAnnotations;

namespace ApiWebService.Models.DataModels
{
    public class Person
    {
        public Guid Id { get; set; }
        [Required]
        public string Email { get; set; } = null!;
        [Required]
        public string Password { get; set; } = null!;
        public bool IsDeleted { get; set; }
    }
}