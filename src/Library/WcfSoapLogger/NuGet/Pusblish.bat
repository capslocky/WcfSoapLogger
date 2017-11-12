@echo off

set /p answer=Publish package? [type 'yes']

if "%answer%"=="yes" (
	nuget push *.nupkg -Source https://www.nuget.org/api/v2/package
)

pause