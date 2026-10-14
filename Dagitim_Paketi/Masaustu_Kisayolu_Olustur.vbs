Option Explicit
Dim WshShell, fso, strDesktop, strCurrentDir, strExePath, oShortcut, appName, lnkName, f

Set WshShell = CreateObject("WScript.Shell")
Set fso = CreateObject("Scripting.FileSystemObject")

strDesktop = WshShell.SpecialFolders("Desktop")
strCurrentDir = fso.GetParentFolderName(WScript.ScriptFullName)
strExePath = fso.BuildPath(strCurrentDir, "hava_kontrol.exe")

appName = "Hava Kontrol SCADA"
lnkName = appName & ".lnk"

If Not fso.FileExists(strExePath) Then
    MsgBox "Hata: 'hava_kontrol.exe' bu klasorde bulunamadi!" & vbCrLf & _
           "Lutfen bu betigi 'hava_kontrol.exe' dosyasinin bulundugu klasorun icinde calistirin.", vbCritical, appName
    WScript.Quit 1
End If

' Varsa eski kisayollari temizle
For Each f In fso.GetFolder(strDesktop).Files
    If InStr(f.Name, "Hava Kontrol") > 0 And LCase(fso.GetExtensionName(f.Name)) = "lnk" Then
        On Error Resume Next
        fso.DeleteFile f.Path, True
        On Error GoTo 0
    End If
Next

Set oShortcut = WshShell.CreateShortcut(fso.BuildPath(strDesktop, lnkName))
oShortcut.TargetPath = strExePath
oShortcut.WorkingDirectory = strCurrentDir
oShortcut.IconLocation = strExePath & ",0"
oShortcut.Description = appName & " - Gas Flow Control System"
oShortcut.Save

MsgBox "Masaustunuze '" & appName & "' kisayolu basariyla olusturuldu!" & vbCrLf & vbCrLf & _
       "Hedef: " & strExePath, vbInformation, appName
