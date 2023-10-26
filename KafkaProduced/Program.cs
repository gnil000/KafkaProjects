class Program {

	static async Task Main(string[] args)
	{
		NewLibrary.Kafka.KafkaProducer producer = new NewLibrary.Kafka.KafkaProducer("jsonTopic");
		await producer.RunProducerAsync();
	}
}