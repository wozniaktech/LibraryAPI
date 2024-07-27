using MediatR;

namespace LibraryAPI.Application.Commands
{
    public class DeleteBookCommand: IRequest<Unit>
    {
        public Guid Id { get; set; }

        public DeleteBookCommand(Guid id)
        {
            Id = id;
        }
    }
}
