del  .PublishFiles\*.*   /s /q

dotnet publish -r win-x64 --self-contained=false  .\CodeGenerator.csproj -c Release -p:PublishDir=.\.PublishFiles 

del  .PublishFiles\*.pdb   /s /q

pause