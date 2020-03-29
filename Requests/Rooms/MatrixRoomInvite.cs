using System.Runtime.Serialization;

namespace libMatrix.Requests.Rooms
{
    [DataContract]
    public class MatrixRoomInvite
    {
        [DataMember(Name = "user_id")]
        public string UserID { get; set; }
    }

    [DataContract]
    public class MatrixRoomInviteThirdParty
    {
        // The hostname+port of the identity server for third party lookups
        [DataMember(Name = "id_server")]
        public string ID_Server { get; set; }
        [DataMember(Name = "medium")]
        public string Medium { get; set; }
        [DataMember(Name = "address")]
        public string Address { get; set; }
    }
}
