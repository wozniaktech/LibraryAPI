using LibraryAPI.Application.Commands;
using LibraryAPI.Domain;
using LibraryAPI.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace LibraryAPI.Tests.Commands
{
    [TestFixture]
    public class CreateBookCommandHandlerTests : IDisposable
    {
        private CreateBookCommandHandler _handler;
        private LibraryContext _context;

        [SetUp]
        public void SetUp()
        {
            var options = new DbContextOptionsBuilder<LibraryContext>()
                .UseInMemoryDatabase(databaseName: $"LibraryTest_{Guid.NewGuid()}")
                .Options;
            _context = new LibraryContext(options);
            _handler = new CreateBookCommandHandler(_context);
        }

        [Test]
        public async Task Handle_ShouldAddNewBook_WhenBookDoesNotExist()
        {
            // Arrange
            var command = new CreateBookCommand
            {
                Title = "New Book",
                Author = "John Doe",
                ISBN = "1234567890",
                Status = BookStatus.OnShelf
            };

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.That(result, Is.Not.EqualTo(Guid.Empty));
            Assert.That(_context.Books.Any(b => b.ISBN == command.ISBN), Is.True);
        }

        [Test]
        public void Handle_ShouldThrowException_WhenBookWithSameISBNExists()
        {
            // Arrange
            var existingBook = new Book
            {
                Id = Guid.NewGuid(),
                Title = "Existing Book",
                Author = "Jane Doe",
                ISBN = "1234567890",
                Status = BookStatus.OnShelf
            };
            _context.Books.Add(existingBook);
            _context.SaveChanges();

            var command = new CreateBookCommand
            {
                Title = "New Book",
                Author = "John Doe",
                ISBN = "1234567890",
                Status = BookStatus.OnShelf
            };

            // Act & Assert
            var ex = Assert.ThrowsAsync<InvalidOperationException>(() => _handler.Handle(command, CancellationToken.None));
            Assert.That(ex.Message, Is.EqualTo("Book with the same ISBN already exists"));
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
