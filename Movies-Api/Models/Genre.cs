using System.ComponentModel.DataAnnotations;

namespace Movies_Api.Models
{
    public class Genre
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(50, ErrorMessage = "Max Length = 50 Char")]
        public string Name { get; set; }
    }
}
