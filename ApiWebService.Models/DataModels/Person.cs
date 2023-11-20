using System.ComponentModel.DataAnnotations;

namespace ApiWebService.Models.DataModels
{
    public class Person
    {
        public Guid Id { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        public List<Note> Notes { get; set; } = new List<Note>();
    }
}