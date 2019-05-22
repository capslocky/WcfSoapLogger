@echo off

del *.nupkg
nuget pack ..\WcfSoapLogger.csproj -build -properties Configuration=Release

pause