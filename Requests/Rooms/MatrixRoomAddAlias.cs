using System.Runtime.Serialization;

namespace libMatrix.Requests.Rooms
{
    [DataContract]
    public class MatrixRoomAddAlias
    {
        [DataMember(Name = "room_id")]
        public string RoomID { get; set; }
    }
}
