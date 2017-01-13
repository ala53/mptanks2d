REM Project Dir: %2
REM Solution Dir: %1



"%2ekUiGen.exe" --input="%2assets\ui\xaml" --output="%2UI" --oa="%2assets\ui\imgs" --rm=MonoGame


%1..\Build\Tools\AssetCompileHelper\AssetCompileHelper.exe "" "%1..\Build Tools\MGCB\Windows\MGCB.exe" "%2\" "WindowsGL"
