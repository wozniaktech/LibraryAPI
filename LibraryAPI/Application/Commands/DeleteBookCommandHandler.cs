using LibraryAPI.Infrastructure;
using MediatR;

namespace LibraryAPI.Application.Commands
{
    public class DeleteBookCommandHandler: IRequestHandler<DeleteBookCommand, Unit>
    {
        private readonly LibraryContext _context;

        public DeleteBookCommandHandler(LibraryContext context)
        {
            _context = context;
        }
        public async Task<Unit> Handle(DeleteBookCommand request, CancellationToken cancellationToken)
        {
            var book = await _context.Books.FindAsync(request.Id);

            if (book == null)
            {
                throw new KeyNotFoundException("Book not found");
            }

            _context.Books.Remove(book);
            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }

    }
}
