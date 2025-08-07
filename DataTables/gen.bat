set LUBAN_DLL=.\Luban\Luban.dll


dotnet %LUBAN_DLL% ^
    -t client ^
    -c cs-newtonsoft-json ^
    -d json  ^
    --conf luban.conf ^
    -x outputCodeDir=..\Assets\Config\Scripts ^
    -x outputDataDir=..\Assets\Resources\Config\Json

pause