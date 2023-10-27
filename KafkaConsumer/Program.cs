
class Program
{
	static async Task Main(string[] args)
	{
		NewLibrary.Kafka.KafkaConsumer consumer = new NewLibrary.Kafka.KafkaConsumer("BaseTopic");
		await consumer.StartAsync();
	}
}