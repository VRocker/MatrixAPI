using libMatrix.Responses;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace libMatrix.Helpers
{
    public class MatrixEventsItemConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return typeof(MatrixEvents).IsAssignableFrom(objectType);
        }
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            List<MatrixEvents> lst = new List<MatrixEvents>();

            if (reader.TokenType == JsonToken.StartArray)
            {
                JArray jsonArray = JArray.Load(reader);

                foreach (var item in jsonArray)
                {
                    switch (item["type"].Value<string>())
                    {
                        case "m.presence":
                            lst.Add(item.ToObject<Responses.Events.Presence>());
                            break;

                        case "m.typing":
                            lst.Add(item.ToObject<Responses.Events.Typing>());
                            break;

                        case "m.room.avatar":
                            lst.Add(item.ToObject<Responses.Events.Room.Avatar>());
                            break;

                        case "m.room.canonical_alias":
                            lst.Add(item.ToObject<Responses.Events.Room.CanonicalAlias>());
                            break;

                        case "m.room.create":
                            lst.Add(item.ToObject<Responses.Events.Room.Create>());
                            break;

                        case "m.room.guest_access":
                            lst.Add(item.ToObject<Responses.Events.Room.GuestAccess>());
                            break;

                        case "m.room.join_rules":
                            lst.Add(item.ToObject<Responses.Events.Room.JoinRules>());
                            break;

                        case "m.room.member":
                            lst.Add(item.ToObject<Responses.Events.Room.Member>());
                            break;

                        case "m.room.message":
                            lst.Add(item.ToObject<Responses.Events.Room.Message>());
                            break;

                        case "m.room.name":
                            lst.Add(item.ToObject<Responses.Events.Room.Name> ());
                            break;

                        case "m.room.topic":
                            lst.Add(item.ToObject<Responses.Events.Room.Topic>());
                            break;

                        case "m.sticker":
                            lst.Add(item.ToObject<Responses.Events.Sticker>());
                            break;

                        default:
                            Debug.WriteLine("Unknown event type: " + item["type"]);
                            lst.Add(item.ToObject<MatrixEvents>());
                            break;
                    }

                }
            }


            return lst;
            //return item.ToObject<MatrixEvents>();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }

    public class MatrixEventsRoomMessageItemConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return typeof(Responses.Events.Room.MessageContent).IsAssignableFrom(objectType);
        }
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {

            if (reader.TokenType == JsonToken.StartObject)
            {
                JObject item = JObject.Load(reader);

                switch (item["msgtype"].Value<string>())
                {
                    case "m.image":
                        return item.ToObject<Responses.Events.Room.MessageImageContent>();

                    case "m.location":
                        return item.ToObject<Responses.Events.Room.MessageLocationContent>();

                    case "m.text":
                    case "m.notice":
                    case "m.emote":
                        return item.ToObject<Responses.Events.Room.MessageContent>();

                    default:
                        Debug.WriteLine("Unknown message type: " + item["msgtype"]);
                        return item.ToObject<Responses.Events.Room.MessageContent>();
                }
            }

            return null;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
