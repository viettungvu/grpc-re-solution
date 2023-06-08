using System.Runtime.Serialization;

namespace SharedContracts.Contracts
{
    [DataContract]
    public class BaseResponse
    {
        [DataMember(Order = 1)]
        public bool Success { get; set; }
        [DataMember(Order = 2)]
        public string Message { get; set; }
    }
}
