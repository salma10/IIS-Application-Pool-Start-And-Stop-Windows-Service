# IIS-Application-Pool-Start-And-Stop-Windows-Service
IIS Application Pool Start And Stop Windows Service
This service starts application pools in IIS which has been stopped.

Installation instructions:

1. Run Command Prompt in Run as Administrator mode
2. cd C:\Windows\Microsoft.NET\Framework\v4.0.30319
3. InstallUtil.exe C:\inetpub\wwwroot\AppPoolService\AppPoolService\bin\Debug\AppPoolService.exe (path of your executable file)  
4. Windows button + R
5. Type services.msc
6. Start your service from here
