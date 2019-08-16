@echo off
echo DnsPingScript
set Interval=5
set file=E:/DnsPingInfo.tc
:PingInterval
echo\ 1>>%file% 2>>&1
echo\ 1>>%file% 2>>&1
echo --Begin-- 1>>%file% 2>>&1
echo %date% %time% 1>>%file% 2>>&1
if errorlevel 1 ipconfig /displaydns 1>>%file% 2>>&1
echo --End-- 1>>%file% 2>>&1
timeout /T %Interval% /Nobreak
goto PingInterval