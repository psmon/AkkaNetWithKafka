using Akka.Actor;
using Akka.Event;

namespace AkkaNetWithKafka.Actors
{

    public class ConsumerActor : ReceiveActor
    {
        private readonly ILoggingAdapter logger = Context.GetLogger();

        public ConsumerActor()
        {
            ReceiveAsync<string>(async strData =>
            {                
                logger.Info("ConsumerActor IncomeMessage:" + strData);
                //TODO : Somthing
            });
        }
    }
}
