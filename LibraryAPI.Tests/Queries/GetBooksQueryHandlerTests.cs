using LibraryAPI.Application.Queries;
using LibraryAPI.Domain;
using LibraryAPI.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryAPI.Tests.Queries
{
    public class GetBooksQueryHandlerTests : IDisposable
    {
        private GetBooksQueryHandler _handler;
        private LibraryContext _context;

        [SetUp]
        public void SetUp()
        {
            var options = new DbContextOptionsBuilder<LibraryContext>()
                .UseInMemoryDatabase(databaseName: $"LibraryTest_{Guid.NewGuid()}")
                .Options;
            _context = new LibraryContext(options);
            _handler = new GetBooksQueryHandler(_context);

            SeedDatabase();
        }

        [Test]
        public async Task Handle_ShouldReturnBooksSortedByTitle_WhenSortByTitle()
        {
            // Arrange
            var query = new GetBooksQuery { SortBy = "title" };

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.EqualTo(4));
            Assert.That(result[0].Title, Is.EqualTo("Book A"));
            Assert.That(result[1].Title, Is.EqualTo("Book B"));
            Assert.That(result[2].Title, Is.EqualTo("Book C"));
            Assert.That(result[3].Title, Is.EqualTo("Book D"));
        }

        [Test]
        public async Task Handle_ShouldReturnBooksSortedByAuthorDescending_WhenSortByAuthorDescending()
        {
            // Arrange
            var query = new GetBooksQuery { SortBy = "author", SortDescending = true };

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.EqualTo(4));
            Assert.That(result[0].Author, Is.EqualTo("Author 4"));
            Assert.That(result[1].Author, Is.EqualTo("Author 3"));
            Assert.That(result[2].Author, Is.EqualTo("Author 2"));
            Assert.That(result[3].Author, Is.EqualTo("Author 1"));
        }

        private void SeedDatabase()
        {
            var books = new List<Book>
            {
                new Book { Id = Guid.NewGuid(), Title = "Book A", Author = "Author 1", ISBN = "1111", Status = BookStatus.OnShelf },
                new Book { Id = Guid.NewGuid(), Title = "Book B", Author = "Author 2", ISBN = "2222", Status = BookStatus.Borrowed },
                new Book { Id = Guid.NewGuid(), Title = "Book C", Author = "Author 3", ISBN = "3333", Status = BookStatus.Damaged },
                new Book { Id = Guid.NewGuid(), Title = "Book D", Author = "Author 4", ISBN = "4444", Status = BookStatus.Returned }
            };

            _context.Books.AddRange(books);
            _context.SaveChanges();
        }

        [TearDown]
        public void TearDown()
        {
            Dispose();
        }

        public void Dispose()
        {
            _context?.Dispose();
        }
    }
}
