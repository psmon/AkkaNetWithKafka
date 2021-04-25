# AkkaDotNetWithKafka

## 이저장소의 목적

닷넷코어 콘솔에서 Akka를 통해 Kafka를 심플하게 사용할수 있는

샘플 HeadLessService 를 구성해보았습니다.

# 컨셉


![alt text](http://wiki.webnori.com/download/attachments/51937532/image2021-4-25_12-20-44.png?version=1&modificationDate=1619320844997&api=v2)


Pub/Sub 컨셉을 알고 있으면, Kafka의 생산과 소비를 심플한 코드로 활용할수 있게

구성하였습니다.

# AppLayOut


- Modules
  - ProducerSystem : Kafka의 생산기가 Akka의 Stream기와 연결
  - ConsumerSystem : Kafka의 소비기가 Akka의 Stream기와 연결
- Services
  - KafkaService : Kafka와 ActorSystem을 초기화하고 실행하는 Headless 서비스
- Actos
  - ConsumerActor : Kafka의 특정 토픽 소비처리를 하는 액터
- Config
  - akka.kafka.conf : KafkaClient의 시스템 셋팅

# 기본 인프라 구동

- docker-compose up -d : StandAlone Kafka를 구동합니다.
- kafka 로컬 클러스터 사용을 위해,호스트 파일을 수정하여 kafka 127.0.0.1 을 설정해주세요

# 이 프로젝트의 문서
- http://wiki.webnori.com/display/webfr/AkkaNetWithKafka - 이 샘플코드를 설명하는 문서
- https://github.com/akkadotnet/Akka.Streams.Kafka - AkkaStream을통해 Kafka를 더 고급적으로 사용할때 참고
