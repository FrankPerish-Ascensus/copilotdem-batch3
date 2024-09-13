using BookStore.Shared.Entities;
using BookStoreAPI.Data;
using BookStoreAPI.Services;
using Microsoft.EntityFrameworkCore;
using Xunit;
using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace BookStoreAPI.Tests.Services
{
    public class BookStoreDataServiceTests
    {
        private DbContextOptions<BookStoreDbContext> GetInMemoryDbContextOptions()
        {
            return new DbContextOptionsBuilder<BookStoreDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;
        }

        [Fact]
        public void GetBooks_ShouldReturnAllBooks()
        {
            // Arrange
            var options = GetInMemoryDbContextOptions();
            using (var dbContext = new BookStoreDbContext(options))
            {
                dbContext.Books.AddRange(
                    new Book { Id = 1, Title = "Book 1", Author = "Author 1" },
                    new Book { Id = 2, Title = "Book 2", Author = "Author 2" }
                );
                dbContext.SaveChanges();
            }

            // Act
            using (var dbContext = new BookStoreDbContext(options))
            {
                var dataService = new BookStoreDataService(dbContext);
                var result = dataService.GetBooks();

                // Assert
                Assert.Equals(2, result.Count());
            }
        }

        [Fact]
        public void GetBookById_ExistingId_ShouldReturnBook()
        {
            // Arrange
            var options = GetInMemoryDbContextOptions();
            using (var dbContext = new BookStoreDbContext(options))
            {
                dbContext.Books.Add(new Book { Id = 1, Title = "Book 1", Author = "Author 1" });
                dbContext.SaveChanges();
            }

            // Act
            using (var dbContext = new BookStoreDbContext(options))
            {
                var dataService = new BookStoreDataService(dbContext);
                var result = dataService.GetBookById(1);

                // Assert
                Assert.NotNull(result);
                Assert.Equals(1, result.Id);
            }
        }

        [Fact]
        public void GetBookById_NonExistingId_ShouldThrowException()
        {
            // Arrange
            var options = GetInMemoryDbContextOptions();

            // Act & Assert
            using (var dbContext = new BookStoreDbContext(options))
            {
                var dataService = new BookStoreDataService(dbContext);
                Assert.Throws<Exception>(() => dataService.GetBookById(1));
            }
        }

        [Fact]
        public void SearchBooks_ShouldReturnMatchingBooks()
        {
            // Arrange
            var options = GetInMemoryDbContextOptions();
            using (var dbContext = new BookStoreDbContext(options))
            {
                dbContext.Books.AddRange(
                    new Book { Id = 1, Title = "C# Programming", Author = "Author 1" },
                    new Book { Id = 2, Title = "Java Programming", Author = "Author 2" }
                );
                dbContext.SaveChanges();
            }

            // Act
            using (var dbContext = new BookStoreDbContext(options))
            {
                var dataService = new BookStoreDataService(dbContext);
                var result = dataService.SearchBooks("C#");

                // Assert
                Assert.Single(result);
                Assert.Equals("C# Programming", result.First().Title);
            }
        }

        [Fact]
        public void AddBook_ShouldAddBook()
        {
            // Arrange
            var options = GetInMemoryDbContextOptions();
            var newBook = new Book { Id = 1, Title = "New Book", Author = "New Author" };

            // Act
            using (var dbContext = new BookStoreDbContext(options))
            {
                var dataService = new BookStoreDataService(dbContext);
                dataService.AddBook(newBook);
            }

            // Assert
            using (var dbContext = new BookStoreDbContext(options))
            {
                Assert.Single(dbContext.Books);
                Assert.Equals("New Book", dbContext.Books.First().Title);
            }
        }

        [Fact]
        public void UpdateBook_ExistingBook_ShouldUpdateBook()
        {
            // Arrange
            var options = GetInMemoryDbContextOptions();
            using (var dbContext = new BookStoreDbContext(options))
            {
                dbContext.Books.Add(new Book { Id = 1, Title = "Old Title", Author = "Old Author" });
                dbContext.SaveChanges();
            }

            // Act
            using (var dbContext = new BookStoreDbContext(options))
            {
                var dataService = new BookStoreDataService(dbContext);
                var updatedBook = new Book { Id = 1, Title = "New Title", Author = "New Author" };
                dataService.UpdateBook(updatedBook);
            }

            // Assert
            using (var dbContext = new BookStoreDbContext(options))
            {
                var book = dbContext.Books.First();
                Assert.Equals("New Title", book.Title);
                Assert.Equals("New Author", book.Author);
            }
        }

        [Fact]
        public void UpdateBook_NonExistingBook_ShouldThrowException()
        {
            // Arrange
            var options = GetInMemoryDbContextOptions();
            var updatedBook = new Book { Id = 1, Title = "New Title", Author = "New Author" };

            // Act & Assert
            using (var dbContext = new BookStoreDbContext(options))
            {
                var dataService = new BookStoreDataService(dbContext);
                Assert.Throws<Exception>(() => dataService.UpdateBook(updatedBook));
            }
        }

        [Fact]
        public void DeleteBook_ExistingId_ShouldDeleteBook()
        {
            // Arrange
            var options = GetInMemoryDbContextOptions();
            using (var dbContext = new BookStoreDbContext(options))
            {
                dbContext.Books.Add(new Book { Id = 1, Title = "Book to Delete", Author = "Author" });
                dbContext.SaveChanges();
            }

            // Act
            using (var dbContext = new BookStoreDbContext(options))
            {
                var dataService = new BookStoreDataService(dbContext);
                dataService.DeleteBook(1);
            }

            // Assert
            using (var dbContext = new BookStoreDbContext(options))
            {
                Assert.Empty(dbContext.Books);
            }
        }

        [Fact]
        public void DeleteBook_NonExistingId_ShouldThrowException()
        {
            // Arrange
            var options = GetInMemoryDbContextOptions();

            // Act & Assert
            using (var dbContext = new BookStoreDbContext(options))
            {
                var dataService = new BookStoreDataService(dbContext);
                Assert.Throws<Exception>(() => dataService.DeleteBook(1));
            }
        }
    }
}
