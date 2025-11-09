using System.ComponentModel.DataAnnotations;

namespace LibraryApi.DTO
{
    public class BookDTO
    {
        public int BookId { get; set; }

        [Required]
        [StringLength(80)]
        public string? Title { get; set; }

        [Required]
        [StringLength(300)]
        public string? Description { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        public string? Author { get; set; }
        public int YearLaunch { get; set; }
        public int GenreId { get; set; }
    }
}
