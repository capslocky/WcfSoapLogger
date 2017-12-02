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

```
Add log results screenshot here
```


## Installation ##
* As [NuGet package](https://www.nuget.org/packages/WcfSoapLogger/) in Visual Studio project (Tools -> NuGet -> Console / Manager)
```
Install-Package WcfSoapLogger
```
* [Manually](/docs/ManualInstallation.md) (for system administators)


## Usage examples ##
This repository contains [usage examples](/src/UsageExamples).
To clone repository execute following command or download it as [zip file](https://github.com/capslocky/WcfSoapLogger/archive/master.zip).
```
git clone https://github.com/capslocky/WcfSoapLogger.git
```


## Comparison with alternatives ##
* **SvcTraceViewer.exe (<system.diagnostics>)**
[Link 1](https://docs.microsoft.com/en-us/dotnet/framework/wcf/diagnostics/configuring-message-logging)
[Link 2](https://docs.microsoft.com/en-us/dotnet/framework/wcf/service-trace-viewer-tool-svctraceviewer-exe)  
big result files   
request seeking is difficult

* **IDispatchMessageInspector (IClientMessageInspector)** 
[Link 1](https://docs.microsoft.com/en-us/dotnet/framework/wcf/samples/message-inspectors)
[Link 2](https://blogs.msdn.microsoft.com/endpoint/2011/04/23/wcf-extensibility-message-inspectors/)  
can't see malformed requests  
can't see XML tags beyond expected scheme

* **Fiddler**
[Link 1](https://www.telerik.com/fiddler)
[Link 2](https://www.telerik.com/fiddler/fiddlercore)  
Fiddler - external desktop application  
FiddlerCore - to check


## Contributing ##
If you find this project useful you are welcome to make it better! Feel free to contact me if you have any ideas, questions or concerns.