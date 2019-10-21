namespace ParkSquare.BuildScreen.AzureDevOps.Build
{
    public interface IBranchNameConverter
    {
        string Convert(string branchName);
    }
}