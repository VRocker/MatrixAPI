using System.Runtime.Serialization;

namespace libMatrix.Responses
{
    [DataContract]
    public class MatrixEventRoomLeft
    {
        [DataMember(Name = "state")]
        public MatrixSyncEvents State { get; set; }
    }

    [DataContract]
    public class MatrixEventRoomJoined
    {
        [DataMember(Name = "state")]
        public MatrixSyncEvents State { get; set; }
        [DataMember(Name = "timeline")]
        public MatrixSyncTimelineEvents Timeline { get; set; }
        [DataMember(Name = "account_data")]
        public MatrixSyncEvents AccountData { get; set; }
        [DataMember(Name = "ephemeral")]
        public MatrixSyncEvents Ephemeral { get; set; }
    }

    [DataContract]
    public class MatrixEventRoomInvited
    {
        [DataMember(Name = "invite_state")]
        public MatrixSyncEvents InviteState { get; set; }
    }
}
