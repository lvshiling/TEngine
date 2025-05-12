Cd /d %~dp0
echo %CD%
set NOW_PATH=%CD%
set WORKSPACE=..\..
dir %WORKSPACE%
set SPROTODUMP_PATH=%WORKSPACE%\Tools\sprotodump\
set SPROTODUMP_DLL=%SPROTODUMP_PATH%\sprotodump.lua
set PROTOS_PATH=%WORKSPACE%\Protos\GameProto\Protos\
dir %SPROTODUMP_PATH%
cd %SPROTODUMP_PATH%
rem dir .
echo "SPROTODUMP_DLL"
echo %SPROTODUMP_DLL%
set LUA_BIN=%WORKSPACE%\Tools\lua547-win32\lua547\Debug\lua54.exe
echo "LUA_BIN"
echo %LUA_BIN%
rem set CONF_ROOT=.
rem set DATA_OUTPATH=%WORKSPACE%/UnityProject/Assets/AssetRaw/Configs/bytes/
rem set CODE_OUTPATH=%WORKSPACE%/UnityProject/Assets/GameScripts/HotFix/GameProto/GameConfig/

%LUA_BIN% %SPROTODUMP_DLL% -cs %PROTOS_PATH%\Member.sproto -o Member.cs

cd %NOW_PATH%
pause


