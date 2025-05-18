@echo off
"%LOCALAPPDATA%\Programs\Inno Setup 6\ISCC.exe" installer.iss
if errorlevel 1 (
    echo Error compiling installer
    pause
    exit /b 1
)
echo Installer compiled successfully
pause 