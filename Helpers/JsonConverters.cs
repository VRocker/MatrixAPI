using libMatrix.Responses;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
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

                        default:
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
}
