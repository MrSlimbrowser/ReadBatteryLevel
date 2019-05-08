# ReadBatteryLevel
Reads battery level of Corsair Void Pro headphones from iCUE's memory.

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

## Known Issues:
When the headphones are turned off the last known battery level will be shown.

## Finding Offsets:
Use CheatEngine to find a suitable pointer. There are many tutorials how to do that online, just pick any. Once you got the pointer double click on it to show the offsets and use them as shown in the picture below
<br>Notice: The battery level from iCUE is a value between 0 and 100 where something around 100 (often 97/98/99/) shows up when the headphones are plugged in.
<br>
![Image](https://mrslimbrowser.github.io/images/ReadBatteryLevel/FindOffsets.png)
