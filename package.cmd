@echo off
NuGet.exe update -self
NuGet.exe pack "Intervals\Intervals.csproj"  -Build -Symbols -Properties Configuration=Release
pause
