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

set CODE_OUTPATH=%WORKSPACE%\UnityProject\Assets\GameScripts\HotFix\GameProto\Message\
echo "CODE_OUTPATH"

%LUA_BIN% %SPROTODUMP_DLL% -cs %PROTOS_PATH%\Member.sproto -o %CODE_OUTPATH%\Member.cs
%LUA_BIN% %SPROTODUMP_DLL% -cs %PROTOS_PATH%\c2s-slim.sproto -p client -o %CODE_OUTPATH%\ClientRpcMessage.cs
%LUA_BIN% %SPROTODUMP_DLL% -cs %PROTOS_PATH%\s2c-slim.sproto -p server -o %CODE_OUTPATH%\ServerRpcMessage.cs
rem %LUA_BIN% %SPROTODUMP_DLL% -cs %PROTOS_PATH%\s2c-slim.sproto -o %CODE_OUTPATH%\s2c-slim.cs

cd %NOW_PATH%
pause


