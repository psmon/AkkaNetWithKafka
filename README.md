# AkkaDotNetWithKafka

## 이저장소의 목적

닷넷코어 콘솔에서 Akka를 통해 Kafka를 심플하게 사용할수 있는

샘플 HeadLessService 를 구성해보았습니다.

# AppLayOut

- KafkaService : Kafka와 ActorSystem을 초기화하고 실행하는 Headless 서비스
- ConsumerActor : Kafka의 특정 토픽 소비처리를 위해 액터에게 메시지 전달

# 기본 인프라 구동

- docker-compose up -d : StandAlone Kafka를 구동합니다.
- kafka 로컬 클러스터 사용을 위해,호스트 파일을 수정하여 kafka 127.0.0.1 을 설정해주세요

# 이 프로젝트의 문서
- http://wiki.webnori.com/display/webfr/AkkaNetWithKafka
