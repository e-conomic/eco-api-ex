#### Looking for e-conomic .Net SDK? It has moved!
Find the SDK here: https://github.com/e-conomic/eco-api-sdk

# e-conomic API examples
##Examples for basic e-conomic SOAP API and REST API interaction.

We advice that you use our REST API if it covers your needs today. The REST API is currently under development and does not cover all functionality of e-conomic, but it is where our development resources are focused. Our SOAP API is still supported, and is currently the API that covers most of the e-conomic functionality.

For in-context examples of using the REST API please see the REST API Documentation: http://restdocs.e-conomic.com/

_When using the SOAP API method ConnectWithToken() all you need to provide is agreement grant token (token) and app secret token (appToken) like so:_
```C#
using (var operationScope = new OperationContextScope(session.InnerChannel))
{
    session.ConnectWithToken(<token>, <appToken>);
}
```

_When using the SOAP API method ConnectAsAdministrator() you are required to define an AppIdentifier like so:_
```C#
using (var operationScope = new OperationContextScope(session.InnerChannel))
{
    // Add a HTTP Header to an outgoing request
    var requestMessage = new HttpRequestMessageProperty();
    requestMessage.Headers["X-EconomicAppIdentifier"] = "MyCoolIntegration/1.1 (http://example.com/MyCoolIntegration/; MyCoolIntegration@example.com) BasedOnSuperLib/1.4";
    OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;

    session.ConnectAsAdministrator(<adminAgreementNo>, <adminUserID>, <adminUserPassword>, <clientAgreementNo>);
}
```
The AppIdentifier is only readable by e-conomic. Please include as much information as necessary for us to be able to reach you, the developers, efficiently.
