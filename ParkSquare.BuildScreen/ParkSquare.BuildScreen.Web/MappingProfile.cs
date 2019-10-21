using AutoMapper;
using ParkSquare.BuildScreen.Web.Models;

namespace ParkSquare.BuildScreen.Web
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Build.Build, BuildInfoDto>()
                .ForMember(dest => dest.RequestedByPictureUrl,
                    opt => opt.MapFrom(src => CreateAvatarUrl(src)));
        }

        private static string CreateAvatarUrl(Build.Build build)
        {
            return $"avatar/{build.RequestedForId}/{build.RequestedForUniqueName}";
        }
    }
}
