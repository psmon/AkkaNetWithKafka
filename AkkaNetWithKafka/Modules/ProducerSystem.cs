using Akka;
using Akka.Actor;
using Akka.Configuration;
using Akka.Streams;
using Akka.Streams.Dsl;
using Akka.Streams.Kafka.Dsl;
using Akka.Streams.Kafka.Messages;
using Akka.Streams.Kafka.Settings;
using AkkaDotModule.Kafka;
using Confluent.Kafka;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AkkaNetWithKafka.Modules
{
    public class ProducerSystem
    {
        private ActorSystem producerSystem;

        private ActorMaterializer materializer_producer;

        private Dictionary<string, ProducerSettings<Null, string>> producerList = new Dictionary<string, ProducerSettings<Null, string>>();


        public ProducerSystem()
        {
            string configText = File.ReadAllText("akka.kafka.conf");
            var config = ConfigurationFactory.ParseString(configText);
            producerSystem = ActorSystem.Create("producerSystem", config);
        }

        public void Start(ProducerAkkaOption producerAkkaOption)
        {
            materializer_producer = producerSystem.Materializer();

            var producer = ProducerSettings<Null, string>.Create(producerSystem, null, null)
                .WithBootstrapServers(producerAkkaOption.BootstrapServers);

            producerList[producerAkkaOption.ProducerName] = producer;
        }

        public void SinkMessage(string producerName, string topic, List<string> message, int tps)
        {
            int maxBuster = 1;  //메시지가 해당값에 도달하면, TPS 제약기가 발동합니다.
            ProducerSettings<Null, string> producerSettings = producerList[producerName];
            Source<string, NotUsed> source = Source.From(message);
            source
            .Select(c =>
            {
                return c;
            })            
            .Select(elem => ProducerMessage.Single(new ProducerRecord<Null, string>(topic, elem)))
            .Throttle(tps, TimeSpan.FromSeconds(1), maxBuster, ThrottleMode.Shaping)      //TPS
            .Via(KafkaProducer.FlexiFlow<Null, string, NotUsed>(producerSettings))            
            .Select(result =>
            {
                var response = result as Result<Null, string, NotUsed>;
                Console.WriteLine($"[{DateTime.Now}] Producer: {response.Metadata.Topic}/{response.Metadata.Partition} {response.Metadata.Offset}: {response.Metadata.Value}");
                return result;
            })
            .RunWith(Sink.Ignore<IResults<Null, string, NotUsed>>(), materializer_producer);
        }

    }
}
