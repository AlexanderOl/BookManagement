using AutoMapper;
using BookServer.Models;
using Shared.Models;

namespace BookServer.Extensions;
public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<BookModel, BookView>();
        CreateMap<CsvBookRow, BookModel>()
            .ForMember(dest => dest.Id,
                       opt => opt.MapFrom(src => Guid.NewGuid()));
        CreateMap<UpsertBookArg, BookModel>()
            .ForMember(dest => dest.Id,
                       opt => opt.MapFrom(src => src.Id.HasValue ? src.Id : Guid.NewGuid()));
    }
}
