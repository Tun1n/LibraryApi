using LibraryApi.Models;

namespace LibraryApi.Repositories
{
    public interface IBookRepository : IRepository<Book>
    {
        IEnumerable<Book> GetBooksByGenre(int id);
    }

}
