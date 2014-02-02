using Apache.NMS;
using Apache.NMS.ActiveMQ;
using Apache.NMS.ActiveMQ.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ChatClient
{
    public partial class ChatForm : Form
    {
        IConnection connection;
        ISession session;
        IMessageConsumer consumer;
        IMessageProducer producer;
        public ChatForm()
        {
            InitializeComponent();
            var factory = new ConnectionFactory("failover:(tcp://localhost:61616)");
            connection = factory.CreateConnection();
            connection.Start();
            session = connection.CreateSession();
            IDestination chatTopic = new ActiveMQTopic("ChatTopic");
            producer = session.CreateProducer(chatTopic);
            consumer = session.CreateConsumer(chatTopic);
            consumer.Listener += consumer_Listener;
        }


        void consumer_Listener(IMessage message)
        {
            ITextMessage chatMessage = (ITextMessage)message;
            string name = chatMessage.Properties.GetString("Name");
            DisplayMessage(name, chatMessage.Text);
        }

        delegate void DisplayMessageCallback(string username, string message);
        private void DisplayMessage(string username, string message)
        {
            if (this.listBox1.InvokeRequired)
            {
                DisplayMessageCallback d = new DisplayMessageCallback(DisplayMessage);
                this.Invoke(d, new object[] { username, message });
            }
            else
            {
                this.listBox1.Items.Add(String.Format("{0}: {1}", username, message));
            }
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                SendMessage();
                e.Handled = true;
            }
        }

        private void SendMessage()
        {
            ITextMessage chatMessage = session.CreateTextMessage(textBox1.Text);
            chatMessage.Properties.SetString("Name", textBox2.Text);
            producer.Send(chatMessage);
            textBox1.Clear();
        }
    }
}
