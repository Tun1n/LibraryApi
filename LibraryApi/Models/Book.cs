using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace LibraryApi.Models
{
    [Table("Books")]
    public class Book
    {
        [Key]
        public int BookId { get; set; }

        [Required]
        [StringLength(80)]
        public string? Title { get; set; }

        [Required]
        [StringLength(300)]
        public string? Description { get; set; }

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal Price { get; set; }

        [Required]
        public string? Author { get; set; }
        public int YearLaunch { get; set; }
        public float Stock { get; set; }
        public int GenreId { get; set; }

        [JsonIgnore]
        public Genre? Genre { get; set; }
    }
}
