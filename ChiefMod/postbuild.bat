REM call with ./postbuild.bat $(SolutionDir) $(TargetDir) $(TargetName)
set SolutionDir=%1
set TargetDir=%2
set TargetName=%3

set TargetPath=%TargetDir%%TargetName%
set Release=%SolutionDir%Release\
set CopyPath=%Release%%TargetName%
set Zip=%Release%Release.zip

copy /Y %TargetPath%.dll %CopyPath%.dll
copy /Y %TargetPath%.pdb %CopyPath%.pdb

if exist %Zip% (Del %Zip%)

powershell Compress-Archive -Path %Release%* -DestinationPath %Zip% -Force