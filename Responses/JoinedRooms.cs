using System.Collections.Generic;
using System.Runtime.Serialization;

namespace libMatrix.Responses
{
    [DataContract]
    public class JoinedRooms
    {
        [DataMember(Name = "joined_rooms")]
        public List<string> Rooms { get; set; }
    }
}
