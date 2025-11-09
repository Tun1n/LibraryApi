using LibraryApi.Context;
using LibraryApi.Models;
using Microsoft.EntityFrameworkCore;


namespace LibraryApi.Repositories
{
    public class BookRepository : Repository<Book>, IBookRepository
    {
        public BookRepository(AppDbContext context) : base(context)
        {

        }

        public async Task<IEnumerable<Book>> GetBooksByAuthorAsync(string author)
        {
            var books = await GetAllAsync();
            var booksByAuthor = books.Where(b => b.Author!.Equals(author, StringComparison.OrdinalIgnoreCase));
            return booksByAuthor;
        }

        public async Task<IEnumerable<Book>> GetBooksByGenreIdAsync(int id)
        {
            var books = await GetAllAsync();
            var booksByGenre = books.Where(b => b.GenreId == id);
            return booksByGenre;
        }

        public async Task<IEnumerable<Book>> GetBooksByGenreNameAsync(string genre)
        {
            return await _context.Books!
                .Include(b => b.Genre)
                .Where(b => b.Genre != null &&
                            b.Genre.Name!.ToLower() == genre.ToLower())
                .ToListAsync();
        }

        public async Task<IEnumerable<Book>> GetBooksByYearLaunchAsync(int yearLaunch)
        {
            var books = await GetAllAsync();
            var booksByYear = books.Where(b => b.YearLaunch == yearLaunch);
            return booksByYear;
        }

        public async Task<IEnumerable<Book>> GetBooksByYearRangeAsync(int startYear, int endYear)
        {
            var books = await GetAllAsync();
            var booksByYearRange = books.Where(b => b.YearLaunch >= startYear && b.YearLaunch <= endYear);
            return booksByYearRange;

        }
    }
}
