using System.IO;
using System.Management;

namespace LauncherRework
{
    internal class HardwareID
    {
        // Make UniqueHWID
        public static string UniqueHWID(string drive)
        {
            if (drive == string.Empty)
                foreach (var compDrive in DriveInfo.GetDrives())
                    if (compDrive.IsReady)
                    {
                        drive = compDrive.RootDirectory.ToString();
                        break;
                    }

            if (drive.EndsWith(":\\"))
                drive = drive.Substring(0, drive.Length - 2);

            var _VOLUMESERIAL = VolumeSerial(drive);
            var _CPUID = CPUID();

            //Mix them up and remove some useless 0's
            return _CPUID.Substring(13) + _CPUID.Substring(1, 4) + _VOLUMESERIAL + _CPUID.Substring(4, 4);
        }

        // Get Volume Serial
        public static string VolumeSerial(string drive)
        {
            var disk = new ManagementObject(@"win32_logicaldisk.deviceid=""" + drive + @":""");
            disk.Get();

            var volumeSerial = disk["VolumeSerialNumber"].ToString();
            disk.Dispose();

            return volumeSerial;
        }

        // Get CPU ID
        public static string CPUID()
        {
            var cpuInfo = "";
            var managClass = new ManagementClass("win32_processor");
            var managCollec = managClass.GetInstances();

            foreach (ManagementObject managObj in managCollec)
                if (cpuInfo == "")
                {
                    cpuInfo = managObj.Properties["processorID"].Value.ToString();
                    break;
                }

            return cpuInfo;
        }
    }
}