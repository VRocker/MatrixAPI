using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;

namespace libMatrix.Helpers
{
    class JsonHelper
    {
        public static string Serialize(object instance)
        {
            using (MemoryStream _Stream = new MemoryStream())
            {
                var _Serializer = new DataContractJsonSerializer(instance.GetType());
                _Serializer.WriteObject(_Stream, instance);
                return Encoding.UTF8.GetString(_Stream.ToArray(), 0, (int)_Stream.Length);
            }
        }
    }
}
