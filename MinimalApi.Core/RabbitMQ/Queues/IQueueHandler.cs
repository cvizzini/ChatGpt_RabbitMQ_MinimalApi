namespace MinimalApi.Core.RabbitMQ.Queues;

public interface IQueueHandler : IDisposable
{
    public string ApplicableQueue { get; }
   public Task Consume();
}