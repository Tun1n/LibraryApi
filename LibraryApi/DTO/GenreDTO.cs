using System.ComponentModel.DataAnnotations;

namespace LibraryApi.DTO
{
    public class GenreDTO
    {
        public int GenreId { get; set; }

        [Required]
        [StringLength(80)]
        public string? Name { get; set; }
    }
}
