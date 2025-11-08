using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace LibraryApi.Models
{
    public class Genre
    {
        public Genre()
        {
            Books = new Collection<Book>();
        }

        [Key]
        public int GenreId { get; set; }

        [Required]
        [StringLength(80)]
        public string? Name { get; set; }

        [JsonIgnore]
        public ICollection<Book>? Books { get; set; }
    }
}

