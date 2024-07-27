using LibraryAPI.Domain;
using MediatR;

namespace LibraryAPI.Application.Queries
{
    public class GetBooksQuery: IRequest<List<BookDto>>
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string SortBy { get; set; } = "Title";
        public bool SortDescending { get; set; } = false;
    }

    public class BookDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string ISBN { get; set; }
        public BookStatus Status { get; set; }
    }
}