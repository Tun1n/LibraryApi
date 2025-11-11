using AutoMapper;
using LibraryApi.DTO;
using LibraryApi.Models;
using LibraryApi.Repositories.UnitOfWork;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LibraryApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly ILogger<BookController> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public BookController(ILogger<BookController> logger, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        // HTTP GET
        [HttpGet("BookDetails")]
        [Authorize(Policy = "AdminOnly")]

        public async Task<ActionResult<IEnumerable<Book>>> GetAllBooksDetailsAsync()
        {
            var books = await _unitOfWork.BookRepository.GetAllAsync();
            if (books == null)
            {
                return NotFound("Book not found"); ;
            }
            return Ok(books);
        }


        [HttpGet("BookByIdDetails/{id}", Name = "BookByIdDetails")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<ActionResult<Book>> GetBookByIdDetailsAsync(int id)
        {
            var book = await _unitOfWork.BookRepository.GetAsync(b => b.BookId == id);
            if (book == null)
            {
                return NotFound("Book not found");
            }

            return Ok(book);
        }


        [HttpGet("Books")]
        public async Task<ActionResult<IEnumerable<BookDTO>>> GetAllBooksAsync()
        {
            var books = await _unitOfWork.BookRepository.GetAllAsync();
            if (books == null)
            {
                return NotFound("Book not found");
            }

            var booksDto = _mapper.Map<IEnumerable<BookDTO>>(books);
            return Ok(booksDto);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BookDTO>> GetBookByIdAsync(int id)
        {
            var book = await _unitOfWork.BookRepository.GetAsync(b => b.BookId == id);
            if (book == null)
            {
                return NotFound("Book not found");
            }

            var bookDto = _mapper.Map<BookDTO>(book);
            return Ok(bookDto);
        }

        [HttpGet("Books/author/{author}")]
        public async Task<ActionResult<IEnumerable<BookDTO>>> GetBooksByAuthorAsync(string author)
        {
            var books = await _unitOfWork.BookRepository.GetBooksByAuthorAsync(author);
            if (books == null || !books.Any())
            {
                return NotFound("Book not found");
            }
            var booksDto = _mapper.Map<IEnumerable<BookDTO>>(books);
            return Ok(booksDto);
        }

        [HttpGet("Books/year/{launchYear:int}")]
        public async Task<ActionResult<IEnumerable<BookDTO>>> GetBooksByYearLaunch(int launchYear)
        {
            var books = await _unitOfWork.BookRepository.GetBooksByYearLaunchAsync(launchYear);
            if (books == null || !books.Any())
            {
                return NotFound("Book not found");
            }

            var booksDto = _mapper.Map<IEnumerable<BookDTO>>(books);
            return Ok(booksDto);
        }

        [HttpGet("Books/yearRange")]
        public async Task<IActionResult> GetBooksByYearRange(
        [FromQuery] int startYear,
        [FromQuery] int endYear)
        {
            if (startYear > endYear)
            {
                return BadRequest("The initial year cannot be longer than the final year.");
            }
            var books = await _unitOfWork.BookRepository.GetBooksByYearRangeAsync(startYear, endYear);

            if (books == null || !books.Any())
            {
                return NotFound("Book not found");
            }

            var booksDto = _mapper.Map<IEnumerable<BookDTO>>(books);
            return Ok(booksDto);
        }

        // HTTP POST

        [HttpPost("BookByIdDetails")]
        [Authorize(Policy = "AdminOnly")]

        public async Task<ActionResult<Book>> CreateBookAsync([FromBody] Book book)
        {
            if (book == null)
            {
                return BadRequest();
            }

            var newBook = _unitOfWork.BookRepository.Create(book);

            await _unitOfWork.CommitAsync();

            return new CreatedAtRouteResult("BookByIdDetails", new { id = newBook.BookId }, newBook);
        }

        // HTTP PUT
        [HttpPut("{id:int}")]
        [Authorize(Policy = "AdminOnly")]

        public async Task<ActionResult<Book>> Put(int id, Book book)
        {
            if (id != book.BookId)
            {
                return BadRequest();
            }

            var updated = _unitOfWork.BookRepository.Update(book);

            await _unitOfWork.CommitAsync();

            return Ok(updated);
        }

        // HTTP DELETE

        [HttpDelete("{id:int}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<ActionResult<Book>> Delete(int id)
        {
            var book = await _unitOfWork.BookRepository.GetAsync(c => c.BookId == id);
            if (book is null)
            {
                return NotFound("Book not found");
            }

            var deleted = _unitOfWork.BookRepository.Delete(book);

            await _unitOfWork.CommitAsync();

            return Ok(deleted);
        }
    }
}

