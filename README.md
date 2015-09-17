# e-conomic .Net SDK

This SDK is intended to make it easier to use the SOAP API made available by e-conomic to integration partners.

## THIS SDK IS DEPRECATED

The current relase (v1.4.20) which is released on Sep 17th 2015 is the last release of the SDK. From this date the SDK has been deprecated and we no longer advise the use of this SDK. We will no longer fix bugs in this SDK. If you encounter bugs or want new functionality, we refer you to use our SOAP api directly or take a look at our REST api.

This last release includes some bugfixes and minor improvements. So if you do use this API today, we strongly urge you to upgrade to this version.

This release also includes a breaking change. You now have to specify an App Identifier when creating a new instance of EconomicSession. This is a string, that identifies your app to us. The recommended identifier format is `MyAppName/1.1 (http://example.com/MyAppName/; MyAppName@example.com)`.

You will need to uprade to this latest SDK as soon as possible. In the fall of 2015 this is the only SDK assembly that will work. All older binaries that do not include an app identifier in all requests will be rejected by our servers.

Below you can find a C# example
```C#
using (var operationScope = new OperationContextScope(session.InnerChannel))
{
    // Add a HTTP Header to an outgoing request
    var requestMessage = new HttpRequestMessageProperty();
    requestMessage.Headers["X-EconomicAppIdentifier"] = "MyCoolIntegration/1.1 (http://example.com/MyCoolIntegration/; MyCoolIntegration@example.com) BasedOnSuperLib/1.4";
    OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;
 
    session.Connect(<agreement>, <user>, <password>);
}
```

### Alternatives to this SDK

You can find more developer resources at http://www.e-conomic.com/developer

We advice that you use our REST Api if this covers your needs today. This is currently under development and does not cover all functionality, but this is where our development resources are focused. Our SOAP Api is still supported, and is currently the API that covers most of the e-conomic functionality.
