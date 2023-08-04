@ECHO OFF
net session >nul 2>&1
IF NOT %ERRORLEVEL% EQU 0 (
   ECHO 請以 Administrator 權限執行.
   PAUSE
   EXIT /B 1
)

ECHO 正在停止 workerServiceSTD...
ECHO ---------------------------------------------------
net stop workerServiceSTD
ECHO ---------------------------------------------------

ECHO 正在刪除 workerServiceSTD...
ECHO ---------------------------------------------------
sc delete workerServiceSTD
ECHO ---------------------------------------------------

ECHO 完成
PAUSE