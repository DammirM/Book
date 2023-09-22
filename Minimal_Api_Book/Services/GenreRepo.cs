using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Minimal_Api_Book.Context;
using Minimal_Api_Book.Data;

namespace Minimal_Api_Book.Services
{
    public class GenreRepo : IGenericRepository<Genre, CreateGenreDto>
    {

        private readonly DataContext _Context;
        private readonly IMapper _mapper;

        public GenreRepo(DataContext db, IMapper mapper)
        {
            _Context = db;
            _mapper = mapper;
        }

        public async Task<CreateGenreDto> Add(CreateGenreDto genreDto)
        {
            var existingGenre = await _Context.Genres.FirstOrDefaultAsync(b =>
                b.GenreName == genreDto.GenreName);

            if (existingGenre != null)
            {
                return null;
            }

            var genre = _mapper.Map<Genre>(genreDto);

            var addedGenre = await _Context.Genres.AddAsync(genre);
            await _Context.SaveChangesAsync();

            return _mapper.Map<CreateGenreDto>(addedGenre.Entity);

        }

        public async Task<Genre> Delete(int id)
        {
            var genreToDelete = await _Context.Genres.FirstOrDefaultAsync(g => g.GenreId == id);

            if (genreToDelete != null)
            {
                _Context.Genres.Remove(genreToDelete);
                await _Context.SaveChangesAsync();
                return genreToDelete;
            }

            return null;
        }

        public async Task<IEnumerable<Genre>> GetAll()
        {
            var genre = await _Context.Genres.Include(b => b.Books).ToListAsync();

            return genre;
        }

        public Task<List<Book>> GetBooksByGenreNameAsync(string genreName)
        {
            throw new NotImplementedException();
        }

        public async Task<Genre> GetSingleById(int id)
        {
            var genre = await _Context.Genres.FirstOrDefaultAsync(g => g.GenreId == id);

            return genre;
        }

        public async Task<Genre> Update(int id, Genre genre)
        {
            var updateGenre = await _Context.Genres.FirstOrDefaultAsync(g => g.GenreId == id);

            if (updateGenre != null)
            {
                updateGenre.GenreName = genre.GenreName;

                await _Context.SaveChangesAsync();

                return updateGenre;
            }

            return null;
        }

    }
}
