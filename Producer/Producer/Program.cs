using Messages;
using System;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using StackExchange.Redis;
namespace Producer
{

    class Program
    {

        static void Main(string[] args)
        {
            CancellationTokenSource src = new CancellationTokenSource();

            Task t = Task.Factory.StartNew(() =>
            {
                ConnectionMultiplexer redis = ConnectionMultiplexer.Connect("192.168.115.129");

                Random rand = new Random((int)DateTime.Now.Ticks);
                
                while (true)
                {
                    if (src.Token.IsCancellationRequested)
                        return;

                    DataMessage msg = new DataMessage();
                    msg.MessageId = Guid.NewGuid();
                    msg.MessageData = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut ";
                    msg.MessageNumber = rand.Next(int.MinValue,int.MaxValue);
                    string jsonMessage = JsonConvert.SerializeObject(msg);

                    IDatabase db = redis.GetDatabase();
                    db.ListLeftPush("DataMessages", jsonMessage, When.Always, CommandFlags.HighPriority);

        //            Thread.Sleep(10);
                }
            },src.Token);

            Console.WriteLine("Press any key to close the app...");
            Console.ReadKey();

            src.Cancel();
        
        }
    }
}
