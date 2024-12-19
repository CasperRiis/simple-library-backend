using AutoMapper;
using LibraryApi.Entities;
using LibraryApi.Models;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Author, AuthorDTO>();
        CreateMap<AuthorDTO, Author>();

        CreateMap<Book, BookDTO>()
            .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Author!.Name));
        CreateMap<BookDTO, Book>();

        CreateMap<Author, AuthorDTO_NestedBooks>();
        CreateMap<AuthorDTO_NestedBooks, Author>();

        CreateMap<Book, BookDTO_NestedAuthor>();
        CreateMap<BookDTO_NestedAuthor, Book>();
    }
}