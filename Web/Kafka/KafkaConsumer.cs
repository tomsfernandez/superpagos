using System;
using System.Collections.Generic;
using Confluent.Kafka;

namespace Web.Kafka {
    public class KafkaConsumer : Consumer<string>{

        private ConsumerConfig Config { get; }
        
        public KafkaConsumer(ConsumerConfig config) {
            Config = config;
        }

        public void StartConsuming(List<string> topics, Action<string> onMessageAction, Func<bool> stopConsuming) {
            using (var c = new Consumer<string, string>(Config)) {
                c.Subscribe(topics);
                while (!stopConsuming()) {
                    try {
                        var cr = c.Consume();
                        onMessageAction(cr.Value);
                    }
                    catch (ConsumeException e) {
                        Console.WriteLine($"Error occured: {e.Error.Reason}");
                    }
                }
                c.Close();
            }
        }
    }
}