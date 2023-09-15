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
            var genre = _mapper.Map<Genre>(genreDto);
            _Context.Genres.Add(genre);
            await _Context.SaveChangesAsync();

            return _mapper.Map<CreateGenreDto>(genre);
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
            var genre = await _Context.Genres.ToListAsync();

            return genre;
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
