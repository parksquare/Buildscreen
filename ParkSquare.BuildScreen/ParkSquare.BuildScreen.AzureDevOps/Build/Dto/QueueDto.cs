namespace ParkSquare.BuildScreen.AzureDevOps.Build.Dto
{
    public class QueueDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public PoolDto Pool { get; set; }
    }
}