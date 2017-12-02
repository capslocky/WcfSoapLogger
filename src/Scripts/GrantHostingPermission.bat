REM netsh http add urlacl url=https://+:5581/weatherService user=PC\MyService

netsh http add urlacl url=http://+:5580/weatherService user=EVERYONE

netsh http show urlacl url=http://+:5580/weatherService

REM  netsh http delete urlacl url=http://+:5580/weatherService
pause