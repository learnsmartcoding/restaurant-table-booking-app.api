To check the installed Entity Framework (EF) versions on your machine, you can use the following steps:

Open a command prompt (cmd) or PowerShell window.
Use the following command to list all installed EF packages for all .NET Core/.NET projects on your machine:

dotnet tool list --global | findstr "dotnet-ef"

if you already have some version and wanted to upgrade use this cmd
dotnet tool update --global dotnet-ef --version 7.0.9 
or
dotnet tool update --global dotnet-ef (this installs latest version)

*********************************

To Scaffold databse as model to local project use below cmd.

dotnet ef dbcontext scaffold "your connection string goes here" Microsoft.EntityFrameworkCore.SqlServer -o ModelTemp -d

