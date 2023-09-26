using Minimal_Api_Book.Context;
using Minimal_Api_Book.Data;
using Minimal_Api_Book.Services;
using Web_Book.Controllers;
using Web_Book.Services;

namespace Web_Book
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            builder.Services.AddHttpClient<IBookService, BookService>();
            builder.Services.AddScoped<IBookService, BookService>();

            builder.Services.AddHttpClient<IGenreService, GenreService>();
            builder.Services.AddScoped<IGenreService, GenreService>();


            builder.Services.AddLogging();

            StaticDetails.BookApiBase = builder.Configuration["ServiceUrls:BooksDbApi"];

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Book}/{action=MyIndex}/{id?}");

            app.Run();
        }
    }
}