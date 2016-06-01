using System;
using System.Threading.Tasks;
using MassTransit;
using SharedContracts;

namespace MassTest3
{
    public class TestConsumer : IConsumer<TestMessage>
    {
        public Task Consume(ConsumeContext<TestMessage> context)
        {
            Console.WriteLine("Received:" + context.Message.GUID + " From:" + context.Message.From);
            return Task.FromResult(true);
        }
    }
}