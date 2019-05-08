# ReadBatteryLevel
Reads battery level of Corsair Void Pro headphones from iCUE's memory.
iCUE must be installed and running for this tool to work.

## Known Issues:
When the headphones are turned off the last known battery level will be shown.

## Available options and their *default values*:
*Notice: the configuration file will be created on launch if it doesn’t exist yet*

|Option|Description|
| --- | --- |
|Processname=*iCUE*|process name from task manager, without .exe|
|CalculateBaseAddress=*true*|Whether or not threadstack.exe should be used to calculate the base address. If set to false, BaseAdress= is used instead and NegativeOffset= will be ignored.|
|useThreadstackNumber=*0*|Which result of threadstack.exe should be used as base address|
|Offset5=*0x34*|Offset as described below|
|Offset4=*0x18*|Offset as described below|
|Offset3=*0xC*|Offset as described below|
|Offset2=*0x4*|Offset as described below|
|Offset1=*0x10*|Offset as described below|
|NegativeOffset=*0xBE8*|First offset from BaseAddress, will be subtracted instead of added|
|BaseAddress=*0x0*|Custom base address used if CalculateBaseAddress is set to false.|
|ResultFilePath=|Custom file path for the result file. Result file contains either battery level or error code|

## Error Codes:
998 = error reading processes memory, threadstack.exe is missing or the process does not exist
<br>997 = battery level out of range, either headphones are not connected or offsets are incorrect
<br>996 = threadstack.exe unexpected output or given UseThreadstackNumber doesn't exist
<br>995 = threadstack.exe missing

## Finding Offsets:
Use CheatEngine to find a suitable pointer. There are many tutorials how to do that online, just pick any. Once you got the pointer double click on it to show the offsets and use them as shown in the picture below
<br>Notice: The battery level from iCUE is a value between 0 and 100 where something around 100 (often 97/98/99/) shows up when the headphones are plugged in.
<br>
![Image](https://mrslimbrowser.github.io/images/ReadBatteryLevel/FindOffsets.png)

## Thanks
Thanks to everyone who helped me creating this by providing helpful code online.
<p>Thanks to user1274820 for providing the answer over there https://stackoverflow.com/questions/28620186/using-pointers-found-in-cheat-engine-in-c-sharp
<p>Special thanks to MakeMEK@github for creating the cheatengine - threadstack - finder(threadstack.exe).I hope it is okay for him that I use it for my program too.
<br>Check out his github: https://github.com/makemek/cheatengine-threadstack-finder
<br>and his explanation on what his program does https://forum.cheatengine.org/viewtopic.php?p=5638945&sid=a9dad6c894a943ff2674d349a1de259c#5638945
<p>Special thanks to the creators of cheatengine for their awesome work: https://www.cheatengine.org/

