using LibraryApi.Models;
using LibraryApi.Pagination;

namespace LibraryApi.Repositories
{
    public interface IGenreRepository : IRepository<Genre>
    {
        Task<IEnumerable<Genre>> GetGenresByNameAsync(string genre);
        Task<IEnumerable<Book>> GetBooksByGenreNameAsync(string genre);
        Task<IEnumerable<Book>> GetBooksByGenreIdAsync(int id);
        Task<IEnumerable<Genre>> GetAllGenresPaginationAsync(GenresParameters genresParameters);
    }
}
