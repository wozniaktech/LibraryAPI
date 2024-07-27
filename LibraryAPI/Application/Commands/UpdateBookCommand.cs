using LibraryAPI.Domain;
using MediatR;

namespace LibraryAPI.Application.Commands
{
    public class UpdateBookCommand: IRequest<Unit>
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string ISBN { get; set; }
        public BookStatus Status { get; set; }
    }
}
