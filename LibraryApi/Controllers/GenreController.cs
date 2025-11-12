using AutoMapper;
using LibraryApi.DTO;
using LibraryApi.Models;
using LibraryApi.Pagination;
using LibraryApi.Repositories.UnitOfWork;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryApi.Controllers
{
    public class GenreController : ControllerBase
    {
        private readonly ILogger<GenreController> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public GenreController(ILogger<GenreController> logger, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet("Genres")]
        public async Task<ActionResult<IEnumerable<Genre>>> GetAllGenresDetailsAsync()
        {
            var genres = await _unitOfWork.GenreRepository.GetAllAsync();

            if (genres == null || !genres.Any())
            {
                return NotFound("Genres not found.");
            }

            return Ok(genres);
        }
        [HttpGet("GenresPagination")]
        public async Task<ActionResult<IEnumerable<GenreDTO>>> GetGenresPagination([FromQuery] GenresParameters genresParameters)
        {
            var genres = await _unitOfWork.GenreRepository.GetAllGenresPaginationAsync(genresParameters);
            if (genres == null || !genres.Any()) {
                return NotFound("Genres not found");
            }

            var genresDto = _mapper.Map<IEnumerable<GenreDTO>>(genres);

            return Ok(genresDto);
        }

        [HttpGet("GenreByIdDetails/{id}", Name = "GenreByIdDetails")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<ActionResult<Genre>> GetGenreByIdAsync(int id)
        {
            var genre = await _unitOfWork.GenreRepository.GetAsync(g => g.GenreId == id);
            if (genre == null)
            {
                return NotFound("Genre not found.");
            }

            return Ok(genre);
        }

        [HttpGet("Genre/{genreName}")]
        public async Task<ActionResult<Genre>> GetGenreByNameAsync(string genreName)
        {

            var genre = await _unitOfWork.GenreRepository.GetGenresByNameAsync(genreName);
            if (genre == null)
            {
                return NotFound("Genre not found.");
            }
            return Ok(genre);
        }

        [HttpGet("Genre/Books/GenreId/{genreId}")]
        public async Task<ActionResult<IEnumerable<BookDTO>>> GetBooksByGenreIdAsync(int genreId)
        {
            var books = await _unitOfWork.GenreRepository.GetBooksByGenreIdAsync(genreId);
            if (books == null || !books.Any())
            {
                return NotFound("Genre not found");
            }
            var booksDto = _mapper.Map<IEnumerable<BookDTO>>(books);
            return Ok(booksDto);
        }

        [HttpGet("Genre/Books/GenreName/{genreName}")]
        public async Task<ActionResult<IEnumerable<BookDTO>>> GetBooksByGenreNameAsync(string genreName)
        {
            var books = await _unitOfWork.GenreRepository.GetBooksByGenreNameAsync(genreName);
            if (books == null || !books.Any())
            {
                return NotFound("Genre not found");
            }
            var booksDto = _mapper.Map<IEnumerable<BookDTO>>(books);
            return Ok(booksDto);
        }

        // HTTP POST
        [HttpPost("GenreByIdDetails")]
        [Authorize(Policy = "AdminOnly")]

        public async Task<ActionResult<Genre>> CreateGenreAsync([FromBody] Genre genre)
        {
            var genreExist = await _unitOfWork.GenreRepository.GetAsync(b => b.GenreId == genre.GenreId);
            if (genre == null || genreExist != null)
            {
                return BadRequest("Invalid Operation");
            }

            var newGenre = _unitOfWork.GenreRepository.Create(genre);

            await _unitOfWork.CommitAsync();

            return new CreatedAtRouteResult("GenreByIdDetails", new { id = newGenre.GenreId }, newGenre);
        }

        // HTTP PUT
        [HttpPut("{id:int}")]
        [Authorize(Policy = "AdminOnly")]

        public async Task<ActionResult<Genre>> Put(int id, Genre genre)
        {
            if (id != genre.GenreId)
            {
                return BadRequest();
            }

            var updated = _unitOfWork.GenreRepository.Update(genre);

            await _unitOfWork.CommitAsync();

            return Ok(updated);
        }

        // HTTP DELETE

        [HttpDelete("{id:int}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<ActionResult<Genre>> Delete(int id)
        {
            var genre = await _unitOfWork.GenreRepository.GetAsync(c => c.GenreId == id);
            if (genre is null)
            {
                return NotFound("Genre not found");
            }

            var deleted = _unitOfWork.GenreRepository.Delete(genre);

            await _unitOfWork.CommitAsync();

            return Ok(deleted);
        }
    }
}

