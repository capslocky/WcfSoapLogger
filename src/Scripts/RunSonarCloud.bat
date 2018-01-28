@rem https://sonarcloud.io/dashboard?id=WcfSoapLogger

@rem https://www.appveyor.com/blog/2016/12/23/sonarqube/
@rem https://about.sonarcloud.io/get-started/
@rem https://docs.sonarqube.org/display/SCAN/Analyzing+with+SonarQube+Scanner+for+MSBuild

@echo SonarCloud for WcfSoapLogger
@rem pause

@set scriptsDir=%~dp0
@rem cd /d %scriptsDir%

rem set MSBuildLocation="C:\Windows\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe"
@set MSBuildLocation="C:\Program Files (x86)\MSBuild\14.0\Bin\MSBuild.exe"
@rem set MSBuildLocation="C:\Program Files (x86)\Microsoft Visual Studio\2017\Community\MSBuild\15.0\Bin\MSBuild.exe"

SonarQube.Scanner.MSBuild.exe begin /k:"WcfSoapLogger" /o:"capslocky-github" /s:"%scriptsDir%RunSonarCloud.Settings.xml"

%MSBuildLocation% "%scriptsDir%../Library/WcfSoapLogger.sln" /verbosity:normal /t:Rebuild /p:Configuration="Release" /p:Platform="Any CPU"

SonarQube.Scanner.MSBuild.exe end 

pause