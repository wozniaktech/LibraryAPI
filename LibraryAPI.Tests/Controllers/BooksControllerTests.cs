using LibraryAPI.Application.Commands;
using LibraryAPI.Application.Queries;
using LibraryAPI.Controllers;
using LibraryAPI.Domain;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;

namespace LibraryAPI.Tests.Controllers
{
    [TestFixture]
    public class BooksControllerTests
    {
        private IMediator _mediator;
        private BooksController _controller;

        [SetUp]
        public void Setup()
        {
            _mediator = Substitute.For<IMediator>();
            _controller = new BooksController(_mediator);
        }

        [Test]
        public async Task GetBooks_ShouldReturnOk_WhenBooksExist()
        {
            // Arrange
            var query = new GetBooksQuery();
            var bookDtos = new List<BookDto>
            {
                new BookDto { Id = Guid.NewGuid(), Title = "Book 1", Author = "Author 1", ISBN = "123", Status = BookStatus.OnShelf },
                new BookDto { Id = Guid.NewGuid(), Title = "Book 2", Author = "Author 2", ISBN = "456", Status = BookStatus.Borrowed }
            };
            _mediator.Send(query).Returns(bookDtos);

            // Act
            var result = await _controller.GetBooks(query);

            // Assert
            Assert.That(result, Is.Not.Null, "Expected result to be not null.");
            var okResult = result as OkObjectResult;
            Assert.That(okResult, Is.Not.Null, "Expected result to be OkObjectResult.");
            Assert.That(okResult.StatusCode, Is.EqualTo(200), "Expected status code to be 200.");
            Assert.That(okResult.Value, Is.EqualTo(bookDtos), "Expected the result value to match the list of books.");
        }

        [Test]
        public async Task GetBookById_ShouldReturnBook_WhenBookExists()
        {
            // Arrange
            var bookId = Guid.NewGuid();
            var bookDto = new BookDto
            {
                Id = bookId,
                Title = "Book 1",
                Author = "Author 1",
                ISBN = "1234567890",
                Status = BookStatus.OnShelf
            };
            _mediator.Send(Arg.Is<GetBookByIdQuery>(q => q.Id == bookId)).Returns(bookDto);

            // Act
            var result = await _controller.GetBookById(bookId);

            // Assert
            Assert.That(result, Is.InstanceOf<OkObjectResult>(), "Expected result to be OkObjectResult.");
            var okResult = result as OkObjectResult;
            Assert.That(okResult.StatusCode, Is.EqualTo(200), "Expected status code to be 200.");
            Assert.That(okResult.Value, Is.EqualTo(bookDto), "Expected the result value to match the bookDto.");
        }

        [Test]
        public async Task GetBookById_ShouldReturnNotFound_WhenBookDoesNotExist()
        {
            // Arrange
            var bookId = Guid.NewGuid();
            _mediator.Send(new GetBookByIdQuery(bookId)).Returns((BookDto)null);

            // Act
            var result = await _controller.GetBookById(bookId);

            // Assert
            Assert.That(result, Is.InstanceOf<NotFoundResult>(), "Expected result to be NotFoundResult.");
        }

        [Test]
        public async Task CreateBook_ShouldReturnBookId_WhenBookIsCreated()
        {
            // Arrange
            var command = new CreateBookCommand { Title = "New Book", Author = "New Author", ISBN = "789", Status = BookStatus.OnShelf };
            var newBookId = Guid.NewGuid();
            _mediator.Send(command).Returns(newBookId);

            // Act
            var result = await _controller.CreateBook(command);

            // Assert
            Assert.That(result, Is.Not.Null, "Expected result to be not null.");
            var okResult = result as OkObjectResult;
            Assert.That(okResult, Is.Not.Null, "Expected result to be OkObjectResult.");
            Assert.That(okResult.StatusCode, Is.EqualTo(200), "Expected status code to be 200.");
            Assert.That(okResult.Value, Is.EqualTo(newBookId), "Expected the result value to match the new book ID.");
        }

        [Test]
        public async Task UpdateBook_ShouldReturnNoContent_WhenUpdateIsSuccessful()
        {
            // Arrange
            var bookId = Guid.NewGuid();
            var command = new UpdateBookCommand { Id = bookId, Title = "Updated Book", Author = "Updated Author", ISBN = "789", Status = BookStatus.Borrowed };

            // Act
            var result = await _controller.UpdateBook(bookId, command);

            // Assert
            Assert.That(result, Is.InstanceOf<NoContentResult>(), "Expected result to be NoContentResult.");
        }

        [Test]
        public async Task DeleteBook_ShouldReturnNoContent_WhenBookIsDeleted()
        {
            // Arrange
            var bookId = Guid.NewGuid();

            // Act
            var result = await _controller.DeleteBook(bookId);

            // Assert
            Assert.That(result, Is.InstanceOf<NoContentResult>(), "Expected result to be NoContentResult.");
        }

    }
}
