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

    response.Result = await repo.GetSingleById(id);
    response.IsSuccess = true;
    response.StatusCode = System.Net.HttpStatusCode.OK;


    return Results.Ok(response);
    
}).WithName("GetBook");

app.MapPost("api/book", async ([FromServices] IGenericRepository<Book, CreateBookDto> repo, CreateBookDto book) =>
{
    APIResponse response = new() { IsSuccess = false, StatusCode = System.Net.HttpStatusCode.BadRequest };

    var addBook = await repo.Add(book);
    if (addBook == null)
    {
        return Results.BadRequest(response);
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
    return Results.NotFound(response);
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
    return Results.NotFound($"BookId: {id} not found");
    

}).WithName("DeleteBook");

// Find book with Genre

app.MapGet("/api/book/genre/{genreId:int}", async ([FromServices] IGenericRepository<Book, CreateBookDto> repo, int genreId) =>
{

    APIResponse response = new APIResponse();

    var books = (await repo.GetAll()).Where(b => b.GenreId == genreId);

    if (books.Any())
    {
        response.Result = books;
        response.IsSuccess = true;
        response.StatusCode = System.Net.HttpStatusCode.OK;
        return Results.Ok(response);
    }
    return Results.NotFound($"No books found for GenreId:{genreId}");

}).WithName("GenreBook");

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

// All below is genre CRUD

app.MapGet("/api/genre", async ([FromServices] IGenericRepository<Genre, CreateGenreDto> repo) =>
{
    APIResponse response = new APIResponse();

    var genres = await repo.GetAll();

    if (genres != null && genres.Any()) // Check if there are books
    {
        response.Result = genres;
        response.IsSuccess = true;
        response.StatusCode = System.Net.HttpStatusCode.OK;
        return Results.Ok(response);
    }
    else
    {
        response.IsSuccess = false;
        response.StatusCode = System.Net.HttpStatusCode.NotFound;
        response.ErrorMessages = new List<string> { "No Genre Found" };
        return Results.NotFound(response);
    }
}).Produces(200);

app.MapGet("/api/genre/{id:int}", async (IGenericRepository<Genre, CreateGenreDto> repo, int id) =>
{
    APIResponse response = new APIResponse();

    response.Result = await repo.GetSingleById(id);
    response.IsSuccess = true;
    response.StatusCode = System.Net.HttpStatusCode.OK;


    return Results.Ok(response);
});

//app.MapPost("/api/genre", async (IGenericRepository<Genre, GenreDto> repo, GenreDto genre) =>
//{
//    var addedGenre = await repo.Add(genre);
//    if (addedGenre == null)
//    {
//        return Results.BadRequest($"{genre.GenreName} already exists");
//    }
//    return Results.Ok(addedGenre);
//});

//app.MapPut("/api/genre/{id:int}", async (IGenericRepository<Genre, GenreDto> repo, Genre genre, int id) =>
//{
//    var updatedGenre = await repo.Update(id, genre);
//    if (updatedGenre != null)
//    {
//        return Results.Ok(updatedGenre);
//    }
//    return Results.NotFound($"Genre with ID:{id} not found");
//});

//app.MapDelete("/api/genre/{id:int}", async (IGenericRepository<Genre, GenreDto> repo, int id) =>
//{
//    var deletedGenre = await repo.Delete(id);

//    if (deletedGenre != null)
//    {
//        return Results.Ok(deletedGenre);
//    }
//    return Results.NotFound($"Genre with ID:{id} not found");
//});

app.Run();

