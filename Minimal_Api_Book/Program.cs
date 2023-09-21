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
using static System.Reflection.Metadata.BlobBuilder;
using Azure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();   

builder.Services.AddDbContext<DataContext>();

// AutoMapper

builder.Services.AddAutoMapper(typeof(AutoMapperConfig).Assembly);

// DTO
builder.Services.AddScoped<IGenericRepository<Book,CreateBookDto>, BookRepo>();
builder.Services.AddScoped<IGenericRepository<Genre,CreateGenreDto>, GenreRepo>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Book Crud
app.MapGet("/api/books", async ([FromServices] IGenericRepository<Book, CreateBookDto> repo) =>
{
    APIResponse response = new APIResponse();
    
    response.Result = await repo.GetAll();
    response.IsSuccess = true;
    response.StatusCode = System.Net.HttpStatusCode.OK;

    return Results.Ok(response);
    
}).Produces(200);

app.MapGet("/api/book/{id:int}", async ([FromServices] IGenericRepository<Book, CreateBookDto> repo, int id) =>
{

    APIResponse response = new APIResponse();

    var BookId = await repo.GetSingleById(id);

    if (BookId != null )
    {
        response.Result = BookId;
        response.IsSuccess = true;
        response.StatusCode = System.Net.HttpStatusCode.OK;

        return Results.Ok(response);
    }
    

    return Results.NotFound($"Book with the BookId: {id} does not exist");
    
}).WithName("GetBook");

app.MapPost("api/book", async ([FromServices] IGenericRepository<Book, CreateBookDto> repo, CreateBookDto book) =>
{
    APIResponse response = new() { IsSuccess = false, StatusCode = System.Net.HttpStatusCode.BadRequest };

    var addBook = await repo.Add(book);
    if (addBook == null)
    {
        return Results.BadRequest($"Book with Title: {book.Titel} and Author: {book.Author} already exist");
    }

    response.Result = addBook;
    response.IsSuccess = true;
    response.StatusCode = System.Net.HttpStatusCode.Created;
    return Results.Ok(response);

}).WithName("CreateBook").Accepts<CreateBookDto>("application/json").Produces<APIResponse>(201).Produces(400);


app.MapPut("api/book/{id:int}", async (IGenericRepository<Book, CreateBookDto> repo,Book book, int id) => 
{

    APIResponse response = new() { IsSuccess = false, StatusCode = System.Net.HttpStatusCode.BadRequest };

    var updatebook = await repo.Update(id,book);
    if (updatebook != null)
    {

        response.Result = updatebook;
        response.IsSuccess = true;
        response.StatusCode = System.Net.HttpStatusCode.OK;

        return Results.Ok(response);
    }

    return Results.NotFound("Update was not succesfull");

}).WithName("UpdateBook").Accepts<Book>("application/json").Produces<APIResponse>(200).Produces(400);

app.MapDelete("api/book/{id:int}", async (IGenericRepository<Book, CreateBookDto> repo, int id) =>
{

    APIResponse response = new APIResponse() { IsSuccess = false, StatusCode = System.Net.HttpStatusCode.BadRequest };

    var DeleteBook = await repo.Delete(id);

    if (DeleteBook != null)
    {
        response.Result = DeleteBook;
        response.IsSuccess = true;
        response.StatusCode = System.Net.HttpStatusCode.NoContent;
        return Results.Ok(DeleteBook);
    }

    return Results.NotFound($"BookId: {id} was not found");
    

}).WithName("DeleteBook").Produces<APIResponse>(200).Produces(400);

// Find book with Genre

app.MapGet("/api/book/genre/{id}", async (int id, [FromServices] IGenericRepository<Book, CreateBookDto> bookRepo,
                                            [FromServices] IGenericRepository<Genre, CreateGenreDto> genreRepo) =>
{
    APIResponse response = new APIResponse();

    // Fetch books with the specified GenreId
    var books = await bookRepo.GetAll();

    if (books.Any())
    {
        var genre = await genreRepo.GetSingleById(id);

        if (genre != null)
        {
            List<BookWithGenreDto> booksWithGenre = books
                .Where(book => book.GenreId == id)
                .Select(book => new BookWithGenreDto
                {
                    Titel = book.Titel,
                    About = book.About,
                    Author = book.Author,
                    Year = book.Year,
                    Loan = book.Loan,
                    GenreName = genre.GenreName
                })
                .ToList();

            response.Result = booksWithGenre;
            response.IsSuccess = true;
            response.StatusCode = System.Net.HttpStatusCode.OK;
            return Results.Ok(response);
        }
    }

    return Results.NotFound($"No books found for GenreId: {id}");

}).WithName("BooksByGenre");


// Find Books available for loan

app.MapGet("/api/book/loan", async ([FromServices] IGenericRepository<Book, CreateBookDto> repo) =>
{
    APIResponse response = new APIResponse();

    var availableBooks = (await repo.GetAll()).Where(b => b.Loan);

    if (availableBooks.Any())
    {
        response.Result = availableBooks;
        response.IsSuccess = true;
        response.StatusCode = System.Net.HttpStatusCode.OK;
        return Results.Ok(response);
    }
    return Results.NotFound("No available books for loan found.");
});

// All below is genre Genre CRUD

app.MapGet("/api/genres", async ([FromServices] IGenericRepository<Genre, CreateGenreDto> repo) =>
{
    APIResponse response = new APIResponse();


    response.Result = await repo.GetAll();
    response.IsSuccess = true;
    response.StatusCode = System.Net.HttpStatusCode.OK;
    return Results.Ok(response);


});

app.MapGet("/api/genre/{id:int}", async (IGenericRepository<Genre, CreateGenreDto> repo, int id) =>
{
    APIResponse response = new APIResponse();

    var genreid = await repo.GetSingleById(id);

    if (genreid != null)
    {
        response.Result = genreid;
        response.IsSuccess = true;
        response.StatusCode = System.Net.HttpStatusCode.OK;


        return Results.Ok(response);
    }

    return Results.NotFound($"Genre with the GenreId: {id} does not exist");
});

app.MapPost("/api/genre", async (IGenericRepository<Genre, CreateGenreDto> repo, CreateGenreDto genre) =>
{
    APIResponse response = new APIResponse();


    var addedGenre = await repo.Add(genre);
    if (addedGenre == null)
    {
        return Results.BadRequest($"{genre.GenreName} already exists");
    }
    response.Result = addedGenre;
    response.IsSuccess = true;
    response.StatusCode = System.Net.HttpStatusCode.Created;
    return Results.Ok(response);
});

app.MapPut("/api/genre/{id:int}", async (IGenericRepository<Genre, CreateGenreDto> repo, Genre genre, int id) =>
{

    APIResponse response = new APIResponse();
    var updatedGenre = await repo.Update(id, genre);
    if (updatedGenre != null)
    {
        response.Result = updatedGenre; ;
        response.IsSuccess = true;
        response.StatusCode = System.Net.HttpStatusCode.OK;

        return Results.Ok(response);
    }

    return Results.NotFound("Update was not succesfull");
});

app.MapDelete("/api/genre/{id:int}", async (IGenericRepository<Genre, CreateGenreDto> repo, int id) =>
{
    APIResponse response = new APIResponse() { IsSuccess = false, StatusCode = System.Net.HttpStatusCode.BadRequest };

    var deletedGenre = await repo.Delete(id);

    if (deletedGenre != null)
    {
        response.Result = deletedGenre;
        response.IsSuccess = true;
        response.StatusCode = System.Net.HttpStatusCode.NoContent;
        return Results.Ok(deletedGenre);
    }

    return Results.NotFound($"GenreId: {id} was not found");
});

app.Run();

