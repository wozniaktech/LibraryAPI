using FluentValidation;
using LibraryAPI.Application.Commands;
using LibraryAPI.Infrastructure;

namespace LibraryAPI.Application.Validators
{
    public class CreateBookCommandValidator:AbstractValidator<CreateBookCommand>
    {
        public CreateBookCommandValidator(LibraryContext context)
        {
            RuleFor(x => x.Title).NotEmpty().WithMessage("Title is required");
            RuleFor(x => x.Author).NotEmpty().WithMessage("Author is required");
            RuleFor(x => x.ISBN).NotEmpty().WithMessage("ISBN is required");
                               
        }
    }
}
