using Confluent.Kafka;
using Newtonsoft.Json;
using System.Text;

namespace NewLibrary.SerializerDesirializer
{
		public class DataSerializer<T> : ISerializer<T>
		{
			public byte[] Serialize(T data, SerializationContext context)
			{
				var obj = JsonConvert.SerializeObject(data);
				var bytes = Encoding.UTF8.GetBytes(obj);
				return bytes;
			}
		}
}
