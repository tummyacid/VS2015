using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.ServiceModel.Web;
using System.Text;
using System.Threading.Tasks;

namespace ServiceHostConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            hostService();
        }


        private static void hostService()
        {
            EdiService servObj = new EdiService();

            WebServiceHost _serviceHost = new WebServiceHost(servObj, new Uri("http://localhost:9000/"));
            WebHttpBinding binding = new WebHttpBinding();

            binding.MaxReceivedMessageSize = int.MaxValue;

            ServiceEndpoint ep = _serviceHost.AddServiceEndpoint(typeof(EdiService), binding, "");

            ServiceDebugBehavior stp = _serviceHost.Description.Behaviors.Find<ServiceDebugBehavior>();
            stp.HttpHelpPageEnabled = false;

            try
            {
                _serviceHost.Open();

                Console.WriteLine("Service is running " + _serviceHost.BaseAddresses[0]);
                Console.WriteLine("Press enter to quit...");
                Console.ReadLine();

                _serviceHost.Close();
            }
            catch (CommunicationException cex)
            {
                Console.WriteLine("An exception occurred: {0}", cex.Message);
                _serviceHost.Abort();
            }
        }
    }
}
