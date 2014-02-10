using Apache.NMS;
using Apache.NMS.ActiveMQ;
using Apache.NMS.ActiveMQ.Commands;
using System;
using System.Threading;

namespace StockServer
{
    class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory("failover:(tcp://localhost:61616)");
            IDestination destination;
            Console.Write("Press Q for Queue or T for Topic: ");
            if (Console.ReadKey().Key == ConsoleKey.Q)
            {
                destination = new ActiveMQQueue("StockQueue");
            }
            else
            {
                destination = new ActiveMQTopic("StockTopic");
            }

            using (IConnection cnn = factory.CreateConnection())
            {
                cnn.Start();
                Console.WriteLine("Connected");
                using (ISession session = cnn.CreateSession())
                using (IMessageProducer producer = session.CreateProducer(destination))
                {
                    while (true)
                    {
                        ITextMessage message = GenerateStockMessage(producer);
                        Console.WriteLine(message.Text);
                        producer.Send(message);
                        Thread.Sleep(random.Next(500, 1500));
                    }
                }
            }
        }
        static string[] Companies = new string[] { "A Company", "B Company", "C Company", "D Company", "E Company" };
        static ITextMessage GenerateStockMessage(IMessageProducer producer)
        {

            string company = Companies[random.Next(Companies.Length)];

            double value;
            int i = random.Next(1001);
            if (i == 500)
            {
                value = 0;
            }
            else if (i > 500)
            {
                value = (i - 500) / 10;
            }
            else
            {
                value = (500 - i) / 10;
            }

            ITextMessage message = producer.CreateTextMessage(String.Format("{0} {1:C2}", company, value));
            message.Properties.SetString("Company", company);
            message.Properties.SetString("Direction", value >= 0 ? "UP" : "DOWN");
            message.Properties.SetDouble("Value", value);
            return message;
        }

        static Random random = new Random();
    }
}
