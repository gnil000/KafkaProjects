using Confluent.Kafka;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Text;

namespace KafkaConsumer3
{
	public class KafkaConsumer : IHostedService
	{
		private readonly IConsumer<Null, AnyData> _consumer;
		private string topicName = "jsonTopic";

		public KafkaConsumer(ILogger<KafkaConsumer> logger)
		{
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
			_consumer.Subscribe(topicName);
			while (!cancellationToken.IsCancellationRequested)
			{
				await Task.Yield();
				var consumeResult = _consumer.Consume(TimeSpan.FromSeconds(1));

				if (consumeResult != null)
				{
					var sendTime = consumeResult.Message.Timestamp.UtcDateTime.ToLocalTime();
					var currentTime = DateTime.Now;

					TimeSpan interval = currentTime - sendTime;

					Console.WriteLine(interval.Seconds);

					if (interval <= TimeSpan.FromMinutes(5))
					{
						Console.WriteLine($"{consumeResult.Message.Value.Name} {consumeResult.Message.Value.TimeWithOffset} ||| {sendTime} {currentTime}");
						Console.Beep();
					}
					else
					{
						Console.WriteLine("Не уложено во время");
						Console.Beep();
;					}
				}
			}
		}

		public Task StopAsync(CancellationToken cancellationToken)
		{
			_consumer?.Dispose();
			return Task.CompletedTask;
		}
	}
}

public class AnyData
{
	public DateTimeOffset TimeWithOffset { get; set; }
	public string Name { get; set; }
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
