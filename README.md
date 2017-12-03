# WcfSoapLogger #
This library is a tracing tool for web-services and clients built with WCF.
Acting on low level it captures raw XML SOAP data.


## Features ##
* Exact byte content of each request and response
* Including any malformed requests and soap faults
* Each request and response is a separate file
* Works for both web-services and clients
* Easy start, no code modification needed - just put dll and adjust config file
* [Custom handling](/docs/CustomHandling.md) of byte content in your code

![ExampleBeta](/docs/images/main_screenshot.png?raw=true)


## Installation ##
* As [NuGet package](https://www.nuget.org/packages/WcfSoapLogger/) in Visual Studio project (Tools -> NuGet -> Console / Manager)
```
Install-Package WcfSoapLogger
```
* [Manually](/docs/ManualInstallation.md) (for system administators)


## Usage examples ##
The repository contains [usage examples](/src/UsageExamples).
To clone repository run this command or download as [zip file](https://github.com/capslocky/WcfSoapLogger/archive/master.zip).
```
git clone https://github.com/capslocky/WcfSoapLogger.git
```


## Comparison with alternatives ##
You can find examples [here](/src/AlternativesExamples).

* **IDispatchMessageInspector (IClientMessageInspector)**
[Link 1](https://docs.microsoft.com/en-us/dotnet/framework/wcf/samples/message-inspectors)
[Link 2](https://blogs.msdn.microsoft.com/endpoint/2011/04/23/wcf-extensibility-message-inspectors/)  
can't see malformed requests (HTTP/1.1 400 Bad Request)  
can't see original content

* **SvcTraceViewer.exe (<system.diagnostics>)**
[Link 1](https://docs.microsoft.com/en-us/dotnet/framework/wcf/diagnostics/configuring-message-logging)
[Link 2](https://docs.microsoft.com/en-us/dotnet/framework/wcf/service-trace-viewer-tool-svctraceviewer-exe)  
request seeking in big text files is difficult  
no custom handling


* **Fiddler**
[Link 1](https://www.telerik.com/fiddler)
[Link 2](https://www.telerik.com/fiddler/fiddlercore)  
Fiddler - external application  
FiddlerCore - to check


## Contributing ##
If you find this project useful you are welcome to make it better! Feel free to contact me if you have any ideas, questions or concerns.