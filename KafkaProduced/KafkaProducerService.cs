using Confluent.Kafka;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace KafkaProduced
{
	public class KafkaProducerService :IHostedService
	{
		private readonly ILogger<KafkaProducerService> _logger;
		private readonly IProducer<Null, AnyData> _producer;

		public KafkaProducerService(ILogger<KafkaProducerService> logger)
		{
			_logger = logger;
			var config = new ProducerConfig { 
				BootstrapServers = "localhost:9092",
				//ClientId
				//BatchSize
				
			};
			_producer = new ProducerBuilder<Null, AnyData>(config)
				.SetValueSerializer(new AnyDataSerializer())
				.Build();
		}

		public async Task StartAsync(CancellationToken ct)
		{
			Random random = new Random();
			string[] firstValues = { "first",  "second", "third", "fourth", "fifth"};
			//string[] secondValues = { "message", "error", "event", "process", "data" };

			//"demo-topic"
			string topicName = "jsonTopic";
			for(int i = 0; i < 3; i++)
			{ 
				AnyData data = new AnyData { TimeWithOffset = DateTimeOffset.Now,  Name = firstValues[random.Next(firstValues.Length)], Count=random.Next(100, 1000)};
				_logger.LogInformation($"Sending >> {data.TimeWithOffset} - {data.Name} - {data.Count}");
				await _producer.ProduceAsync(
						topicName,
						new Message<Null, AnyData> {Value= data},
						ct
					);
				await Task.Delay(1000);
			}
		}

		public Task StopAsync(CancellationToken ct)
		{
			_producer?.Dispose();
			_logger.LogInformation($"{nameof(KafkaProducerService)} stopped");
			return Task.CompletedTask;
		}

	}

	public class AnyData
	{
		public DateTimeOffset TimeWithOffset { get; set; }
		//public DateTime Time { get; set; }
		public string Name { get; set; }
		public int Count { get; set; }
	}

	public class AnyDataSerializer : Confluent.Kafka.ISerializer<AnyData>
	{
		public byte[] Serialize(AnyData data, SerializationContext context)
		{
			var obj = JsonConvert.SerializeObject(data);
			var bytes = Encoding.UTF8.GetBytes(obj);
			return bytes;
			//throw new NotImplementedException();
		}
	}

}
