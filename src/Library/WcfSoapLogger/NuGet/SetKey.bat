@echo foo
REM setting NuGet API key to its config file: %AppData%\NuGet
REM DON'T commit real value !!!

nuget setapikey 1234-abcd-1234 -source https://www.nuget.org/api/v2/package

pause