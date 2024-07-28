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
    public class DeleteBookCommandHandlerTests : IDisposable
    {
        private DeleteBookCommandHandler _handler;
        private LibraryContext _context;

        [SetUp]
        public void SetUp()
        {
            var options = new DbContextOptionsBuilder<LibraryContext>()
                .UseInMemoryDatabase(databaseName: $"LibraryTest_{Guid.NewGuid()}")
                .Options;
            _context = new LibraryContext(options);
            _handler = new DeleteBookCommandHandler(_context);
        }

        [Test]
        public async Task Handle_ShouldDeleteBook_WhenBookExists()
        {
            // Arrange
            var book = new Book
            {
                Id = Guid.NewGuid(),
                Title = "Book to Delete",
                Author = "John Doe",
                ISBN = "1234567890",
                Status = BookStatus.OnShelf
            };
            _context.Books.Add(book);
            await _context.SaveChangesAsync();

            var command = new DeleteBookCommand(book.Id);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.That(result, Is.EqualTo(Unit.Value));
            Assert.That(await _context.Books.FindAsync(book.Id), Is.Null);
        }

        [Test]
        public void Handle_ShouldThrowException_WhenBookDoesNotExist()
        {
            // Arrange
            var nonExistentBookId = Guid.NewGuid();
            var command = new DeleteBookCommand(nonExistentBookId);

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
