using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace libMatrix.Responses.Pushers
{
    [DataContract]
    public class MatrixNotifications
    {
        [DataMember(Name ="next_token")]
        public string NextToken { get; set; }
        [DataMember(Name = "notifications")]
        public Notification[] Notifications { get; set; }
    }

    [DataContract]
    public class Notification
    {
        /*[DataMember(Name = "actions")]
        public string[] Actions { get; set; }*/
        [DataMember(Name = "profile_tag")]
        public string ProfileTag { get; set; }
        [DataMember(Name = "read")]
        public bool Read { get; set; }
        [DataMember(Name = "room_id")]
        public string RoomID { get; set; }
        [DataMember(Name = "ts")]
        public long Timestamp { get; set; }
        [DataMember(Name = "event")]
        public NotificationEvent _event { get; set; }
    }

    [DataContract]
    public class NotificationEvent : MatrixEvents
    {
        [DataMember(Name = "content")]
        [JsonConverter(typeof(Helpers.MatrixEventsRoomMessageItemConverter))]
        public Events.Room.MessageContent Content { get; set; }
        
    }
}