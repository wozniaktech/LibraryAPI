using MediatR;

namespace LibraryAPI.Application.Queries
{
    public class GetBookByIdQuery: IRequest<BookDto>
    {
        public Guid Id { get; set; }

        public GetBookByIdQuery(Guid id)
        {
            Id = id;
        }
    }
}
