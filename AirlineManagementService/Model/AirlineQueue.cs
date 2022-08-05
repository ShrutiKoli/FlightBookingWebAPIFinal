using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace AirlineManagementService.Model
{
    public static class AirlineQueue
    {
        public static IConnection GetConnection()
        {
            ConnectionFactory factory = new ConnectionFactory();
            factory.UserName = "guest";
            factory.Password = "guest";
            factory.Port = 5672;
            factory.HostName = "localhost";
            factory.VirtualHost = "/";

            return factory.CreateConnection();
        }
        public static async Task<bool> send(IConnection con, string message, string friendqueue)
        {
            try
            {
                IModel channel = con.CreateModel();
                channel.ExchangeDeclare("messageexchange", ExchangeType.Direct);
                channel.QueueDeclare(friendqueue, true, false, false, null);
                channel.QueueBind(friendqueue, "messageexchange", friendqueue, null);
                var msg = Encoding.UTF8.GetBytes(message);
                channel.BasicPublish("messageexchange", friendqueue, null, msg);

            }
            catch (Exception)
            {


            }
            return true;

        }
        public static async Task<bool> sendSharedData(IConnection con, List<FlightScheduleDetail> message, string friendqueue)
        {
            try
            {
                IModel channel = con.CreateModel();
                channel.ExchangeDeclare("messageexchange", ExchangeType.Direct);
                channel.QueueDeclare(friendqueue, true, false, false, null);
                channel.QueueBind(friendqueue, "messageexchange", friendqueue, null);
                string obj=JsonConvert.SerializeObject(message);
                var msg = Encoding.UTF8.GetBytes(obj);
                channel.BasicPublish("messageexchange", friendqueue, null, msg);

            }
            catch (Exception)
            {


            }
            return true;

        }
        public static async Task<string> receive(IConnection con, string myqueue)
        {
            try
            {
                string queue = myqueue;
                IModel channel = con.CreateModel();
                channel.QueueDeclare(queue: queue, durable: true, exclusive: false, autoDelete: false, arguments: null);
                var consumer = new EventingBasicConsumer(channel);
                BasicGetResult result = channel.BasicGet(queue: queue, autoAck: true);
                
                if (result != null)
                    return Encoding.UTF8.GetString(result.Body.ToArray());
                else
                    return null;
            }
            catch (Exception)
            {
                return null;

            }

        }
    }
}
