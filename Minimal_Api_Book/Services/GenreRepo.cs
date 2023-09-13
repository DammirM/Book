using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Minimal_Api_Book.Context;
using Minimal_Api_Book.Data;

namespace Minimal_Api_Book.Services
{
    public class GenreRepo
    {

        private readonly DataContext _Context;
        private readonly IMapper _mapper;

        public GenreRepo(DataContext db, IMapper mapper)
        {
            _Context = db;
            _mapper = mapper;
        }

        public async Task<GenreDto> Add(GenreDto genreDto)
        {
            var genre = _mapper.Map<Genre>(genreDto); 
            _Context.Genres.Add(genre);
            await _Context.SaveChangesAsync();

            return _mapper.Map<GenreDto>(genre); 
        }

        public async Task<GenreDto> Delete(int id)
        {
            var genreToDelete = await _Context.Genres.FirstOrDefaultAsync(g => g.GenreId == id);

            if (genreToDelete != null)
            {
                _Context.Genres.Remove(genreToDelete);
                await _Context.SaveChangesAsync();

                return _mapper.Map<GenreDto>(genreToDelete); 
            }

            return null;
        }

        public async Task<IEnumerable<GenreDto>> GetAll()
        {
            var genres = await _Context.Genres
                .ProjectTo<GenreDto>(_mapper.ConfigurationProvider) 
                .ToListAsync();

            return genres;
        }

        public async Task<GenreDto> GetSingleById(int id)
        {
            var genre = await _Context.Genres.FirstOrDefaultAsync(g => g.GenreId == id);

            return _mapper.Map<GenreDto>(genre); 
        }

        public async Task<GenreDto> Update(int id, GenreDto genreDto)
        {
            var updateGenre = await _Context.Genres.FirstOrDefaultAsync(g => g.GenreId == id);

            if (updateGenre != null)
            {
                updateGenre.GenreName = genreDto.GenreName;

                await _Context.SaveChangesAsync();

                return _mapper.Map<GenreDto>(updateGenre); 
            }

            return null;
        }
    }
}
