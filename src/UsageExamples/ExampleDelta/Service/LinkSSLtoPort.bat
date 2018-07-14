rem Linking SSL certificate with port
rem 'appid' is any random guid, 'certhash' is thumbprint of certificate
rem run this as admin

netsh http add sslcert ipport=0.0.0.0:5584 certstore=MY certhash=77ce7ba50ea75cdaa70c097301ea1b69ba18dbbe appid={365F3B03-46D7-463B-B8AF-BE8456760A63}

netsh http show sslcert ipport=0.0.0.0:5584

rem netsh http delete sslcert ipport=0.0.0.0:5584

pause