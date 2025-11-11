using LibraryApi.Context;
using LibraryApi.Models;
using Microsoft.EntityFrameworkCore;
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
        public async Task<IEnumerable<Book>> GetBooksByGenreNameAsync(string genre)
        {
            return await _context.Books!
                .Include(b => b.Genre)
                .Where(b => b.Genre != null &&
                            b.Genre.Name!.ToLower() == genre.ToLower())
                .ToListAsync();
        }

        public async Task<IEnumerable<Book>> GetBooksByGenreIdAsync(int id)
        {
            return await _context.Books!
                .Include(b => b.Genre)
                .Where(b => b.Genre != null &&
                            b.Genre.GenreId == id)
                .ToListAsync();
        }
    }   
}
