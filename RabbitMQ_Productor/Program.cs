using EasyNetQ;
using EasyNetQHelper;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQ_Productor
{
    class Program
    {
        static void Main(string[] args)
        {
           // Console.WriteLine("123");
            Producer();
           // EasyNetQProducer();

        }

        public static void Producer()
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
                        //队列持久化
                        channel.QueueDeclare(qName, true, false, false, null);
                        //绑定消息队列，交换器，routingkey
                        channel.QueueBind(qName, exchangeName, routingKey);
                        while (true)
                        {
                            var value = Console.ReadLine();
                            var properties = channel.CreateBasicProperties();
                            //消息持久化
                            properties.Persistent = true;
                            var message = DateTime.Now.ToShortTimeString() + ":" + value;
                            var body = Encoding.UTF8.GetBytes(message);
                            //发送信息
                            channel.BasicPublish(exchangeName, routingKey, properties, body);
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
        /// EasyNetQ封装方法
        /// </summary>
        public static void EasyNetQProducer()
        {
            try
            {
                while (true)
                {
                    var value = Console.ReadLine();

                    Message msg = new Message();
                    msg.MessageID = "1";
                    msg.MessageBody = DateTime.Now.ToString() + ":" + value;
                    msg.MessageTitle = "1";
                    msg.MessageRouter = "llj";
                    MQHelper.Publish(msg);

                }
            }
            catch (EasyNetQException ex)
            {
                //处理连接消息服务器异常 
                // MessageHelper.WriteFuntionExceptionLog("Publish", ex.Message + " | " + ex.StackTrace);

            }

        }
    }
}
