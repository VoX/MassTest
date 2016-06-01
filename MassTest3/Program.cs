using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MassTransit;
using SharedContracts;

namespace MassTest3
{
    internal class Program
    {
        private static List<IBus> _serviceBusses;

        private static void Main(string[] args)
        {
            _serviceBusses = new List<IBus>();
            foreach (var i in Enumerable.Range(0, 10))
            {
                var bus = Bus.Factory.CreateUsingRabbitMq(sbc =>
                {
                    var host = sbc.Host(new Uri("rabbitmq://localhost/"), configurator =>
                    {
                        configurator.Username("admin");
                        configurator.Password("admin");
                    });
                    sbc.UseJsonSerializer();
                    sbc.ReceiveEndpoint(host, "Mass3-" + Guid.NewGuid().ToString(), configurator =>
                    {
                        configurator.Exclusive = true;
                        configurator.PrefetchCount = 4;
                        configurator.Consumer(() => new TestConsumer());
                    });
                });
                bus.Start();
                _serviceBusses.Add(bus);
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
                            serviceBus.Publish(new TestMessage {GUID = counter.ToString(), From = "Mass3"});
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
                ((IBusControl) serviceBus).Stop();
            }
        }
    }
}