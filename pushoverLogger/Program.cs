using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace pushoverLogger
{
    class Program
    {
        private static readonly string secret = "xx";
        private static readonly string devID = "dd";
        static void Main(string[] args)
        {
            //getSecret("email=tummyacid@gmail.com&password=hunter2");
            //getDevID("secret=" + secret + "&name=logger&os=O");

            XmlDocument messages = getMessages();
            
            Console.WriteLine("--- new messages ---");
            UInt64 maxAckedMessage = 0;
            foreach (XmlNode message in messages.SelectNodes("//message"))
            {
                Console.WriteLine(message.InnerText + "\r\n");
                maxAckedMessage = Math.Max(maxAckedMessage, Convert.ToUInt64(message.SelectSingleNode("../id").InnerText));
            }
            messages.Save(DateTime.Now.ToString("yyyyMMdd_hhmmss") + ".xml");
            Console.Write("Press any key to ack and clear these messages");
            Console.ReadLine();
            updateHighestMessage(maxAckedMessage);


        }
        private static void getSecret(string postData)
        {
            HttpWebRequest regDevice = (HttpWebRequest)WebRequest.Create("https://api.pushover.net/1/users/login.json");

            regDevice.Method = "POST";

            byte[] data = ASCIIEncoding.ASCII.GetBytes(postData);

            regDevice.ContentLength = data.Length;

            var newStream = regDevice.GetRequestStream();
            newStream.Write(data, 0, data.Length);
            newStream.Close();

            var response = regDevice.GetResponse();
            var responseStream = response.GetResponseStream();
            var responseReader = new StreamReader(responseStream);
            var result = responseReader.ReadToEnd();
        }

        private static void getDevID(string postData)
        {
            HttpWebRequest regDevice = (HttpWebRequest)WebRequest.Create("https://api.pushover.net/1/devices.json");

            regDevice.Method = "POST";

            byte[] data = ASCIIEncoding.ASCII.GetBytes(postData);

            regDevice.ContentLength = data.Length;

            var newStream = regDevice.GetRequestStream();
            newStream.Write(data, 0, data.Length);
            newStream.Close();

            var response = regDevice.GetResponse();
            var responseStream = response.GetResponseStream();
            var responseReader = new StreamReader(responseStream);
            var result = responseReader.ReadToEnd();
        }
        private static XmlDocument getMessages()
        {
            String postData = "secret=" + secret + "&device_id=" + devID;
            HttpWebRequest messageRequest = (HttpWebRequest)WebRequest.Create("https://api.pushover.net/1/messages.json?" + postData);

            messageRequest.Method = "GET";

            byte[] data = ASCIIEncoding.ASCII.GetBytes(postData);

            //regDevice.ContentLength = data.Length;
            

            var response = messageRequest.GetResponse();
            var responseStream = response.GetResponseStream();
            var responseReader = new StreamReader(responseStream);
            var result = responseReader.ReadToEnd();


            return (XmlDocument)Newtonsoft.Json.JsonConvert.DeserializeXmlNode(result, "OldMessages");
        }
        private static void updateHighestMessage(UInt64 messageID)
        {
            String postData = "secret=" + secret + "&message=" + messageID.ToString();
            HttpWebRequest messageClear = (HttpWebRequest)WebRequest.Create("https://api.pushover.net/1/devices/" + devID + "/update_highest_message.json?" + postData);

            messageClear.Method = "POST";
            
            var response = messageClear.GetResponse();
            var responseStream = response.GetResponseStream();
            var responseReader = new StreamReader(responseStream);
            var result = responseReader.ReadToEnd();
        }
    }

}

