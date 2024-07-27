using LibraryAPI.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LibraryAPI.Application.Queries
{
    public class GetBooksQueryHandler: IRequestHandler<GetBooksQuery, List<BookDto>>
    {
        private readonly LibraryContext _context;

        public GetBooksQueryHandler(LibraryContext context)
        {
            _context = context;
        }

        public async Task<List<BookDto>> Handle(GetBooksQuery request, CancellationToken cancellationToken)
        {
            var query = _context.Books.AsQueryable();

            switch (request.SortBy.ToLower())
            {
                case "title":
                    query = request.SortDescending ? query.OrderByDescending(b => b.Title) : query.OrderBy(b => b.Title);
                    break;
                case "author":
                    query = request.SortDescending ? query.OrderByDescending(b => b.Author) : query.OrderBy(b => b.Author);
                    break;
                case "isbn":
                    query = request.SortDescending ? query.OrderByDescending(b => b.ISBN) : query.OrderBy(b => b.ISBN);
                    break;
                case "status":
                    query = request.SortDescending ? query.OrderByDescending(b => b.Status) : query.OrderBy(b => b.Status);
                    break;
                default:
                    query = query.OrderBy(b => b.Title);
                    break;
            }

            query = query.Skip((request.PageNumber - 1) * request.PageSize)
                         .Take(request.PageSize);

            var books = await query.ToListAsync(cancellationToken);

            return books.Select(b => new BookDto
            {
                Id = b.Id,
                Title = b.Title,
                Author = b.Author,
                ISBN = b.ISBN,
                Status = b.Status
            }).ToList();
        }
    }
}
