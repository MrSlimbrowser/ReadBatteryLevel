using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;

namespace ReadBatteryLevel
{
    internal class Program
    {
        const int PROCESS_WM_READ = 0x0010;

        [DllImport("kernel32.dll")]
        private static extern IntPtr OpenProcess(int dwDesiredAccess, bool bInheritHandle, int dwProcessId);

        [DllImport("kernel32.dll")]
        private static extern bool ReadProcessMemory(int hProcess, int lpBaseAddress, 
            byte[] lpBuffer, int dwSize, ref int lpNumberOfBytesRead);

        private static void Main(string[] args)
        {
            // read config file -- filename follows assembly name but using .ini instead of .exe
            string configFilePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase)
                + "\\" + Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().CodeBase)
                + ".ini";
            configFilePath = configFilePath.Remove(0, 6);

            ConfigFile conf = new ConfigFile(configFilePath);

            Int32 batteryLevel = GetBatteryLevel(conf);

            try
            {
                File.WriteAllText(conf.resultFilePath, batteryLevel.ToString());
            }
            catch { }
        }

        private static int GetBatteryLevel(ConfigFile conf)
        {
            try
            {
                Process process = Process.GetProcessesByName("iCUE")[0];
                IntPtr processHandle = OpenProcess(PROCESS_WM_READ, false, process.Id);


                int bytesRead = 0;
                byte[] buffer = new byte[8]; // 8 = pointer address length
                Int32 baseAddr = 0x0; // must be calculated
                Int32 Addr = 0x0;

                // get baseAddr
                if (conf.calculateBaseAddress)
                {
                    baseAddr = GetThread0Address(process.Id, conf.useThreadstackNumber) - conf.negativeOffset;
                }
                else
                {
                    baseAddr = conf.baseAddress;
                }
                ReadProcessMemory((int)processHandle, baseAddr, buffer, buffer.Length, ref bytesRead);
                Addr = BitConverter.ToInt32(buffer, 0);

                // apply offsets
                foreach (int offs in conf.offsets)
                {
                    // Console.WriteLine("pointer address: " + Addr.ToString());
                    // read value at address Addr 
                    ReadProcessMemory((int)processHandle, Addr + offs, buffer, buffer.Length, ref bytesRead);
                    // write value to Addr
                    Addr = BitConverter.ToInt32(buffer, 0);
                    // Console.WriteLine("pointer value: " + Addr.ToString());
                }

                if (Addr < 0 || Addr > 100)
                    return 997;
                else
                    return Addr;
            }
            catch
            {
                return 998;
            }
        }
        private static int GetThread0Address(int pid, int threadstackNumber)
        {
            var proc = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "threadstack.exe",
                    Arguments = pid.ToString(),
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true
                }
            };
            try
            {
                proc.Start();
                while (!proc.StandardOutput.EndOfStream)
                {
                    string line = proc.StandardOutput.ReadLine();
                    if (line.Contains("THREADSTACK " + threadstackNumber + " BASE ADDRESS: "))
                    {
                        line = line.Substring(line.LastIndexOf(":") + 2);
                        return int.Parse(line.Substring(2), System.Globalization.NumberStyles.HexNumber);
                    }
                }
                return 996;
            }
            catch
            {
                return 995;
            }
        }
    }
}
