rem Linking SSL certificate with port

netsh http add sslcert ipport=0.0.0.0:5581 certstore=MY certhash=b0ad08fe1e383e855cb978849e9d0c0fd8d7d331 appid={A941EA13-3509-418C-8822-3FBA137723C3}

netsh http show sslcert ipport=0.0.0.0:5581

rem netsh http delete sslcert ipport=0.0.0.0:5581

pause