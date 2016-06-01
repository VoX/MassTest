using System;
using MassTransit;
using SharedContracts;

namespace MassTest2
{
    public class TestConsumer : Consumes<TestMessage>.All
    {
        public void Consume(TestMessage message)
        {
            Console.WriteLine("Received:" + message.GUID + " From:" + message.From);
        }
    }
}