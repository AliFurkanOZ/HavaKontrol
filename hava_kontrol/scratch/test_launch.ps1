$exePath = "c:\Users\onure\OneDrive\Desktop\HavaKontrol\hava_kontrol\hava_kontrol\bin\Debug\net8.0-windows\hava_kontrol.exe"
$proc = Start-Process -FilePath $exePath -PassThru
Start-Sleep -Seconds 2
$running = (Get-Process -Id $proc.Id -ErrorAction SilentlyContinue) -ne $null
if ($running) {
    Stop-Process -Id $proc.Id -Force
    Write-Output "SUCCESS: App started cleanly and ran without crash!"
} else {
    Write-Output "FAILED: App exited prematurely with exit code: $($proc.ExitCode)"
}
