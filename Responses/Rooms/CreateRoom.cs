using System.Runtime.Serialization;

namespace libMatrix.Responses.Rooms
{
    [DataContract]
    public class CreateRoom
    {
        [DataMember(Name = "room_id")]
        public string RoomID { get; set; }
    }
}
