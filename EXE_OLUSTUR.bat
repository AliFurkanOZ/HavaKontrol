@echo off
chcp 65001 > nul
title Hava Kontrol - EXE Dağıtım Paketi Oluşturucu
cls
echo =======================================================================
echo            HAVA KONTROL - GAS FLOW CONTROL SYSTEM
echo               STANDALONE EXE DERLEME ARACI
echo =======================================================================
echo.
echo [1/3] Mevcut derleme temizleniyor...
if exist "Dagitim_Paketi\hava_kontrol.exe" del /f /q "Dagitim_Paketi\hava_kontrol.exe"
if exist "Dagitim_Paketi\hava_kontrol.pdb" del /f /q "Dagitim_Paketi\hava_kontrol.pdb"

echo.
echo [2/3] .NET 8 Self-Contained Single-File (win-x64) olarak derleniyor...
echo (Bu islem .NET Desktop Runtime, OpenCV ve tum bagimliliklari EXE icine gommektedir)
echo.

dotnet publish "hava_kontrol\hava_kontrol\hava_kontrol.csproj" /p:PublishProfile=FolderProfile -c Release

if exist "Dagitim_Paketi\hava_kontrol.pdb" del /f /q "Dagitim_Paketi\hava_kontrol.pdb"

if %ERRORLEVEL% NEQ 0 (
    echo.
    echo [HATA] Derleme sirasinda bir hata meydana geldi! Lutfen hata mesajlarini kontrol edin.
    pause
    exit /b %ERRORLEVEL%
)

echo.
echo [3/3] Derleme basariyla tamamlandi!
echo.
echo Olusturulan EXE konumu:
echo %~dp0Dagitim_Paketi\hava_kontrol.exe
echo.
echo Bu "Dagitim_Paketi" klasorunu bir USB bellege atarak veya zipleyerek
echo hedef bilgisayara goturebilirsiniz. Hedef bilgisayarda hicbir .NET
echo veya gelistirici aracinin kurulu olmasina gerek YOKTUR.
echo.
echo =======================================================================
echo Islem tamamlandi. Klasor simdi aciliyor...
explorer "%~dp0Dagitim_Paketi"
pause
