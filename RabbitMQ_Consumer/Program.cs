using EasyNetQ;
using EasyNetQHelper;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQ_Consumer
{
    class Program
    {
        static void Main(string[] args)
        {
            Consumer();
           // EasyNetQConsumer();
        }

        public static void Consumer()
        {
            try
            {
                var qName = "lhtest1";
                var exchangeName = "directexchange1";
                var exchangeType = "direct";//topic、fanout
                var routingKey = "*";
                var uri = new Uri("amqp://localhost:5672");
                var factory = new ConnectionFactory
                {
                    //HostName = "localhost"
                    UserName = "llj",
                    Password = "llj2016",
                    RequestedHeartbeat = 0,
                    Endpoint = new AmqpTcpEndpoint(uri)
                };
                using (var connection = factory.CreateConnection())
                {
                    using (var channel = connection.CreateModel())
                    {
                        //设置交换器的类型
                        channel.ExchangeDeclare(exchangeName, exchangeType);
                        //声明一个队列，设置队列是否持久化，排他性，与自动删除
                        channel.QueueDeclare(qName, true, false, false, null);
                        //绑定消息队列，交换器，routingkey
                        channel.QueueBind(qName, exchangeName, routingKey);
                        var properties = channel.CreateBasicProperties();
                        QueueingBasicConsumer consumer = new QueueingBasicConsumer();
                        channel.BasicConsume(qName, false, consumer);
                        while (true)
                        {
                            var ea = (BasicDeliverEventArgs)consumer.Queue.Dequeue();

                            var body = ea.Body;
                            var message = Encoding.UTF8.GetString(body);
                            Console.WriteLine("Received : {0}", message);
                            channel.BasicAck(ea.DeliveryTag, false);//回复确认
                        }

                       
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// /// <summary>
        /// EasyNetQ封装方法
        /// </summary>
        /// </summary>
        public static void EasyNetQConsumer()
        {
            try
            {
                ProcessMessage order = new ProcessMessage();

                Message msg = new Message();
                msg.MessageID = "1";
                msg.MessageRouter = "llj";

                MQHelper.Subscribe(msg, order);

            }
            catch (EasyNetQException ex)
            {
                //处理连接消息服务器异常 
                // MessageHelper.WriteFuntionExceptionLog("Publish", ex.Message + " | " + ex.StackTrace);

            }

        }
    }
}
