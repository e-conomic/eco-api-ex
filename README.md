# e-conomic API examples
##Examples for basic e-conomic SOAP API and REST API interaction.

We advice that you use our REST API if it covers your needs today. The REST API is where our development resources are focused. Our SOAP API is still supported although in feature freeze.

For in-context examples of using the REST API please see the REST API Documentation: http://restdocs.e-conomic.com/

_When using the SOAP API method ConnectWithToken() all you need to provide is agreement grant token (token) and app secret token (appToken) like so:_
```C#
using (var operationScope = new OperationContextScope(session.InnerChannel))
{
    session.ConnectWithToken(<token>, <appToken>);
}
```

You can find our full API authentication guide here: https://www.e-conomic.com/developer/connect


#### Looking for the e-conomic .net SDK (the dll)?
The SDK is end-of-life and is no longer supported and is not recommended for production use.
