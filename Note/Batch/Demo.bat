@echo off
echo Test
set s=Name
echo %s%
echo text >> info.tc
echo text >> info.txt
1>>info.txt 2>>&1