using System;
using System.IO;
using System.Collections.Generic;

namespace BackupCatalog {
    static class DirectoryBundleCopier {
        public static void Copy(List<DirectoryBackup> backups, UpdateProgress progress) {
            //Discovering
            progress.ProcessType = ProcessType.Discovering;

            List<FileInfo>[] files = new List<FileInfo>[backups.Count];
            List<DirectoryInfo>[] directories = new List<DirectoryInfo>[backups.Count];
            int fileCount = 0;
            for (int i = 0; i < backups.Count; i++) {
                files[i] = new List<FileInfo>();
                directories[i] = new List<DirectoryInfo>();

                Discover(new DirectoryInfo(backups[i].SourcePath), files[i], directories[i]);
                fileCount += files[i].Count;
            }

            //Copying
            progress.ProcessType = ProcessType.Copying;
            int fileIndex = 0;

            for (int i = 0; i < backups.Count; i++) {
                var backup = backups[i];
                string name = Path.GetFileName(backup.SourcePath);

                var fileProgress = new UpdateProgress() { Count = files[i].Count };
                fileProgress.ProcessType = ProcessType.Copying;

                progress.CurrentDirectoryProgress = fileProgress;
                progress.CurrentItem = new ItemCopy(backup.SourcePath, ItemType.Directory);

                List<DirectoryInfo> directoryList = directories[i];
                foreach (var directory in directoryList) {
                    string newPath = GetSubPath(backup.SourcePath, backup.DestinationFolder, directory.FullName, name);
                    Directory.CreateDirectory(newPath);
                }

                List<FileInfo> fileList = files[i];
                foreach (var file in fileList) {
                    string newPath = GetSubPath(backup.SourcePath, backup.DestinationFolder, file.FullName, name);
                    File.Copy(file.FullName, newPath);

                    fileProgress.Current++;
                    fileProgress.Percentage = fileProgress.Current / (float)fileProgress.Count;
                    fileProgress.CurrentItem = new ItemCopy(file.FullName, ItemType.File);

                    fileIndex++;
                    progress.Percentage = fileIndex / (float)fileCount;
                }

                backup.UpdateHistory.Update();
                progress.Current++;
            }
        }

        private static void Discover(DirectoryInfo diTop, List<FileInfo> files, List<DirectoryInfo> directories){
            directories.Add(diTop);
            foreach (var fi in diTop.EnumerateFiles()) {
                files.Add(fi);
            }

            foreach (var di in diTop.EnumerateDirectories()) {
                Discover(di, files, directories);
            }
        }
        private static string GetSubPath(string sourcePath, string destinationPath, string subPath, string name){
            return $"{destinationPath}\\{name}\\{subPath.Substring(sourcePath.Length, subPath.Length - sourcePath.Length)}";
        }
    }
}
