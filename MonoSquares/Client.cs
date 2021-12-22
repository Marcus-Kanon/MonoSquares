using Newtonsoft.Json.Linq;
using SharedClientServer;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace MonoSquaresClient
{
    public class Message
    {
        public string StringProp { get; set; }
        public int IntProp { get; set; }
    }


    internal class Client
    {
        
        public static async Task Run()
        {
            Console.WriteLine("Press Enter to COnnect");

            Console.ReadLine();

            var endPoint = new IPEndPoint(IPAddress.Loopback, 9000);
            //var channel = new ClientChannel<JsonMessageProtocol, JObject>();
            var channel = new ClientChannel<XmlMessageProtocol, XDocument>();
            channel.ConnectAsync(endPoint);
            channel.OnMessage(OnMessage);
            

            var message = new Message();
            message.StringProp = "HHEllu";
            message.IntProp = 404;

            await channel.SendAsync(message).ConfigureAwait(false);

            Console.ReadLine();
        }

        static Task OnMessage(JObject jObject)
        {
            Console.WriteLine("Received JOBject");
            Print(Convert(jObject));

            Console.Read();
            return Task.CompletedTask;
        }

        static Task OnMessage(XDocument xDoc)
        {
            Console.WriteLine("Received XDocument");
            Print(Convert(xDoc));

            Console.Read();
            return Task.CompletedTask;
        }

        static Message Convert(JObject jObject)
        {
            return jObject.ToObject(typeof(Message)) as Message;
        }

        static Message Convert(XDocument xmlDocument)
        {
            return new XmlSerializer( typeof(Message)).Deserialize(new StringReader(xmlDocument.ToString())) as Message;
        }

        static void Print(Message message)
        {
            Console.WriteLine("StringProp = " + message.StringProp + "\nIntProp = " + message.IntProp);
        }
    }
}
