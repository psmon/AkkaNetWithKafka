using Akka.Actor;
using AkkaDotModule.Kafka;
using AkkaNetWithKafka.Actors;
using Microsoft.Extensions.Hosting;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ConsumerSystem = AkkaNetWithKafka.Modules.ConsumerSystem;
using ProducerSystem = AkkaNetWithKafka.Modules.ProducerSystem;

namespace AkkaNetWithKafka.Services
{
    public sealed class KafkaService : IHostedService
    {
        private ActorSystem AkkaSystem;

        private ConsumerSystem KafkaConsumerSystem;

        private ProducerSystem KafkaProducerSystem;        

        public Task StartAsync(CancellationToken cancellationToken)
        {
            AkkaSystem = ActorSystem.Create("KafkaService");

            KafkaConsumerSystem = new ConsumerSystem();

            KafkaProducerSystem = new ProducerSystem();

            var consumerActor = AkkaSystem.ActorOf(Props.Create(() => new ConsumerActor()),
                    "consumerActor" /*AKKA가 인식하는 Path명*/);

            //소비자 : 복수개의 소비자 생성가능
            KafkaConsumerSystem.Start(new ConsumerAkkaOption()
            {
                KafkaGroupId = "testGroup",
                BootstrapServers = "kafka:9092",
                RelayActor = consumerActor,               //소비되는 메시지가 지정 액터로 전달
                Topics = "akka100",
            });

            //생산자 : 복수개의 생산자 생성가능
            KafkaProducerSystem.Start(new ProducerAkkaOption()
            {
                BootstrapServers = "kafka:9092",
                ProducerName = "producer1",
            });

            
            List<string> messages = new List<string>();
            for (int i = 0; i < 10; i++)
            {
                messages.Add($"message-{i}");
            }

            //보너스 : 생산의 TPS 속도를 조절할수 있습니다.
            int tps = 10;
            KafkaProducerSystem.SinkMessage("producer1", "akka100", messages, tps);


            return Task.CompletedTask;
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            // strictly speaking this may not be necessary - terminating the ActorSystem would also work
            // but this call guarantees that the shutdown of the cluster is graceful regardless
            await CoordinatedShutdown.Get(AkkaSystem).Run(CoordinatedShutdown.ClrExitReason.Instance);
        }
    }
}
