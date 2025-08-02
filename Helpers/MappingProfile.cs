using AutoMapper;
using ArticleManagement.Web.Models.Articles;

namespace ArticleManagement.Web.Helpers;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<ArticleDto, ArticleViewModel>()
            .ForMember(dest => dest.Author, opt => opt.MapFrom(src => src.userName));


        CreateMap<ArticleDetailDto, ArticleDetailViewModel>()
       .ForMember(dest => dest.Author, opt => opt.MapFrom(src => src.userName));

        CreateMap<CommentDto, CommentViewModel>()
            .ForMember(dest => dest.Author, opt => opt.MapFrom(src => src.author))
            .ForMember(dest => dest.Text, opt => opt.MapFrom(src => src.content));

        CreateMap<CreateArticleViewModel, CreateArticleRequest>();

        CreateMap<EditArticleViewModel, UpdateArticleRequest>();
        CreateMap<ArticleDetailDto, EditArticleViewModel>();
    }
}