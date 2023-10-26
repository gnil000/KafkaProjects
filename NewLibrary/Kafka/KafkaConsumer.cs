﻿using Confluent.Kafka;
using NewLibrary.Models;
using NewLibrary.SerializerDesirializer;
using System.Threading.Channels;

namespace NewLibrary.Kafka
{
	public class KafkaConsumer
	{
		private readonly IConsumer<Null, AnyData> _consumer;
		private string TopicName;

		public KafkaConsumer(string topicName)
		{
			TopicName= topicName;
			var config = new ConsumerConfig
			{
				BootstrapServers = "localhost:9092",
				GroupId = "demo-group",
				AutoOffsetReset = AutoOffsetReset.Earliest,
			};
			_consumer = new ConsumerBuilder<Null, AnyData>(config)
				.SetValueDeserializer(new DataDesirializer<AnyData>())
				.Build();
		}

		//public async Task StartConsumerAsync()
		//{
		//	await StartAsync();
		//	Console.ReadKey();
		//}

		public async Task StartDeferredProcessingAsync()
		{
			//await Task.Yield();

			var channel = Channel.CreateBounded<ConsumeResult<Null, AnyData>>(new BoundedChannelOptions(100));
			var reader = channel.Reader;
			var writer = channel.Writer;

			var readProces = Task.Factory.StartNew(async () =>
			{
				while (!reader.Completion.IsCompleted)
				{
					var r = await reader.ReadAsync();
					var inter = DateTime.Now - r.Message.Timestamp.UtcDateTime.ToLocalTime();
					if (inter < TimeSpan.FromMinutes(1))
					{
						await Task.Delay((int)(TimeSpan.FromMinutes(1) - inter).TotalMilliseconds);
						Console.WriteLine(inter);
					}
					var result = r.Message.Value;
					Console.WriteLine($"{r.Message.Value.ToString()} ||| {r.Message.Timestamp.UtcDateTime.ToLocalTime()} {DateTime.Now.ToString("HH:mm:ss")}");
				}
			});

			_consumer.Subscribe(TopicName);
			while (true)
			{
				var consumeResult = _consumer.Consume();
				if (consumeResult != null)
				{
					var sendTime = consumeResult.Message.Timestamp.UtcDateTime.ToLocalTime();
					var currentTime = DateTime.Now;
					TimeSpan interval = currentTime - sendTime;
					Console.WriteLine(interval.TotalMinutes);
					if (interval >= TimeSpan.FromMinutes(1))
					{
						Console.WriteLine($"{consumeResult.Message.Value.ToString()} ||| {sendTime} {currentTime}");
						Console.Beep();
					}
					else
					{
						Console.WriteLine("Не уложено во время");
						await writer.WriteAsync(consumeResult);
					}
				}
				await readProces;
			}
		}

		public async Task StartAsync()
		{
			await Task.Yield();
			_consumer.Subscribe(TopicName);
			while (true)
			{
				var consumeResult = _consumer.Consume();
				var result = consumeResult.Message.Value;
				Console.WriteLine($"Received >> {result.ToString()}");
			}
		}

		public Task StopAsync()
		{
			_consumer?.Dispose();
			Console.WriteLine($"{nameof(KafkaConsumer)} stopped");
			return Task.CompletedTask;
		}
	}
}

