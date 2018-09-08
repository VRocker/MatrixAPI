using System.Runtime.Serialization;

namespace libMatrix.Responses.Events.Room
{
    [DataContract]
    public class Create : MatrixEvents
    {
        [DataMember(Name = "content")]
        public CreateContent Content { get; set; }
    }

    [DataContract]
    public class CreateContent
    {
        [DataMember(Name = "creator")]
        public string Creator { get; set; }
    }
}
