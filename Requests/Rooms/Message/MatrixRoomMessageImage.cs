using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace libMatrix.Requests.Rooms.Message
{
    [DataContract]
    public class MatrixRoomMessageImage : MatrixRoomMessageBase
    {
        public MatrixRoomMessageImage()
            : base()
        {
            base.MessageType = "m.image";
        }

        [DataMember(Name = "body", IsRequired = true)]
        public string Description { get; set; }

        [DataMember(Name = "url", IsRequired = false)]
        public string ImageUrl { get; set; }
    }
}
