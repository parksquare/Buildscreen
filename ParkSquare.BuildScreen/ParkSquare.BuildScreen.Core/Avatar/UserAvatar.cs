namespace ParkSquare.BuildScreen.Core.Avatar
{
    public class UserAvatar
    {
        public static UserAvatar NotAvailable = new UserAvatar();

        public byte[] Data { get; set; }

        public string ContentType { get; set; }
    }
}