using System.ComponentModel.DataAnnotations;

namespace ApiWebService.Models.DataModels
{
    public class Note
    {
        public Guid Id { get; set; }
        [Required]
        public string Title { get; set; } = null!;
        public string? Content { get; set; }
        [Required]
        public Guid PersonId { get; set; }
        public Person? Person { get; set; }
        public bool IsDeleted { get; set; }
    }
}