using System.Collections.Generic;

namespace ParkSquare.BuildScreen.AzureDevOps.Build.Dto
{
    public class GetBuildsResponseDto
    {
        public int Count { get; set; }

        public IReadOnlyCollection<BuildDto> Value { get; set; }
    }
}
