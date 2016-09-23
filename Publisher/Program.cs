using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MassTransit;

namespace Publisher
{
    class Program
    {
        static void Main(string[] args)
        {

            var bus = Bus.Factory.CreateUsingRabbitMq(x =>
       x.Host(new Uri("rabbitmq://localhost/"), h => { }));
            var busHandle = bus.Start();
            var text = "";

            while (text != "quit")
            {
                Console.Write("Enter a message: ");
                text = Console.ReadLine();

                var message = new SomethingHappenedMessage()
                {
                    What = text,
                    When = DateTime.Now
                };
                bus.Publish<SomethingHappened>(message);
            }

            busHandle.Stop();
        }
    }
    class SomethingHappenedMessage : SomethingHappened
    {
        public string What { get; set; }
        public DateTime When { get; set; }
    }
    public interface SomethingHappened
    {
        string What { get; }
        DateTime When { get; }
    }
}
