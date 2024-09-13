using BookStore.Shared.Entities;
using BookStoreAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly BookStoreDataService _bookStoreDataService;

        public BooksController(BookStoreDataService bookStoreDataService)
        {
            _bookStoreDataService = bookStoreDataService;
        }

        /// <summary>
        /// Retrieves all books from the database.
        /// </summary>
        /// <returns>An IEnumerable of Book objects.</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IEnumerable<Book> GetBooks()
        {
            return _bookStoreDataService.GetBooks();
        }

        /// <summary>
        /// Retrieves a book by its id.
        /// </summary>
        /// <param name="id">The id of the book.</param>
        /// <returns>The book with the specified id.</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Book> GetBookById(int id)
        {
            try
            {
                var book = _bookStoreDataService.GetBookById(id);
                if (book == null)
                {
                    return NotFound();
                }
                return book;
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Inserts a new book into the database.
        /// </summary>
        /// <param name="book">The book object to insert.</param>
        /// <returns>The inserted book.</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<Book> InsertBook([FromBody] Book book)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _bookStoreDataService.AddBook(book);
            return CreatedAtAction(nameof(GetBookById), new { id = book.Id }, book);
        }
        /// <summary>
        /// Updates an existing book in the database.
        /// </summary>
        /// <param name="id">The id of the book to update.</param>
        /// <param name="book">The updated Book object.</param>
        /// <returns>An IActionResult indicating the result of the update operation.</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult UpdateBook(int id, [FromBody] Book book)
        {
            if (id != book.Id)
            {
                return BadRequest();
            }

            try
            {
                _bookStoreDataService.UpdateBook(book);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }



    }
}
