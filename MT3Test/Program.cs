using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MassTransit;
using MassTransit.Pipeline;

namespace MT3Test
{
    class Program
    {
        static void Main(string[] args)
        {
            IBusControl bus = Bus.Factory.CreateUsingRabbitMq(sbc =>
            {
                try
                {

                
                var host = sbc.Host(new Uri("rabbitmq://localhost/vhost/"), h => {});

                sbc.UseRetry(Retry.Immediate(5));

                 sbc.ReceiveEndpoint(host, "test_queue", ep =>
                    {
                        
                        ep.Consumer<SomethingCameIn>();
                        
                        //ep.Handler<TestMessage>(context =>
                        //{
                        //    return Console.Out.WriteLineAsync("Received: {context.Message.sData}");
                        //});
                    });

                sbc.PurgeOnStartup = true;

                //sbc.ReceiveEndpoint(host, "Test_queue", ep =>
                //{
                //    ep.Consumer<MyConsumer>();
                //});
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                    Console.ReadLine();
                    throw;
                    throw;
                }
            });

            try
            {
                bus.Start();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                Console.ReadLine();
                throw;
            }

            
            var text = "";


            PublishMessage(bus)
             .Wait();
            
            Console.ReadKey();

            bus.Stop();
        }


       

        public static Task PublishMessage(IBus bus)
        {
            return bus.Publish(new TestMessageMessage { sData = "Hi" });
        }
    }

    class SomethingCameIn : IConsumer<TestMessage>
    {
        public Task Consume(ConsumeContext<TestMessage> context)
        {
            Console.Write("TXT: " + context);
            //Console.Write("  SENT: " + context.Message.When);
            //Console.Write("  PROCESSED: " + DateTime.Now);
            //Console.WriteLine(" (" + System.Threading.Thread.CurrentThread.ManagedThreadId + ")");
            return Task.FromResult(0);
        }
    }
   
    public interface TestMessage
    {
        string sData { get; set; }
    }
    class TestMessageMessage : TestMessage
    {
        public string sData { get; set; }
       
    }
    //class TestMessage //: IConsumer
    //{
    //    public string sData { get; set; }
    //}

    
}
