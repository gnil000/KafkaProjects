using NewLibrary.Kafka;

KafkaConsumer consumer = new KafkaConsumer("jsonTopic");
await consumer.StartDeferredProcessingAsync();