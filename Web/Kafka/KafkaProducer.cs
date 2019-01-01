using System.Collections.Generic;
using Confluent.Kafka;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Web.Kafka {
    public class KafkaProducer : Producer{
        private ProducerConfig Config { get; }
        private string Topic { get; }
        private JsonSerializerSettings JsonSettings { get; }

        public KafkaProducer(ProducerConfig config, string topic) {
            Config = config;
            Topic = topic;
            JsonSettings = new JsonSerializerSettings {
                ContractResolver = new DefaultContractResolver {
                    NamingStrategy = new CamelCaseNamingStrategy()
                },
                Formatting = Formatting.Indented
            };
        }

        public bool Publish<T>(T t) {
            var message = JsonConvert.SerializeObject(t, JsonSettings);
            using (var producer = new Producer<Null, string>(Config)) {
                var deliveryReport = producer.ProduceAsync(Topic, new Message<Null, string>{Value = message}).Result;
                return true;
            }
        }

        public bool BulkPublish<T>(List<T> list) {
            using (var producer = new Producer<Null, string>(Config)) {
                list.ForEach(x => {
                    var message = JsonConvert.SerializeObject(x, JsonSettings);
                    producer.BeginProduce(Topic, new Message<Null, string> {Value = message});
                });
                return true;
            }
        }
    }
}