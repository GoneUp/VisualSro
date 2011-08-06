@echo off

BatchProcess.exe compress_lzma "content"

echo ==================================================

choice /C yn /N /M "Clear non lzma ? [y]es, [n]o:"
If ErrorLevel 1 goto :clear
If ErrorLevel 2 exit

:clear
BatchProcess.exe clean_non_lzma "content"

echo ==================================================

PAUSE