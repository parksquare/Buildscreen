using System;
using System.Runtime.Serialization;

namespace ParkSquare.BuildScreen.Web.AzureDevOps
{
    [Serializable]
    public class AzureDevOpsProviderException : Exception
    {
        public AzureDevOpsProviderException()
        {
        }

        protected AzureDevOpsProviderException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public AzureDevOpsProviderException(string message) : base(message)
        {
        }

        public AzureDevOpsProviderException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}