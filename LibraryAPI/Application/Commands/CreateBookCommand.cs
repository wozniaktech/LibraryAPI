using LibraryAPI.Domain;
using LibraryAPI.Infrastructure;
using MediatR;

namespace LibraryAPI.Application.Commands
{
    public class CreateBookCommand:IRequest<Guid>
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public string ISBN { get; set; }
        public BookStatus Status { get; set; }
    }
}
