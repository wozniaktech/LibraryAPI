using FluentValidation.AspNetCore;
using FluentValidation;
using LibraryAPI.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;
using LibraryAPI.Application.Validators;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<LibraryContext>(options =>options.UseInMemoryDatabase("LibraryDb"));
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(Program).Assembly));
builder.Services.AddValidatorsFromAssemblyContaining<CreateBookCommandValidator>();
builder.Services.AddFluentValidationAutoValidation();

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();
app.MapControllers();
app.Run();
