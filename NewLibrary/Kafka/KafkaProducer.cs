using Confluent.Kafka;
using NewLibrary.Models;
using NewLibrary.SerializerDesirializer;

namespace NewLibrary.Kafka
{
    public class KafkaProducer
    {
        private readonly IProducer<Null, AnyData> _producer;
        public readonly string TopicName;

        public KafkaProducer(string topicName)
        {
            TopicName = topicName;
            var config = new ProducerConfig
            {
                BootstrapServers = "localhost:9092",
            };
            _producer = new ProducerBuilder<Null, AnyData>(config)
                .SetValueSerializer(new DataSerializer<AnyData>())
                .Build();
        }

        public async Task StartProducerAsync()
        {
            await StartAsync();
            Console.ReadKey();
        }

        public async Task StartAsync()
        {
            Random random = new Random();
            var data = RandomStringGenerator.RandomGenerator(5, 5, 10);

            string topicName = TopicName;
            for (int i = 0; i < data.Count; i++)
            {
                AnyData sendData = new AnyData { RandomTime = new TimeSpan(random.Next(0, 13), random.Next(0, 60), random.Next(0, 60)), RandomValue = data[i], Count = random.Next(100, 1000) };
                Console.WriteLine($"Sending >> {sendData.ToString()}");
                await _producer.ProduceAsync(
                        topicName,
                        new Message<Null, AnyData> { Value = sendData }
                    );
                await Task.Delay(1000);
            }
        }

        public Task StopAsync()
        {
            _producer?.Dispose();
            Console.WriteLine($"{nameof(KafkaProducer)} stopped");
            return Task.CompletedTask;
        }

    }
}
