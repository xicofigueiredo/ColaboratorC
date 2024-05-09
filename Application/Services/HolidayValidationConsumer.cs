using System.Drawing;
using System.Text;
using Application.DTO;
using Application.Services;
using Domain.IRepository;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using NuGet.Packaging.Signing;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

public class HolidayValidationConsumer : IHolidayValidationConsumer
{
    private readonly IModel _channel;
   

    private readonly IServiceScopeFactory _serviceScopeFactory;
  

    List<string> _errorMessages = new List<string>();

    public HolidayValidationConsumer(IServiceScopeFactory serviceScopeFactory)
    {
       
        _serviceScopeFactory = serviceScopeFactory;
        var factory = new ConnectionFactory { HostName = "localhost" };
        var connection = factory.CreateConnection();
        _channel = connection.CreateModel();

        _channel.ExchangeDeclare(exchange: "HolidayValidation", type: ExchangeType.Fanout);

        

      
        Console.WriteLine(" [*] Waiting for Holiday to validate.");
    }

    public void StartHolidayConsuming(string queueName)
    {
        string queue = queueName + "holiday";
        _channel.QueueDeclare(queue: queue, durable: true, exclusive: false, autoDelete: false);
        _channel.QueueBind(queue: queue, exchange: "HolidayValidation", routingKey: "holiKey");


        var consumer = new EventingBasicConsumer(_channel);
        consumer.Received += async (model, ea) =>
        {
            byte[] body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            if(message != null){
               string validação = "ok";
               PublishMessage(validação);
               Console.WriteLine($" [HolidayAccepted]");
            }
            Console.WriteLine($" [HolidayValidationConsumer] {message}");
 
        };

        _channel.BasicConsume(queue: queue, autoAck: true, consumer: consumer);
    }

        public void PublishMessage(string message)
        {
          
          

            var body = Encoding.UTF8.GetBytes(message);
            
            _channel.BasicPublish(exchange: "HolidayValidationConfirmed",
                              routingKey: "holiKey",
                              basicProperties: null,
                              body: body);
            

           
        }
}
