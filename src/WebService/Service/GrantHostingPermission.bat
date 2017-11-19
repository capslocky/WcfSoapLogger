netsh http add urlacl url=http://+:62060/service user=PC\MyService
rem netsh http add urlacl url=https://+:62062/service user=EVERYONE

netsh http show urlacl url=http://+:62060/service
rem netsh http delete urlacl url=http://+:62060/service
pause