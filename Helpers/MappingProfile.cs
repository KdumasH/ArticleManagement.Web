using AutoMapper;
using ArticleManagement.Web.Models.Articles;

namespace ArticleManagement.Web.Helpers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<ArticleDto, ArticleViewModel>()
                .ForMember(dest => dest.Author, opt => opt.MapFrom(src => src.userName));


            CreateMap<ArticleDetailDto, ArticleDetailViewModel>()
           .ForMember(dest => dest.Author, opt => opt.MapFrom(src => src.UserName));

            CreateMap<CommentDto, CommentViewModel>()
                .ForMember(dest => dest.Author, opt => opt.MapFrom(src => src.userName));
        }
    }
}