## Config file ##


### Endpoint ###

Basically all you need to start logging is to modify following line in your config file.

From this one:
```xml
<endpoint address="..." binding="basicHttpBinding" bindingConfiguration="..." contract="..." />
```

To this one:
```xml  
<endpoint address="..." binding="customBinding" bindingConfiguration="soapLoggerBinding" contract="..." />
```


### Custom Binding ###

`<bindings>` element should contain a definition of that custom binding.
Luckily NuGet package automatically adds this one during its installation.
But you can add more if different output folders for different endpoints needed.

```xml
<bindings>
  <customBinding>
    <binding name="soapLoggerBinding">
      <soapLoggerMessageEncoding logPath="C:\SoapLog\MyService" useCustomHandler="false" />
      <httpTransport />
    </binding>
  </customBinding>
  <basicHttpBinding>
    ...
  </basicHttpBinding>
</bindings>
```


### Extensions ###

This element is also added automatically and should be declared only once.

```xml
<extensions>
  <bindingElementExtensions>
    <add name="soapLoggerMessageEncoding" type="WcfSoapLogger.EncodingExtension.LoggingExtensionElement, WcfSoapLogger" />
  </bindingElementExtensions>
</extensions>
```


### Final config for service ###

This is a full configuration of service in [example Beta](../src/UsageExamples/ExampleBeta/Service/App.config).

```xml
<?xml version="1.0" encoding="utf-8"?>
<configuration>
  <system.serviceModel>

    <services>
      <service name="CommonService.WeatherServiceEurope" behaviorConfiguration="weatherServiceBehavior">
        <host>
          <baseAddresses>
            <add baseAddress="http://localhost:5580/weatherService" />
          </baseAddresses>
        </host>
        <endpoint address="" 
                  binding="customBinding" bindingConfiguration="soapLoggerBinding" 
                  contract="CommonService.IWeatherService" />
        <endpoint address="mex" binding="mexHttpBinding" contract="IMetadataExchange" />
      </service>
    </services>

    <bindings>
      <customBinding>
        <binding name="soapLoggerBinding">
          <soapLoggerMessageEncoding logPath="C:\SoapLog\Beta\Service" useCustomHandler="false" />
          <httpTransport />
        </binding>
      </customBinding>
    </bindings>

    <behaviors>
      <serviceBehaviors>
        <behavior name="weatherServiceBehavior">
          <serviceMetadata httpGetEnabled="true" />
          <serviceDebug includeExceptionDetailInFaults="true" />
        </behavior>
      </serviceBehaviors>
    </behaviors>
    
    <extensions>
      <bindingElementExtensions>
        <add name="soapLoggerMessageEncoding" type="WcfSoapLogger.EncodingExtension.LoggingExtensionElement, WcfSoapLogger" />
      </bindingElementExtensions>
    </extensions>

  </system.serviceModel>
</configuration>
```


### Final config for client ###

This is a full configuration of client in [example Beta](../src/UsageExamples/ExampleBeta/Client/App.config).

```xml
<?xml version="1.0" encoding="utf-8"?>
<configuration>
  <system.serviceModel>

    <client>
      <endpoint address="http://localhost:5580/weatherService"
                binding="customBinding" bindingConfiguration="soapLoggerBinding"
                contract="CommonClient.IWeatherService" />
    </client>

    <bindings>
      <customBinding>
        <binding name="soapLoggerBinding">
          <soapLoggerMessageEncoding logPath="C:\SoapLog\Beta\Client" useCustomHandler="false" />
          <httpTransport />
        </binding>
      </customBinding>
    </bindings>

    <extensions>
      <bindingElementExtensions>
        <add name="soapLoggerMessageEncoding" type="WcfSoapLogger.EncodingExtension.LoggingExtensionElement, WcfSoapLogger" />
      </bindingElementExtensions>
    </extensions>

  </system.serviceModel>
</configuration>

```




### Config file name ###

**web.config** - in case of IIS hosting

**App.config** - Visual Studio - in case of self-hosting or client side

**_ApplicationName_.exe.config** - deployed