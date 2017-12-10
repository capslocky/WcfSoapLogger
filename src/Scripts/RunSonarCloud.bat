@rem https://sonarcloud.io/dashboard?id=WcfSoapLogger

@rem https://www.appveyor.com/blog/2016/12/23/sonarqube/
@rem https://about.sonarcloud.io/get-started/
@rem https://docs.sonarqube.org/display/SCAN/Analyzing+with+SonarQube+Scanner+for+MSBuild

@echo SonarCloud for WcfSoapLogger
@pause

@cd /d %~dp0
@set login="a73e2dc3c1b2da9be687279b2dfbd4cac3035de0"

@rem set MSBuildLocation="C:\Windows\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe"
@set MSBuildLocation="C:\Program Files (x86)\MSBuild\14.0\Bin\MSBuild.exe"
@rem set MSBuildLocation="C:\Program Files (x86)\Microsoft Visual Studio\2017\Community\MSBuild\15.0\Bin\MSBuild.exe"


SonarQube.Scanner.MSBuild.exe begin /k:"WcfSoapLogger" /o:"capslocky-github" /d:sonar.host.url="https://sonarcloud.io" /d:sonar.login=%login%

%MSBuildLocation% "../Library/WcfSoapLogger.sln" /t:Rebuild

SonarQube.Scanner.MSBuild.exe end /d:sonar.login=%login%

pause