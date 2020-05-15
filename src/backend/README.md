# Prerequisites
* .NET Core SDK >= 3.1
* Microsoft SQL Server 2017 Express

https://dotnet.microsoft.com/download
https://www.microsoft.com/en-us/download/details.aspx?id=55994

# How to run
1. Install .NET Core SDK
2. Install Microsoft SQL Server 2017 Express
   1. (Optional) Go to src\Hotel5000_Api\Web\appsettings.json, and modify the LodgingDb & LoggingDb's "Server=" parameter in the connection strings in appsettings.json according to your installation's connection string "Server=" parameter.
3. Execute run.bat, and navigate to https://localhost:5000 in your web browser.
   1. This step could take some minutes if its your first run. This step creates the necessary databases and applies migrations to it.