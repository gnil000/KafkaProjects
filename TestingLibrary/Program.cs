using NewLibrary.Kafka;

KafkaProducer producer = new KafkaProducer("jsonTopic");
await producer.StartProducerAsync();
