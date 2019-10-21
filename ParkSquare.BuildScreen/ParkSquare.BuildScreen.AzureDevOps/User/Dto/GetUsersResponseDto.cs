using System.Collections.Generic;

namespace ParkSquare.BuildScreen.AzureDevOps.User.Dto
{
    public class GetUsersResponseDto
    {
        public int Count { get; set; }

        public IReadOnlyCollection<UserDto> Value { get; set; }
    }
}
