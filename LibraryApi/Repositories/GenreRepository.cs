using LibraryApi.Context;
using LibraryApi.Models;
using System.Reflection;

namespace LibraryApi.Repositories
{
    public class GenreRepository : Repository<Genre>, IGenreRepository
    {
        public GenreRepository(AppDbContext context) : base(context)
        {
        }
        public async Task<IEnumerable<Genre>> GetGenresByNameAsync(string genre)
        {
            var genres = await GetAllAsync();
            var genresBy = genres.Where(g => g.Name == genre);
            return genresBy;
        }
    }   
  
}
