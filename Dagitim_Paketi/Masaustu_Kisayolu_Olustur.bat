@echo off
setlocal
cd /d "%~dp0"
if exist "%~dp0Masaustu_Kisayolu_Olustur.vbs" (
    wscript "%~dp0Masaustu_Kisayolu_Olustur.vbs"
) else (
    echo VBScript dosyasi bulunamadi!
    pause
)
