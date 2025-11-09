using LibraryApi.Context;

namespace LibraryApi.Repositories.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private IBookRepository? _BookRepo;

        private IGenreRepository? _GenreRepo;

        public AppDbContext _context;

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
        }

        public IBookRepository BookRepository
        {
            get
            {
                return _BookRepo = _BookRepo ?? new BookRepository(_context);
            }
        }

        public IGenreRepository GenreRepository
        {
            get
            {
                return _GenreRepo = _GenreRepo ?? new GenreRepository(_context);
            }
        }
        public async Task CommitAsync()
        {
            await _context.SaveChangesAsync();
        }
        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
