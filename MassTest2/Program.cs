using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MassTransit;
using SharedContracts;

namespace MassTest2
{
    internal class Program
    {
        private static List<IServiceBus> _serviceBusses;

        private static void Main(string[] args)
        {
            _serviceBusses = new List<IServiceBus>();
            foreach (var i in Enumerable.Range(0, 10))
            {
                _serviceBusses.Add(ServiceBusFactory.New(sbc =>
                {
                    var uri = "rabbitmq://localhost/" + "Mass2-" + Guid.NewGuid().ToString();
                    sbc.UseRabbitMq(
                        rbtConfig => rbtConfig.ConfigureHost(new Uri(uri),
                            h =>
                            {
                                h.SetUsername("admin");
                                h.SetPassword("admin");
                            }));

                    sbc.ReceiveFrom(uri);
                    sbc.UseJsonSerializer();
                    sbc.SetConcurrentConsumerLimit(4);
                    sbc.Subscribe(x => { x.Consumer(() => new TestConsumer()); });
                }));
            }

            foreach (var i in Enumerable.Range(0, 1))
            {
                Task.Factory.StartNew(() =>
                {
                    var counter = 0;
                    while (true)
                    {
                        foreach (var serviceBus in _serviceBusses)
                        {
                            serviceBus.Publish(new TestMessage {GUID = counter.ToString(), From = "Mass2"});
                            counter++;
                            Thread.Sleep(100);
                        }
                    }
                });
            }


            Console.ReadLine();
            Cleanup();
        }

        public static void Cleanup()
        {
            foreach (var serviceBus in _serviceBusses)
            {
                serviceBus.Dispose();
            }
        }
    }
}