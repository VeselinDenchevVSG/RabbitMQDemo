using RabbitMQ.Client;

using System.Text;

// https://www.youtube.com/watch?v=bfVddTJNiAw

ConnectionFactory connectionFactory = new();
connectionFactory.Uri = new Uri("amqp://guest:guest@localhost:5672");
connectionFactory.ClientProvidedName = "Rabbit Sender App";

IConnection connection = connectionFactory.CreateConnection();
IModel channel = connection.CreateModel();

string exchangeName = "DemoExchange";
string routingKey = "demo-routing-key";
string queueName = "DemoQueue";

channel.ExchangeDeclare(exchangeName, ExchangeType.Direct);
channel.QueueDeclare(queueName, false, false, false, null);
channel.QueueBind(queueName, exchangeName, routingKey, null);

for (int i = 0; i < 60; i++)
{
    Console.WriteLine($"Sending message {i}");
    byte[] messageBodyBytes = Encoding.UTF8.GetBytes($"Message #{i}!");
    channel.BasicPublish(exchangeName, routingKey, null, messageBodyBytes);

    Thread.Sleep(1000);
}

channel.Close();
connection.Close();