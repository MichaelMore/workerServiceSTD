@ECHO OFF
net session >nul 2>&1
IF NOT %ERRORLEVEL% EQU 0 (
   ECHO �ХH Administrator �v������.
   PAUSE
   EXIT /B 1
)

ECHO ���b���� workerServiceSTD...
ECHO ---------------------------------------------------
net stop workerServiceSTD
ECHO ---------------------------------------------------

ECHO ���b�R�� workerServiceSTD...
ECHO ---------------------------------------------------
sc delete workerServiceSTD
ECHO ---------------------------------------------------

ECHO ����
PAUSE