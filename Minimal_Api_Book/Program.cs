using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Minimal_Api_Book.Context;
using Minimal_Api_Book.Data;
using Minimal_Api_Book.Services;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http.HttpResults;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<DataContext>();

builder.Services.AddScoped<IBookRepository, BookRepo>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


app.MapGet("/api/book", async (IBookRepository repo) =>
{
    var books = await repo.GetAllBooks();

    return Results.Ok(books);

});

app.MapGet("/api/book/{id:int}", async (IBookRepository repo, int id) =>
{
    var book = await repo.GetSingleBook(id);
    if (book != null)
    {
        return Results.Ok(book);
    }
    return Results.NotFound($"Book with ID:{id} not Found");
    
});

app.MapPost("api/book", async (IBookRepository repo, Book book) =>
{
    var addBook = await repo.AddBook(book);
    if (addBook == null)
    {
        return Results.BadRequest($"BookId {book.BookId} already Exists");
    }
    return Results.Ok(addBook);
});

app.MapPut("api/book/{id:int}", async (IBookRepository repo,Book book, int id) => 
{
    var updatebook = await repo.UpdateBook(id,book);
    if (updatebook != null)
    {
        return Results.Ok(updatebook);
    }
    return Results.NotFound("BookId not found");
});

app.MapDelete("api/Book/{id:int}", async (IBookRepository repo, int id) =>
{
    var DeleteBook = await repo.DeleteBook(id);

    if (DeleteBook != null)
    {
        return Results.Ok(DeleteBook);
    }
    return Results.NotFound("BookId not found");
    

});

app.Run();

