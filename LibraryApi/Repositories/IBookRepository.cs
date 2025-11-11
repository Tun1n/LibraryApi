using LibraryApi.Models;


namespace LibraryApi.Repositories
{
    public interface IBookRepository : IRepository<Book>
    {
        Task<IEnumerable<Book>> GetBooksByAuthorAsync(string author);
        Task<IEnumerable<Book>> GetBooksByYearLaunchAsync(int yearLaunch);
        Task<IEnumerable<Book>> GetBooksByYearRangeAsync(int startYear, int endYear);

    }
}
