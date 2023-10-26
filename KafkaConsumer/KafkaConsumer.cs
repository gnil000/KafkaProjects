using Confluent.Kafka;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Text;


namespace KafkaConsumer
{
	public class KafkaConsumer
	{
		public class KafkaConsumerService : IHostedService
		{
			private readonly ILogger<KafkaConsumerService> _logger;
			private readonly IConsumer<Null, AnyData> _consumer;
			private string topicName = "jsonTopic";

			public KafkaConsumerService(ILogger<KafkaConsumerService> logger)
			{
				_logger = logger;
				var config = new ConsumerConfig
				{
					BootstrapServers = "localhost:9092",
					GroupId = "demo-group",
					AutoOffsetReset = AutoOffsetReset.Earliest,
				};
				_consumer = new ConsumerBuilder<Null, AnyData>(config)
					.SetValueDeserializer(new AnyDataDesirializer())
					.Build();
			}

			public async Task StartAsync(CancellationToken cancellationToken)
			{
				await Task.Yield();
				_consumer.Subscribe(topicName);
				while (!cancellationToken.IsCancellationRequested)
				{
					var consumeResult = _consumer.Consume(cancellationToken);
					var result = consumeResult.Message.Value;
					_logger.LogInformation($"Received >>{result.RandomTime} - {result.RandomValue} - {result.Count}");
				}
			}

			public Task StopAsync(CancellationToken cancellationToken)
			{
				_consumer?.Dispose();
				_logger.LogInformation($"{nameof(KafkaConsumerService)} stopped");
				return Task.CompletedTask;
			}
		}
	}

	public class AnyData
	{
		public TimeSpan RandomTime { get; set; }
		public string RandomValue { get; set; }
		public int Count { get; set; }
	}

	public class AnyDataDesirializer : Confluent.Kafka.IDeserializer<AnyData>
	{
		public AnyData Deserialize(ReadOnlySpan<byte> data, bool isNull, SerializationContext context)
		{
			var res = Encoding.UTF8.GetString(data.ToArray());
			var json = JsonConvert.DeserializeObject<AnyData>(res);
			return json;
		}
	}

}
