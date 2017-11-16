# WcfSoapLogger #
This library is a tracing tool for web-services or clients built with WCF.

## Features ##
* Exact byte content of each request and response
* Including any malformed requests and soap faults
* Each request and response is a separate file
* Works for both web-services and clients
* Easy start, no code modification needed - just put dll and [adjust](ConfigFile.md) config file
* [Custom handling](CustomHandling.md) of byte content in your code

```
Add log results screenshot here
```

## Installation ##
* Via [NuGet](https://www.nuget.org/packages/WcfSoapLogger/) in Visual Studio project
```
Install-Package WcfSoapLogger
```
* [Manually](ManualInstallation.md) (for system administators)

## Demo ##
Repository contains demo web-service and client.
https://github.com/capslocky/WcfSoapLogger/tree/master/src/WebService

## Contributing ##
If you find this project useful you are welcome to make it better!
Feel free to report issues here or to my email.
