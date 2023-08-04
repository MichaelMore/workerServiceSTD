@ECHO OFF
net session >nul 2>&1
IF NOT %ERRORLEVEL% EQU 0 (
   ECHO 錯誤: 請以 Administrator 權限執行.
   PAUSE
   EXIT /B 1
)

set batdir=%~dp0

ECHO 開始安裝 workerServiceSTD 服務...
ECHO ---------------------------------------------------
sc create "workerServiceSTD" binPath= "%batdir%..\workerServiceSTD.exe" DisplayName= "workerServiceSTD" Start=delayed-auto
ECHO ---------------------------------------------------

ECHO 正在設定 workerServiceSTD 服務名稱...
ECHO ---------------------------------------------------
sc description workerServiceSTD "workerServiceSTD"
ECHO ---------------------------------------------------

ECHO 正在啟動 workerServiceSTD 服務...
ECHO ---------------------------------------------------
net start "workerServiceSTD"
ECHO ---------------------------------------------------

ECHO 結束
PAUSE