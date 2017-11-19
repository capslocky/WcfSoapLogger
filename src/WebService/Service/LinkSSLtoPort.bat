rem Linking SSL certificate with port
rem netsh http add sslcert ipport=0.0.0.0:62069 certstore=MY certhash=c180a7a64c9733026340511d8b404837fffc4383 appid={555b2e5f-4877-459b-bff2-60bb25898455}

netsh http show sslcert ipport=0.0.0.0:62069

rem netsh http delete sslcert ipport=0.0.0.0:62069

pause