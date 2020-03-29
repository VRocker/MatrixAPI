using System.Runtime.Serialization;

namespace libMatrix.Requests.Pushers
{
    [DataContract]
    public class MatrixSetPusher
    {
        [DataMember(Name = "pushkey")]
        public string PushKey { get; set; }
        [DataMember(Name = "kind")]
        public string Kind { get; set; }
        [DataMember(Name = "app_id")]
        public string AppID { get; set; }
        [DataMember(Name = "app_display_name")]
        public string AppDisplayName { get; set; }
        [DataMember(Name = "device_display_name")]
        public string DeviceDisplayName { get; set; }
        [DataMember(Name = "profile_tag", EmitDefaultValue = false, IsRequired = false)]
        public string ProfileTag { get; set; }
        [DataMember(Name = "lang")]
        public string Language { get; set; }
        [DataMember(Name = "data")]
        public MatrixPusherData Data { get; set; }
        [DataMember(Name = "append", EmitDefaultValue = false, IsRequired = false)]
        public bool Append { get; set; }
    }

    [DataContract]
    public class MatrixPusherData
    {
        [DataMember(Name = "url")]
        public string Url { get; set; }
        [DataMember(Name = "format")]
        public string Format { get; set; } = "event_id_only";
    }
}
