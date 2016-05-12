using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Reflection;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Threading.Tasks;
//Using the imported service reference. You may have another name for the reference.
using X_EconomicAppIdentifier.econSoap;

namespace X_EconomicAppIdentifier
{
    class Program
    {
        static void Main(string[] args)
         {
                VerifyAppIdentifier();
                
         } 


        private static void Connect(EconomicWebServiceSoapClient session)
        {
            // A necessary setting as the session is put in a cookie
            ((BasicHttpBinding)session.Endpoint.Binding).AllowCookies = true;

            //Setting the X-EconomicAppIdentifier HTTP Header
            using (new OperationContextScope(session.InnerChannel))
            {
                var requestMessage = new HttpRequestMessageProperty();
                requestMessage.Headers["X-EconomicAppIdentifier"] =
                    "MyCoolIntegration/1.1 (http://example.com/MyCoolIntegration/; MyCoolIntegration@example.com) BasedOnSuperLib/1.4";
                OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;

                // Connect as administrator
                //session.ConnectAsAdministrator(ADMINAGREEMENT, "ADMINUSER", "PASS", ENDUSERAGREEMENT);

                // Connect with credentials
                //session.Connect(ENDUSERAGREEMENT, "USER", "PASS");

                // Connect with token
                //session.ConnectWithToken("AGREEMENTGRANTTOKEN","APPSECRETTOKEN");
            }
        }

        public static void VerifyAppIdentifier()
        {
            using (var session = new EconomicWebServiceSoapClient())
            {
                Console.WriteLine("Connecting");
                Connect(session);

                var verification = session.Verify_XEconomicAppIdentifier();
                if (verification == true)
                {
                    Console.WriteLine("AppIdentifier acknowledged.");
                }
                else
                {
                    Console.WriteLine("AppIdentifier failed.");
                }

                Console.WriteLine("Disconnecting");
                session.Disconnect();
                Console.WriteLine("Done");
                Console.ReadKey();

            }
        }
    }
}
