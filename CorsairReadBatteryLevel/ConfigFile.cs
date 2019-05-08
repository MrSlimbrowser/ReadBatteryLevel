using System;
using System.IO;
using System.Reflection;

namespace ReadBatteryLevel
{
    internal class ConfigFile
    {
        internal string processname { get; }
        internal bool calculateBaseAddress { get; }
        internal Int32 useThreadstackNumber { get; }
        internal Int32 baseAddress { get; }
        internal Int32 negativeOffset { get; } // ignored when calculateBaseAddress==false
        internal Int32[] offsets { get; }
        internal string resultFilePath { get; }

        internal ConfigFile(string filepath)
        {
            this.processname = "iCUE.exe";
            this.calculateBaseAddress = true;
            this.useThreadstackNumber = 0;
            this.baseAddress = 0x0;
            this.negativeOffset = 0xBE8;
            this.offsets = new Int32[] { 0x10, 0x4, 0xC, 0x18, 0x34 };
            this.resultFilePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase)
                                     + "\\" + Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().CodeBase)
                                     + ".result";
            this. resultFilePath = this.resultFilePath.Remove(0, 6);

            if (!File.Exists(filepath))
            {
                CreateConfigFile(filepath);
            }
            else
            {
                string[] fileContent = File.ReadAllLines(filepath);

                bool useCustomOffsets = false;
                foreach (string str in fileContent)
                {
                    string[] strSplit = str.Split('=');
                    Int32 tempInt = 0;
                    Int32 tempOffset = 0x0;
                    bool tempBool = true;

                    if (strSplit.Length != 2)
                        continue;

                    switch (strSplit[0].ToLower())
                    {
                        case "processname":
                            this.processname = str.Split('=')[1];
                            break;

                        case "calculatebaseaddress":
                            tempBool = true;
                            bool.TryParse(str.Split('=')[1], out tempBool);
                            this.calculateBaseAddress = tempBool;
                            break;
                        case "usethreadstacknumber=":
                            this.useThreadstackNumber = ConvertStrToInt32(strSplit[1]);
                            break;
                        case "baseaddres":
                                this.baseAddress = ConvertStrToInt32(strSplit[1]);
                            break;
                        case "negativeoffset":
                                this.negativeOffset = ConvertStrToInt32(strSplit[1]);
                            break;

                        case "offset1":
                        case "offset2":
                        case "offset3":
                        case "offset4":
                        case "offset5":
                            // custom offsets found -> use custom values or default to 0x0
                            if (!useCustomOffsets)
                            {
                                for (int i = 0; i < offsets.Length; i++)
                                {
                                    offsets[i] = 0x0;
                                }
                                useCustomOffsets = true;
                            }

                            // offset value
                            tempOffset = ConvertStrToInt32(strSplit[1]);
                            // offset number
                            tempInt = ConvertStrToInt32(strSplit[0].Substring(strSplit[0].Length - 1, 1)) - 1;

                            this.offsets[tempInt] = tempOffset;
                            break;
                        case "resultfilepath":
                            if (strSplit[1].Length > 0)
                            {
                                this.resultFilePath = strSplit[1];
                            }
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        private static void CreateConfigFile(string filepath)
        {
            try
            {
                //string[] lines = { "Example config file, change values to fit your needs." , "If you choose not to let the program calculate the base address then NegativeOffset will be ignored."
                //        , "In case of an error the program might return 999 in it's result file."
                //        , "I am not responsible for any unwanted behavior, use the program at your own risk!", ""
                //        , "Processname=iCUE", "CalculateBaseAddress=true", "BaseAddress=0x0", "NegativeOffset=0xBE8"
                //        , "Offset1=0x10", "Offset2=0x4", "Offset3=0xC", "Offset4=0x18", "Offset5=0x34" };

                string[] lines = {
                   "Welcome to this little tool. Use it to read your headphones battery level from iCUE's memory.",
                   "Be aware that this tool is NOT an official tool from Corsair!",
                   "",
                   "Thanks to everyone who helped me creating this by providing helpful code online.",
                   "Thanks to user1274820 for providing the answer over there https://stackoverflow.com/questions/28620186/using-pointers-found-in-cheat-engine-in-c-sharp",
                   "Special thanks to MakeMEK@github for creating the cheatengine - threadstack - finder(threadstack.exe).I hope it is okay for him that I use it for my program too.",
                   "Check out his github: https://github.com/makemek/cheatengine-threadstack-finder",
                   "and his explanation on what his program does https://forum.cheatengine.org/viewtopic.php?p=5638945&sid=a9dad6c894a943ff2674d349a1de259c#5638945",
                   "Special thanks to the creators of cheatengine for their awesome work: https://www.cheatengine.org/",
                   "",
                   "",
                   "DISCLAIMER:",
                   "Use my program at your own risk! I am not responsible for any damages or unwanted behavior.",
                   "I am not a programmer, if I need a functionality I try to build it myself making big use of google search, stackoverflow, github, etc.",
                   "I most probably don't know if I make horrible, hidden mistakes.",
                   "Therefore, please, be careful or review the code yourself.",
                   "",
                   "",
                   "Processname=iCUE",
                   "CalculateBaseAddress=true",
                   "useThreadstackNumber=0",
                   "Offset5=0x34",
                   "Offset4=0x18",
                   "Offset3=0xC",
                   "Offset2=0x4",
                   "Offset1=0x10",
                   "NegativeOffset=0xBE8",
                   "BaseAddress=0x0",
                   "ResultFilePath="
                };
                System.IO.File.WriteAllLines(filepath, lines);
            }
            catch
            {
                
            }
        }

        private static int ConvertStrToInt32(string input)
        {
            try
            {
                return Convert.ToInt32(input, 16);
            }
            catch
            {
                return 0;
            }
        }
    }
}
