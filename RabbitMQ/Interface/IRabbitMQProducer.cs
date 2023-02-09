namespace EmsiSoft.RabbitMQ.Interface
{
    public interface IRabbitMQProducer
    {
        public void SendProductMessage<T>(T message);
    }
}
