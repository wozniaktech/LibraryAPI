using LibraryAPI.Application.Commands;
using LibraryAPI.Domain;
using LibraryAPI.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryAPI.Tests.Commands
{
    [TestFixture]
    public class UpdateBookCommandHandlerTests : IDisposable
    {
        private UpdateBookCommandHandler _handler;
        private LibraryContext _context;

        [SetUp]
        public void SetUp()
        {
            var options = new DbContextOptionsBuilder<LibraryContext>()
                .UseInMemoryDatabase(databaseName: $"LibraryTest_{Guid.NewGuid()}")
                .Options;
            _context = new LibraryContext(options);
            _handler = new UpdateBookCommandHandler(_context);
        }

        [Test]
        public async Task Handle_ShouldUpdateBook_WhenBookExists()
        {
            // Arrange
            var book = new Book
            {
                Id = Guid.NewGuid(),
                Title = "Original Title",
                Author = "Original Author",
                ISBN = "1234567890",
                Status = BookStatus.OnShelf
            };
            _context.Books.Add(book);
            await _context.SaveChangesAsync();

            var command = new UpdateBookCommand
            {
                Id = book.Id,
                Title = "Updated Title",
                Author = "Updated Author",
                ISBN = "0987654321",
                Status = BookStatus.Borrowed
            };

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.That(result, Is.EqualTo(Unit.Value));

            var updatedBook = await _context.Books.FindAsync(book.Id);
            Assert.That(updatedBook, Is.Not.Null);
            Assert.That(updatedBook.Title, Is.EqualTo("Updated Title"));
            Assert.That(updatedBook.Author, Is.EqualTo("Updated Author"));
            Assert.That(updatedBook.ISBN, Is.EqualTo("0987654321"));
            Assert.That(updatedBook.Status, Is.EqualTo(BookStatus.Borrowed));
        }

        [Test]
        public void Handle_ShouldThrowException_WhenBookDoesNotExist()
        {
            // Arrange
            var nonExistentBookId = Guid.NewGuid();
            var command = new UpdateBookCommand
            {
                Id = nonExistentBookId,
                Title = "Updated Title",
                Author = "Updated Author",
                ISBN = "0987654321",
                Status = BookStatus.Borrowed
            };

            // Act & Assert
            var ex = Assert.ThrowsAsync<KeyNotFoundException>(() => _handler.Handle(command, CancellationToken.None));
            Assert.That(ex.Message, Is.EqualTo("Book not found"));
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
