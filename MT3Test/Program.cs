using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MassTransit;
using MassTransit.Log4NetIntegration.Logging;
using MassTransit.Pipeline;
using Contracts;

namespace MT3Test
{
   
    class Program
      {
        
        
        static void Main(string[] args)
            {             
                Log4NetLogger.Use();
                
                var bus = Bus.Factory.CreateUsingRabbitMq(x =>
                {
                    //x.UseConcurrencyLimit(1);
                    var host = x.Host(new Uri("rabbitmq://10.120.244.161/TPD"), h => 
                    { 
                        h.Username(@"TPD"); 
                        h.Password(@"TPDUser"); 
                    });
                   
                    x.ReceiveEndpoint(host,"Scott", e =>
                        
                    e.Consumer<SomethingHappenedConsumer>());
                });

                //var busHandle = null;
                try
                { 
                    var busHandle = bus.Start();
                    Console.ReadKey();
                    busHandle.Stop();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    throw;
                }
            }
      }
    
    class SomethingHappenedConsumer : IConsumer<SomethingHappened>
        {
        public Task Consume(ConsumeContext<SomethingHappened> context)
            {
                Console.WriteLine("TXT: " + context.Message.What);
                //Console.Write("  SENT: " + context.Message.When);
                //Console.Write("  PROCESSED: " + DateTime.Now);
                //Console.WriteLine(" (" + System.Threading.Thread.CurrentThread.ManagedThreadId + ")");
                return Task.FromResult(0);
            }
    }
}

