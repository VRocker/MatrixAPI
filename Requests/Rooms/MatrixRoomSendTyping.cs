using System.Runtime.Serialization;

namespace libMatrix.Requests.Rooms
{
    [DataContract]
    class MatrixRoomSendTyping
    {
        [DataMember(Name = "typing")]
        public bool Typing { get; set; }
        [DataMember(Name = "timeout", IsRequired = false, EmitDefaultValue = false)]
        public int Timeout { get; set; }
    }
}
