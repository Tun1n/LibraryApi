using LibraryApi.Context;
using LibraryApi.Models;
using LibraryApi.Pagination;
using Microsoft.EntityFrameworkCore;


namespace LibraryApi.Repositories
{
    public class BookRepository : Repository<Book>, IBookRepository
    {
        public BookRepository(AppDbContext context) : base(context)
        {

        }

        public async Task<IEnumerable<Book>> GetAllBooksPaginationAsync(BooksParameters booksParameters)
        {
            var books = await GetAllAsync();
            return books.OrderBy(c => c.Title)
                .Skip((booksParameters.PageNumber - 1) * booksParameters.PageSize)
                .Take(booksParameters.PageSize).ToList();
        }

        public async Task<IEnumerable<Book>> GetBooksByAuthorAsync(string author)
        {
            var books = await GetAllAsync();
            var booksByAuthor = books.Where(b => b.Author!.Equals(author, StringComparison.OrdinalIgnoreCase));
            return booksByAuthor;
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
