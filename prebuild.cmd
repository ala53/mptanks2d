SET msbuildpath="C:\Program Files\Microsoft Visual Studio\2022\Community\MSBuild\Current\bin\msbuild.exe"

echo "blank" > %~dp0\MPTanks-MK5\CoreAssets\core-assets.mod

nuget restore %~dp0Tools\AssetCompileHelper\AssetCompileHelper.sln
nuget restore %~dp03rd_Party\Starbound.Input-3.18.2015\Starbound.Input.sln
nuget restore %~dp03rd_Party\Lidgren.Network-8.31.2015\Lidgren.Network.sln
nuget restore %~dp0Tools\MPTanks.ModCompiler\MPTanks.ModCompiler.sln
nuget restore %~dp0MPTanks-MK5\MPTanks-MK5.sln
nuget restore %~dp0Infrastructure\ZSB.Drm.Client\ZSB.Drm.Client.sln

%msbuildpath% %~dp0\Tools\AssetCompileHelper\AssetCompileHelper.sln /t:Rebuild /p:Configuration=Release
%msbuildpath% %~dp0\3rd_Party\Starbound.Input-3.18.2015\Starbound.Input.sln /t:Rebuild /p:Configuration=Release
%msbuildpath% %~dp0\3rd_Party\Lidgren.Network-8.31.2015\Lidgren.Network.sln /t:Rebuild /p:Configuration=Release
%msbuildpath% %~dp0\Tools\MPTanks.ModCompiler\MPTanks.ModCompiler.sln /t:Rebuild /p:Configuration=Release
%msbuildpath% %~dp0\Infrastructure\ZSB.Drm.Client\ZSB.Drm.Client.sln /t:Rebuild /p:Configuration=Release

REM Building assets
asset_build.cmd %~dp0\MPTanks-MK5\ %~dp0\MPTanks-MK5\Client\Backend\