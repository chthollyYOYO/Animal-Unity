using System.Diagnostics;
using System.IO;

namespace GameTools
{
    public class ToolsHelper
    {
        public static void RunBat(string path, params string[] arguments)
        {
            ProcessStartInfo startInfo = new();
            startInfo.FileName = path;
            startInfo.WorkingDirectory = Path.GetDirectoryName(path);
            startInfo.Arguments = string.Join(" ", arguments);
            using Process process = Process.Start(startInfo);
            process.WaitForExit();
        }

        public static void SvnCommit(string path, string logMsg)
        {
            ProcessStartInfo startInfo = new();
            startInfo.FileName = "TortoiseProc.exe";
            startInfo.WorkingDirectory = Path.GetDirectoryName(path);
            startInfo.Arguments = $"/command:commit + /path:{path} + /logMsg:{logMsg}";
            using Process process = Process.Start(startInfo);
            process.WaitForExit();
        }

        public static void DeleteDirectory(string folderPath)
        {
            if (Directory.Exists(folderPath))
            {
                string[] filePaths = Directory.GetFiles(folderPath);
                foreach (string filePath in filePaths)
                {
                    File.Delete(filePath);
                }
                Directory.Delete(folderPath);
            }
        }
    }
}
