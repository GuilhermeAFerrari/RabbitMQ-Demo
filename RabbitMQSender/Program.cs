using RabbitMQ.Client;
using System.Text;

ConnectionFactory factory = new();
factory.Uri = new Uri("amqp://guest:guest@localhost:5672");
factory.ClientProvidedName = "Rabbit sender app demo";

IConnection connection = factory.CreateConnection();

IModel channel = connection.CreateModel();

string exchangeName = "DemoExchange";
string routingKey = "demo-routing-key";
string queueName = "DemoQueue";

channel.ExchangeDeclare(exchangeName, ExchangeType.Direct);
channel.QueueDeclare(queueName, false, false, false, null);
channel.QueueBind(queueName, exchangeName, routingKey, null);

for (int i = 0; i < 50; i++)
{
    byte[] messageBodyBytes = Encoding.UTF8.GetBytes($"Message #{i+1}");
    channel.BasicPublish(exchangeName, routingKey, null, messageBodyBytes);
    Console.WriteLine(Encoding.UTF8.GetString(messageBodyBytes));
    Thread.Sleep(1000);
}


channel.Close();
connection.Close();