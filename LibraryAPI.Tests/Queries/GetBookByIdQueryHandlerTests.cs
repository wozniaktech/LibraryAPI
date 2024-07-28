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
    public class GetBookByIdQueryHandlerTests : IDisposable
    {
        private GetBookByIdQueryHandler _handler;
        private LibraryContext _context;

        [SetUp]
        public void SetUp()
        {
            var options = new DbContextOptionsBuilder<LibraryContext>()
                .UseInMemoryDatabase(databaseName: $"LibraryTest_{Guid.NewGuid()}")
                .Options;
            _context = new LibraryContext(options);
            _handler = new GetBookByIdQueryHandler(_context);

            SeedDatabase();
        }

        [Test]
        public async Task Handle_ShouldReturnBook_WhenBookExists()
        {
            // Arrange
            var bookId = _context.Books.First().Id;
            var query = new GetBookByIdQuery(bookId);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Id, Is.EqualTo(bookId));
            Assert.That(result.Title, Is.EqualTo("Book A"));
        }

        [Test]
        public async Task Handle_ShouldReturnNull_WhenBookDoesNotExist()
        {
            // Arrange
            var query = new GetBookByIdQuery(Guid.NewGuid());

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.That(result, Is.Null);
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
