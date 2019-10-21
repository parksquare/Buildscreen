using System.Collections.Generic;

namespace ParkSquare.BuildScreen.AzureDevOps.Build.Dto
{
    public class RefsResponseDto
    {
        public List<RefDto> Value { get; set; }

        public int Count { get; set; }
    }
}