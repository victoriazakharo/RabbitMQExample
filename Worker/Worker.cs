using System;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Threading;

class Worker
{
    public static void Main()
    {
        const string queueName = "task_queue";
        using (var connection = CreateConnection())
        using (var channel = connection.CreateModel())
        {
            channel.QueueDeclare(queue: queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);
            channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);

            Console.WriteLine("Waiting for messages.");

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                var message = Encoding.UTF8.GetString(ea.Body);
                Console.WriteLine("Received {0}", message);
                channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
            };
            channel.BasicConsume(queue: queueName, autoAck: false, consumer: consumer);

            Console.WriteLine(" Press [enter] to exit.");
            Console.ReadLine();
        }
    }

    private static IConnection CreateConnection()
    {
        var factory = new ConnectionFactory() { HostName = "rabbitmq", UserName = "user", Password = "password" };
        IConnection connection = null;
        while (connection == null)
        {
            try
            {
                connection = factory.CreateConnection();
            }
            catch (RabbitMQ.Client.Exceptions.BrokerUnreachableException e)
            {
                Thread.Sleep(5000);
            }
        }
        return connection;
    }
}