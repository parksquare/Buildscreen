using AutoMapper;
using ParkSquare.BuildScreen.Web.Build;
using ParkSquare.BuildScreen.Web.Models;

namespace ParkSquare.BuildScreen.Web
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<BuildTile, BuildInfoDto>()
                .ForMember(dest => dest.RequestedByPictureUrl,
                    opt => opt.MapFrom(src => CreateAvatarUrl(src)));
        }

        private static string CreateAvatarUrl(BuildTile build)
        {
            return $"avatar/{build.RequestedForId}/{build.RequestedForUniqueName}";
        }
    }
}
