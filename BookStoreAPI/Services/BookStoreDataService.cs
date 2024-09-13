using BookStore.Shared.Entities;
using BookStoreAPI.Data;

namespace BookStoreAPI.Services
{
    /// <summary>
    /// Provides methods for retrieving, adding, updating, and deleting book data from the database using BookStoreDbContext.
    /// </summary>
    public class BookStoreDataService
    {
        private readonly BookStoreDbContext _dbContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="BookStoreDataService"/> class.
        /// </summary>
        /// <param name="dbContext">The BookStoreDbContext instance.</param>
        public BookStoreDataService(BookStoreDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Retrieves all books from the database.
        /// </summary>
        /// <returns>An IEnumerable of Book objects.</returns>
        public IEnumerable<Book> GetBooks()
        {
            return _dbContext.Books.ToList();
        }

        /// <summary>
        /// Retrieves a book by its ID from the database.
        /// </summary>
        /// <param name="id">The ID of the book.</param>
        /// <returns>The Book object with the specified ID.</returns>
        public Book GetBookById(int id)
        {
            var book = _dbContext.Books.FirstOrDefault(b => b.Id == id);
            if (book == null)
            {
                throw new Exception("Book not found");
            }

            return book;
        }

        /// <summary>
        /// Searches for books in the database based on a search term.
        /// </summary>
        /// <param name="searchTerm">The search term to match against book titles and authors.</param>
        /// <returns>An IEnumerable of Book objects that match the search term.</returns>
        public IEnumerable<Book> SearchBooks(string searchTerm)
        {
            return _dbContext.Books.Where(b => b.Title.Contains(searchTerm) || b.Author.Contains(searchTerm)).ToList();
        }

        /// <summary>
        /// Adds a new book to the database.
        /// </summary>
        /// <param name="book">The Book object to add.</param>
        public void AddBook(Book book)
        {
            _dbContext.Books.Add(book);
            _dbContext.SaveChanges();
        }

        /// <summary>
        /// Updates an existing book in the database.
        /// </summary>
        /// <param name="book">The updated Book object.</param>
        public void UpdateBook(Book book)
        {
            var existingBook = _dbContext.Books.Find(book.Id);
            if (existingBook == null)
            {
                throw new Exception("Book not found");
            }

            if (existingBook.Title != book.Title)
            {
                existingBook.Title = book.Title;
            }

            if (existingBook.Author != book.Author)
            {
                existingBook.Author = book.Author;
            }

            if (existingBook.NoOfPages != book.NoOfPages)
            {
                existingBook.NoOfPages = book.NoOfPages;
            }

            if (existingBook.Language != book.Language)
            {
                existingBook.Language = book.Language;
            }

            if (existingBook.Category != book.Category)
            {
                existingBook.Category = book.Category;
            }

            if (existingBook.Price != book.Price)
            {
                existingBook.Price = book.Price;
            }

            if (existingBook.ImageUrl != book.ImageUrl)
            {
                existingBook.ImageUrl = book.ImageUrl;
            }

            _dbContext.SaveChanges();
        }

        /// <summary>
        /// Deletes a book from the database.
        /// </summary>
        /// <param name="id">The ID of the book to delete.</param>
        public void DeleteBook(int id)
        {
            var book = _dbContext.Books.FirstOrDefault(b => b.Id == id);
            if (book == null)
            {
                throw new Exception("Book not found");
            }

            _dbContext.Books.Remove(book);
            _dbContext.SaveChanges();
        }
    }
}
