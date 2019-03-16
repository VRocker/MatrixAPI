using System.Runtime.Serialization;

namespace libMatrix.Responses.Events.Room
{
    [DataContract]
    public class Topic : MatrixEvents
    {
        [DataMember(Name = "content")]
        public TopicContent Content { get; set; }
    }

    [DataContract]
    public class TopicContent
    {
        [DataMember(Name = "topic")]
        public string Topic { get; set; }
    }
}
