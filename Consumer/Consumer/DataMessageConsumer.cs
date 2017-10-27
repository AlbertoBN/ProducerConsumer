using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using StackExchange.Redis;
using System.Threading;
using Messages;
using System.Diagnostics;

namespace Consumer
{
    public class DataMessageConsumer
    {
        CancellationTokenSource _cancellationTokenSource;
        ConnectionMultiplexer _redis;
        public DataMessageConsumer()
        {
            _redis = ConnectionMultiplexer.Connect("192.168.115.129");
        }

        public void RunConsumer()
        {
            _cancellationTokenSource = new CancellationTokenSource();
            Task t = Task.Factory.StartNew(() =>
            {
                while(true)
                {
                    if (_cancellationTokenSource.Token.IsCancellationRequested)
                        return;

                    IDatabase db = _redis.GetDatabase();
                    RedisValue value = db.ListRightPop("DataMessages");
                    DataMessage dataMessage = JsonConvert.DeserializeObject<DataMessage>((string)value);
                    Console.WriteLine(dataMessage.MessageId);
                }
            },_cancellationTokenSource.Token);
        }

        public void HaltConsumer()
        {
            _cancellationTokenSource.Cancel();
        }
    }
}
