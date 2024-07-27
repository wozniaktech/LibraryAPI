using LibraryAPI.Infrastructure;
using MediatR;

namespace LibraryAPI.Application.Queries
{
    public class GetBookByIdQueryHandler: IRequestHandler<GetBookByIdQuery, BookDto>
    {
        private readonly LibraryContext _context;

        public GetBookByIdQueryHandler(LibraryContext context)
        {
            _context = context;
        }

        public async Task<BookDto> Handle(GetBookByIdQuery request, CancellationToken cancellationToken)
        {
            var book = await _context.Books.FindAsync(request.Id);

            if (book == null)
            {
                return null;
            }

            return new BookDto
            {
                Id = book.Id,
                Title = book.Title,
                Author = book.Author,
                ISBN = book.ISBN,
                Status = book.Status
            };
        }

    }
}
