using System.Runtime.Serialization;

namespace libMatrix.Requests.Rooms.Message
{
    [DataContract]
    public class MatrixRoomMessageEmote : MatrixRoomMessageBase
    {
        public MatrixRoomMessageEmote()
            : base()
        {
            base.MessageType = "m.emote";
        }
    }
}
