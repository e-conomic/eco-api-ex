# e-conomic API PHP examples
##Examples for basic e-conomic SOAP API and REST API interaction.

We advice that you use our REST API if it covers your needs today. The REST API is currently under development and does not cover all functionality of e-conomic, but it is where our development resources are focused. Our SOAP API is still supported, and is currently the API that covers most of the e-conomic functionality.

For in-context examples of using the REST API please see the REST API Documentation: http://restdocs.e-conomic.com/

_When using any other SOAP connect method than ConnectWithToken() you are required to set an X-EconomicAppIdentifier HTTP header in the PHP SOAP Client constructor like so:_
```PHP
"stream_context" => stream_context_create(
				array(
					"http" => array(
						"header" => "X-EconomicAppIdentifier: MyCoolIntegration/1.1 (http://example.com/MyCoolIntegration/; MyCoolIntegration@example.com) BasedOnSuperLib/1.4"
					)
				)
```
The AppIdentifier is only readable by e-conomic. Please include as much information as necessary for us to be able to reach you the developers efficiently.