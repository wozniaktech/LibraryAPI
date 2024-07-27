using LibraryAPI.Domain;
using LibraryAPI.Infrastructure;
using MediatR;

namespace LibraryAPI.Application.Commands
{
    public class CreateBookCommandHandler : IRequestHandler<CreateBookCommand, Guid>
    {
        private readonly LibraryContext _context;

        public CreateBookCommandHandler(LibraryContext context)
        {
            _context = context;
        }

        public async Task<Guid> Handle(CreateBookCommand request, CancellationToken cancellationToken)
        {
            if (_context.Books.Any(b => b.ISBN == request.ISBN))
            {
                throw new InvalidOperationException("Book with the same ISBN already exists");
            }

            var book = new Book
            {
                Id = Guid.NewGuid(),
                Title = request.Title,
                Author = request.Author,
                ISBN = request.ISBN,
                Status = BookStatus.OnShelf
            };

            _context.Books.Add(book);
            await _context.SaveChangesAsync(cancellationToken);

            return book.Id;
        }
    }
}
