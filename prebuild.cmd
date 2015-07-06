SET msbuildpath="C:\Program Files (x86)\MSBuild\14.0\Bin\msbuild.exe"

%msbuildpath% %~dp0\Tools\AssetCompileHelper\AssetCompileHelper.sln /t:Rebuild /p:Configuration=Release
%msbuildpath% %~dp0\Tools\MPTanks.MapMaker\MPTanks.MapMaker.sln /t:Rebuild /p:Configuration=Release
%msbuildpath% %~dp0\Tools\MPTanks.ModCompiler\MPTanks.ModCompiler.sln /t:Rebuild /p:Configuration=Release

%msbuildpath% %~dp0\Tools\AssetCompileHelper\AssetCompileHelper.sln /t:Rebuild /p:Configuration=Debug
%msbuildpath% %~dp0\Tools\MPTanks.MapMaker\MPTanks.MapMaker.sln /t:Rebuild /p:Configuration=Debug
%msbuildpath% %~dp0\Tools\MPTanks.ModCompiler\MPTanks.ModCompiler.sln /t:Rebuild /p:Configuration=Debug
