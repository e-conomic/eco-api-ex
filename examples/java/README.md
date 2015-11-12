Java example SOAP client

To compile using Maven:

1. Download and install Maven as described on http://maven.apache.org/download.html
2. From a shell in /eco-api-ex/java run the command: `mvn assembly:assembly` to generate objects based on WSDL
3. Compile using maven: `mvn install` or using your favorite ide
4. Run using `javac -jar .\target\e-conomic-soap-client.jar`