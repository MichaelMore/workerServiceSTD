@ECHO OFF
net session >nul 2>&1
IF NOT %ERRORLEVEL% EQU 0 (
   ECHO ���~: �ХH Administrator �v������.
   PAUSE
   EXIT /B 1
)

set batdir=%~dp0

ECHO �}�l�w�� workerServiceSTD �A��...
ECHO ---------------------------------------------------
sc create "workerServiceSTD" binPath= "%batdir%..\workerServiceSTD.exe" DisplayName= "workerServiceSTD" Start=delayed-auto
ECHO ---------------------------------------------------

ECHO ���b�]�w workerServiceSTD �A�ȦW��...
ECHO ---------------------------------------------------
sc description workerServiceSTD "workerServiceSTD"
ECHO ---------------------------------------------------

ECHO ���b�Ұ� workerServiceSTD �A��...
ECHO ---------------------------------------------------
net start "workerServiceSTD"
ECHO ---------------------------------------------------

ECHO ����
PAUSE