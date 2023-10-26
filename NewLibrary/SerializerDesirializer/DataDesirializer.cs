using Confluent.Kafka;
using Newtonsoft.Json;
using System.Text;

namespace NewLibrary.SerializerDesirializer
{
	public class DataDesirializer<T> : IDeserializer<T>
	{
		public T Deserialize(ReadOnlySpan<byte> data, bool isNull, SerializationContext context)
		{
			var res = Encoding.UTF8.GetString(data.ToArray());
			var json = JsonConvert.DeserializeObject<T>(res);
			return json;
		}
	}
}
