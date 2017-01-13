echo Project Dir: %2
echo Solution Dir: %1



"%2ekUiGen.exe" --input="%2assets\ui\xaml" --output="%2UI" --oa="%2assets\ui\imgs" --rm=MonoGame


%~dp0Build\Tools\AssetCompileHelper\AssetCompileHelper.exe "" "%~dp0\Build Tools\MGCB\Windows\MGCB.exe" %2 "WindowsGL"
