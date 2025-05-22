using System;
using System.IO;
using BackupCatalog;

namespace Test_UI {
    class Program {
        private static string Path = $"{Environment.GetFolderPath(Environment.SpecialFolder.Desktop)}\\20180725_111006.mp4";
        private static string DestinationFolder = $"{Environment.GetFolderPath(Environment.SpecialFolder.Desktop)}\\CopyTest";

        static void Main(string[] args) {
            //FileCopier copier = new FileCopier(Path, $"{DestinationFolder}\\{System.IO.Path.GetFileName(Path)}");
            //copier.OnProgressChanged += DisplayProgress;

            //copier.Copy();

            DateTime time = DateTime.Now;

            Directory.Delete(DestinationFolder, true);
            Directory.CreateDirectory(DestinationFolder);

            DirectoryCopier copier = new DirectoryCopier($"D:\\Attēli", DestinationFolder);
            copier.OnProgressChanged += DisplayProgress;

            copier.Copy();

            Console.WriteLine((DateTime.Now - time).TotalSeconds + " seconds");

            Console.ReadLine();
        }

        private static void DisplayProgress(double percentage) {
            Console.WriteLine(percentage);
        }

        private static long SizeOfTest(string directoryPath) {
            long bytes = 0;

            string[] directories = Directory.GetDirectories(directoryPath);
            foreach (var dir in directories) {
                bytes += SizeOfTest(dir);
            }

            string[] files = Directory.GetFiles(directoryPath);
            foreach (var file in files) {
                bytes += new FileInfo(file).Length;
            }

            return bytes;
        }
        private static long SizeOfTestNew(DirectoryInfo diTop) {
            long bytes = 0;

            foreach (var fi in diTop.EnumerateFiles()) {
                bytes += fi.Length;
            }

            foreach (var di in diTop.EnumerateDirectories()) {
                bytes += SizeOfTestNew(di);
            }

            return bytes;
        }
    }
}
