using Newtonsoft.Json;
using System;
using System.Runtime.Serialization;

namespace libMatrix.Responses
{
    [DataContract]
    public class MatrixEvents
    {
        [DataMember(Name = "origin_server_ts")]
        public long OriginServerTimestamp { get; set; }
        [DataMember(Name = "age")]
        public long Age { get; set; }
        [DataMember(Name = "sender")]
        public string Sender { get; set; }
        [DataMember(Name = "type", IsRequired = true)]
        public string Type { get; set; }
        [DataMember(Name = "event_id")]
        public string EventID { get; set; }
        [DataMember(Name = "room_id")]
        public string RoomID { get; set; }
        [DataMember(Name = "unsigned")]
        public MatrixEventsUnsigned Unsigned { get; set; }
        [DataMember(Name = "state_key")]
        public string StateKey { get; set; }
    }

    [DataContract]
    public class MatrixEventsUnsigned
    {
        [DataMember(Name = "prev_content")]
        public MatrixEventsUnsigned PrevContent;
        [DataMember(Name = "age")]
        public long Age;
        [DataMember(Name = "transaction_id")]
        public string TransactionID;
    }
}
