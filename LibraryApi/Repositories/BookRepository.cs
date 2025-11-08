using LibraryApi.Context;
using LibraryApi.Models;

namespace LibraryApi.Repositories
{
    public class BookRepository : Repository<Book>, IBookRepository
    {
        public BookRepository(AppDbContext context) : base(context)
        {

        }
        public IEnumerable<Book> GetBooksByGenre(int id)
        {
            return GetAll().Where(p => p.GenreId == id);
        }
    }
}
