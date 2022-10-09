using System.ComponentModel.DataAnnotations;

namespace Movies_Api.Dtos
{
    public class GenreDto
    {
        [Required]
        [MaxLength(50, ErrorMessage = "Max Length = 50 Char")]
        public string Name { get; set; }
    }
}
