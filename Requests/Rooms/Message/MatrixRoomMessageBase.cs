using System.Runtime.Serialization;

namespace libMatrix.Requests.Rooms.Message
{
    [DataContract]
    public class MatrixRoomMessageBase
    {
        [DataMember(Name = "msgtype")]
        public string MessageType { get; set; }
    }
}
