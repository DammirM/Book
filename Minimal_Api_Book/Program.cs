using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Minimal_Api_Book.Context;
using Minimal_Api_Book.Data;
using Minimal_Api_Book.Services;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http.HttpResults;
using Minimal_Api_Book;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();   

builder.Services.AddDbContext<DataContext>();

// AutoMapper

builder.Services.AddAutoMapper(typeof(AutoMapperConfig).Assembly);

// DTO

builder.Services.AddScoped<IGenericRepository<BookDto>, BookRepo>();
builder.Services.AddScoped<IGenericRepository<GenreDto>, GenreRepo>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


app.MapGet("/api/book", async (IGenericRepository<BookDto> repo) =>
{
    var books = await repo.GetAll();

    return Results.Ok(books);

});

app.MapGet("/api/book/{id:int}", async (IGenericRepository<BookDto> repo, int id) =>
{
    var book = await repo.GetSingleById(id);
    if (book != null)
    {
        return Results.Ok(book);
    }
    return Results.NotFound($"Book with ID:{id} not Found");
    
});

app.MapPost("api/book", async (IGenericRepository<BookDto> repo, BookDto book) =>
{
    var addBook = await repo.Add(book);
    if (addBook == null)
    {
        return Results.BadRequest($"Book with the Title: {book.Titel} and Author: {book.Author} already exists");
    }
    return Results.Ok(addBook);
});

app.MapPut("api/book/{id:int}", async (IGenericRepository<BookDto> repo,BookDto book, int id) => 
{
    var updatebook = await repo.Update(id,book);
    if (updatebook != null)
    {
        return Results.Ok(updatebook);
    }
    return Results.NotFound($"BookId: {id} not found");
});

app.MapDelete("api/Book/{id:int}", async (IGenericRepository<BookDto> repo, int id) =>
{
    var DeleteBook = await repo.Delete(id);

    if (DeleteBook != null)
    {
        return Results.Ok(DeleteBook);
    }
    return Results.NotFound($"BookId: {id} not found");
    

});

// Find book with Genre

app.MapGet("/api/book/genre/{genreId:int}", async (IGenericRepository<BookDto> repo, int genreId) =>
{
    var books = (await repo.GetAll()).Where(b => b.GenreId == genreId);

    if (books.Any())
    {
        return Results.Ok(books);
    }
    return Results.NotFound($"No books found for GenreId:{genreId}");
});

app.MapGet("/api/book/loan", async (IGenericRepository<BookDto> repo) =>
{
    var availableBooks = (await repo.GetAll()).Where(b => b.Loan);

    if (availableBooks.Any())
    {
        return Results.Ok(availableBooks);
    }
    return Results.NotFound("No available books for loan found.");
});

// All below is genre CRUD

app.MapGet("/api/genre", async (IGenericRepository<GenreDto> repo) =>
{
    var genres = await repo.GetAll();

    return Results.Ok(genres);
});

app.MapGet("/api/genre/{id:int}", async (IGenericRepository<GenreDto> repo, int id) =>
{
    var genre = await repo.GetSingleById(id);
    if (genre != null)
    {
        return Results.Ok(genre);
    }
    return Results.NotFound($"Genre with ID:{id} not found");
});

app.MapPost("/api/genre", async (IGenericRepository<GenreDto> repo, GenreDto genre) =>
{
    var addedGenre = await repo.Add(genre);
    if (addedGenre == null)
    {
        return Results.BadRequest($"{genre.GenreName} already exists");
    }
    return Results.Ok(addedGenre);
});

app.MapPut("/api/genre/{id:int}", async (IGenericRepository<GenreDto> repo, GenreDto genre, int id) =>
{
    var updatedGenre = await repo.Update(id, genre);
    if (updatedGenre != null)
    {
        return Results.Ok(updatedGenre);
    }
    return Results.NotFound($"Genre with ID:{id} not found");
});

app.MapDelete("/api/genre/{id:int}", async (IGenericRepository<GenreDto> repo, int id) =>
{
    var deletedGenre = await repo.Delete(id);

    if (deletedGenre != null)
    {
        return Results.Ok(deletedGenre);
    }
    return Results.NotFound($"Genre with ID:{id} not found");
});

app.Run();

