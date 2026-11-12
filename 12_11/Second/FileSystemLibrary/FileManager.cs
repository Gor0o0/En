
using System;
using System.IO;

namespace FileSystemLibrary
{
    public static class FileManager
    {
        public static void CopyFile(string source, string destination)
        {
            File.Copy(source, destination, true);
        }
        public static void CopyDirectory(string source, string destination)
        {
            Directory.CreateDirectory(destination);

            foreach (string file in Directory.GetFiles(source))
            {
                string fileName = Path.GetFileName(file);

                File.Copy(
                    file,
                    Path.Combine(destination, fileName),
                    true
                );
            }

            foreach (string directory in Directory.GetDirectories(source))
            {
                string directoryName = Path.GetFileName(directory);

                CopyDirectory(
                    directory,
                    Path.Combine(destination, directoryName)
                );
            }
        }
        public static void DeleteFile(string fileName)
        {
            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }
        }
        public static void DeleteFiles(string[] fileNames)
        {
            foreach (string fileName in fileNames)
            {
                if (File.Exists(fileName))
                {
                    File.Delete(fileName);
                }
            }
        }
        public static void DeleteFilesByMask(string directory, string mask)
        {
            string[] files = Directory.GetFiles(directory, mask);

            foreach (string file in files)
            {
                File.Delete(file);
            }
        }
        public static void MoveFile(string source, string destination)
        {
            File.Move(source, destination, true);
        }
    }
}
