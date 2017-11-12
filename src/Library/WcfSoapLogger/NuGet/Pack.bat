@echo off

del *.nupkg
nuget pack WcfSoapLogger.nuspec -properties Configuration=Release

pause