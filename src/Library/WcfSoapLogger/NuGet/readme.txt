WcfSoapLogger
Logging SOAP request/response body at binary level.

All you need to start default logging is:

1) Set these two attributes to your endpoint element (inside <service> or <client>)

<endpoint address="http://server/web-service" binding="customBinding" bindingConfiguration="soapLoggerBinding"

2) Specify folder to save files. Make sure user account has write access to it.

<soapLoggerMessageEncoding logPath="D:\MyService\SoapLogs" useCustomHandler="false"/>

============================================================================

Two elements have been automatically added under <system.serviceModel> element of your config file.
Both will be automatically removed on package uninstallation.

<bindings>
	<customBinding>
		<binding name="soapLoggerBinding">
			...
		</binding>
	</customBinding>
</bindings>

<extensions>
	<bindingElementExtensions>
		<add name="soapLoggerMessageEncoding" ... >
	</bindingElementExtensions>
</extensions>

============================================================================

This library is under active development.
Any help would be much appreciated.

Project site:
https://github.com/capslocky/WcfSoapLogger

Contact me if you have any questions:
atanov.b@gmail.com

Baurzhan Atanov
25 November 2017