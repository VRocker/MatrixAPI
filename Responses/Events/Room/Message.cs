using libMatrix.Helpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace libMatrix.Responses.Events.Room
{
    [DataContract]
    public class Message : MatrixEvents
    {
        [DataMember(Name = "content")]
        [JsonConverter(typeof(MatrixEventsRoomMessageItemConverter))]
        public MessageContent Content { get; set; }
    }

    [DataContract]
    public class MessageContent
    {
        [DataMember(Name = "body")]
        public string Body { get; set; }
        [DataMember(Name = "msgtype")]
        public string MessageType { get; set; }
    }

    [DataContract]
    public class MessageImageContent : MessageContent
    {
        [DataMember(Name = "info")]
        public APITypes.MatrixContentThumbnailInfo ImageInfo { get; set; }
        [DataMember(Name = "url")]
        public string Url { get; set; }
    }
}
