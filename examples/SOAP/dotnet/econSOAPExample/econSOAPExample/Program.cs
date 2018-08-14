using System;
using System.ServiceModel;
//Using the imported service reference. You may have another name for the reference.
using econSOAPExample.econSoap;

namespace econSOAPExample
{
    class Program
    {
        static void Main(string[] args)
        {
            // Include this in the start of your implementation to allow dotnet to use higher tls version than 1.0
            // e-conomic support tls 1.2; use the highest tls version your framework allows.
            // This line will work on NET4.5 and up
            System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;

            // This line will work on NET4.0 if you have NET4.5 installed on the system it is running on
            //System.Net.ServicePointManager.SecurityProtocol = (System.Net.SecurityProtocolType)3072;

            // If you are building NET3.5 then you need  Reliability Rollup HR-1605 to support higher tls versions

            PrintCompanyName();
        }

        private static void Connect(EconomicWebServiceSoapClient session)
        {
            // A necessary setting as the session is put in a cookie
            ((BasicHttpBinding)session.Endpoint.Binding).AllowCookies = true;


            using (new OperationContextScope(session.InnerChannel))
            {
                //Setting the X-EconomicAppIdentifier HTTP Header. Only required for ConnectAsAdministrator.
                //var requestMessage = new HttpRequestMessageProperty();
                //requestMessage.Headers["X-EconomicAppIdentifier"] =
                //    "MyCoolIntegration/1.1 (http://example.com/MyCoolIntegration/; MyCoolIntegration@example.com) BasedOnSuperLib/1.4";
                //OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;

                // Connect as administrator
                //session.ConnectAsAdministrator(ADMINAGREEMENT, "ADMINUSER", "PASS", ENDUSERAGREEMENT);

                // Connect with token
                session.ConnectWithToken("AGREEMENTGRANTTOKEN", "APPSECRETTOKEN");
            }
        }

        public static void PrintCompanyName()
        {
            using (var session = new EconomicWebServiceSoapClient())
            {
                Console.WriteLine("Connecting");
                Connect(session);

                var companyHandle = session.Company_Get();
                var companyData = session.Company_GetData(companyHandle);

                Console.WriteLine(companyData.Name);

                Console.WriteLine("Disconnecting");
                session.Disconnect();
                Console.WriteLine("Done");
                Console.ReadKey();
            }
        }
    }
}
