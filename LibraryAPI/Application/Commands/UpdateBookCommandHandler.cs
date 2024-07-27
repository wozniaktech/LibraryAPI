using LibraryAPI.Infrastructure;
using MediatR;

namespace LibraryAPI.Application.Commands
{
    public class UpdateBookCommandHandler : IRequestHandler<UpdateBookCommand, Unit>
    {
        private readonly LibraryContext _context;

        public UpdateBookCommandHandler(LibraryContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(UpdateBookCommand request, CancellationToken cancellationToken)
        {
            var book = await _context.Books.FindAsync(request.Id);

            if (book == null)
            {
                throw new KeyNotFoundException("Book not found");
            }

            book.Title = request.Title;
            book.Author = request.Author;
            book.ISBN = request.ISBN;
            book.SetStatus(request.Status);

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
