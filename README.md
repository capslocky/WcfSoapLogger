[![Build status](https://ci.appveyor.com/api/projects/status/0bemisvvtdtbih97/branch/master?svg=true)](https://ci.appveyor.com/project/capslocky/wcfsoaplogger/branch/master)
[![NuGet](https://img.shields.io/nuget/v/WcfSoapLogger.svg?colorB=0f81c3)](https://www.nuget.org/packages/WcfSoapLogger/)
[![PVS-Studio](https://img.shields.io/badge/pvs--studio-free-brightgreen.svg)](https://www.viva64.com/en/b/0457/)
[![SonarCloud](https://img.shields.io/badge/sonar--cloud-pass-brightgreen.svg)](https://sonarcloud.io/dashboard?id=WcfSoapLogger)

# WcfSoapLogger #
This library is a message tracing tool for web-services and clients built with WCF.
Acting as [custom message encoder](https://docs.microsoft.com/en-us/dotnet/framework/wcf/samples/custom-message-encoder-custom-text-encoder) it captures raw XML SOAP data as plain HTTP body.


## Features ##
* You get exact content of each request and response
* Including any malformed requests and soap faults
* It's applicable for both parties: web-services and clients
* Easy start, no code modification needed - just put dll and adjust config file
* Default handler just saves each request and response as a separate file
* But you can implement any **custom handler** instead
* Supports SOAP 1.1, SOAP 1.2, addressings


## Default output sample ##
![ExampleBeta](/docs/images/main_screenshot.png?raw=true)


## Installation ##
* As [NuGet package](https://www.nuget.org/packages/WcfSoapLogger/) in Visual Studio project (Tools -> NuGet -> Console / Manager  -> Choose project). How to adjust [config file](/docs/ConfigFile.md).
```
Install-Package WcfSoapLogger
```
* [Manually](/docs/ManualInstallation.md) (for system administators)


## Usage examples ##
The repository contains **[usage examples](/src/UsageExamples)** covering different scenarios.
To clone run this command or download as [zip file](https://github.com/capslocky/WcfSoapLogger/archive/master.zip).
```
git clone https://github.com/capslocky/WcfSoapLogger.git
```


## Comparison with alternatives ##
You can find examples [here](/src/AlternativesExamples).

* **IDispatchMessageInspector (IClientMessageInspector)**
[Link 1](https://docs.microsoft.com/en-us/dotnet/framework/wcf/samples/message-inspectors)
[Link 2](https://blogs.msdn.microsoft.com/endpoint/2011/04/23/wcf-extensibility-message-inspectors/)  
can't see malformed requests (HTTP/1.1 400 Bad Request specifically)  
can't see original content


* **SvcTraceViewer.exe (<system.diagnostics>)**
[Link 1](https://docs.microsoft.com/en-us/dotnet/framework/wcf/diagnostics/configuring-message-logging)
[Link 2](https://docs.microsoft.com/en-us/dotnet/framework/wcf/service-trace-viewer-tool-svctraceviewer-exe)  
external utility hard in use  
no custom handling possible


* **Fiddler**
[Link 1](https://www.telerik.com/fiddler)
[Link 2](https://www.telerik.com/fiddler/fiddlercore)  
external proxy application


## Contributing ##
If you find this project useful you are welcome to make it better! Feel free to contact me if you have any ideas, questions or concerns. Also see [issues](https://github.com/capslocky/WcfSoapLogger/issues).
