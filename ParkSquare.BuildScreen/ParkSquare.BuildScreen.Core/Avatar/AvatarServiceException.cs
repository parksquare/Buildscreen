using System;
using System.Runtime.Serialization;

namespace ParkSquare.BuildScreen.Core.Avatar
{
    [Serializable]
    public class AvatarServiceException : Exception
    {
        public AvatarServiceException()
        {
        }

        protected AvatarServiceException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public AvatarServiceException(string message) : base(message)
        {
        }

        public AvatarServiceException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
