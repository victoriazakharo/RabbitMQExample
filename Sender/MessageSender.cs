using System;
using System.Text;
using System.Threading;
using RabbitMQ.Client;

namespace Sender
{
    public class MessageSender : IDisposable
    {
        private IConnection connection = null;

        public void Initialize()
        {
            var factory = new ConnectionFactory() { HostName = "rabbitmq", UserName = "user", Password = "password" };
            while (connection == null)
            {
                try
                {
                    connection = factory.CreateConnection();
                }
                catch (RabbitMQ.Client.Exceptions.BrokerUnreachableException)
                {
                    Thread.Sleep(5000);
                }
            }
        }

        public bool SendMessage(string message)
        {
            if (connection == null)
            {
                return false;
            }
            const string queueName = "task_queue";
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);

                var body = Encoding.UTF8.GetBytes(message);

                var properties = channel.CreateBasicProperties();
                properties.Persistent = true;

                channel.BasicPublish(exchange: "", routingKey: queueName, basicProperties: properties, body: body);
            }
            return true;
        }

        public void Dispose()
        {
            if (connection != null)
            {
                connection.Close();
            }
        }
    }
}
