using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Contracts;
using MassTransit;
using Contracts;

namespace TestPublisher
{
    class Program
    {
        const string Server_IP = "10.120.244.161";
        const string Server_Vhost = "TPD";
        const string Username = "TPD";
        const string Password = "TPDUser";
        class SomethingHappenedMessage : SomethingHappened
        {
            public string What { get; set; }
            public DateTime When { get; set; }
        }
        static void Main(string[] args)
        {
            var bus = Bus.Factory.CreateUsingRabbitMq(x =>
                {
                    //x.UseConcurrencyLimit(1);
                    string _Uri = "rabbitmq://" + Server_IP + "/" + Server_Vhost ;                    
                    var host = x.Host(new Uri(_Uri), h => 
                    {
                        // The Guest account does not have access to any vhost if it does
                        //  not come from localhost.  So you must setup access through 
                        //  the Rabbitmq admin tool on the server.

                        h.Username(Username);
                        h.Password(Password);                   
                    });
                });

            var busHandle = bus.Start();
           
            var text = "";

            


            while (true)
            {



                Console.Write("Enter number of messages to send: ");
                text = Console.ReadLine();
                int iCount = Convert.ToInt32(text);
                string _message = string.Empty;

                for (int i = 0; i < iCount; i++)
                {
                    _message = i.ToString() + ": " + "This is a test of the Enterprise Service Bus.";
                    var message = new SomethingHappenedMessage()
                    {
                        What = _message,
                        When = DateTime.Now
                    };
                    try
                    {
                       
                        bus.Publish<SomethingHappened>(message);
                        
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        Console.ReadLine();

                        throw;
                    }
                    
                }
                //_message = "Done.";
                //var mess = new SomethingHappenedMessage()
                //{
                //    What = _message,
                //    When = DateTime.Now
                //};
                //bus.Publish<SomethingHappened>(mess);
            }
            System.Threading.Thread.Sleep(5000);


            busHandle.Stop();
        }
       
    }
    class SomethingHappenedConsumer : IConsumer<SomethingHappened>
    {
        public Task Consume(ConsumeContext<SomethingHappened> context)
        {
            Console.Write("TXT: " + context.Message.What);
            Console.Write("  SENT: " + context.Message.When);
            Console.Write("  PROCESSED: " + DateTime.Now);
            Console.WriteLine(" (" + System.Threading.Thread.CurrentThread.ManagedThreadId + ")");
            return Task.FromResult(0);
        }
    }
    class SomethingHappenedConsumer1 : IConsumer<SomethingHappened>
    {
        public Task Consume(ConsumeContext<SomethingHappened> context)
        {
            Console.Write("TXT: " + context.Message.What);
            Console.Write("  SENT: " + context.Message.When);
            Console.Write("  PROCESSED: " + DateTime.Now);
            Console.WriteLine(" (" + System.Threading.Thread.CurrentThread.ManagedThreadId + ")");
            return Task.FromResult(0);
        }
    }
}
