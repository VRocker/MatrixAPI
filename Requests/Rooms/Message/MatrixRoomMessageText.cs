using System.Runtime.Serialization;

namespace libMatrix.Requests.Rooms.Message
{
    [DataContract]
    public class MatrixRoomMessageText : MatrixRoomMessageBase
    {
        public MatrixRoomMessageText()
            : base()
        {
            base.MessageType = "m.text";
        }

        [DataMember(Name = "body")]
        public string Body { get; set; }
    }
}
