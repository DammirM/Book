using AutoMapper;
using Minimal_Api_Book.Data;

namespace Minimal_Api_Book
{
    public class AutoMapperConfig : Profile
    {

        public AutoMapperConfig()
        {
            CreateMap<Book, CreateBookDto>().ReverseMap();
            CreateMap<Genre, GenreDto>().ReverseMap();
        }
    }
}
