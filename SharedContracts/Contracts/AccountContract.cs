using System.Runtime.Serialization;

namespace SharedContracts.Contracts
{
    [DataContract]
    public class AccountContract
    {
        [DataMember]
        public string Username { get; set; }

        [DataMember]
        public List<int> ThuocTinh { get; set; }
    }
}
