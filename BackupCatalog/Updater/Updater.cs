using System;
using System.Collections.Generic;
using System.IO;

namespace BackupCatalog {
    static class Updater {        
        public static void UpdateDirectory(DirectoryBackup backup, UpdateProgress progress) {
            if (!Directory.Exists(backup.SourcePath)) return;

            string headDirectory = GetHeadName(backup.SourcePath, backup.DestinationFolder);
            if (Directory.Exists(headDirectory)) {
                Directory.Delete(headDirectory, true);
            }
            Directory.CreateDirectory(headDirectory);

            DirectoryCopier copier = new DirectoryCopier(backup.SourcePath, backup.DestinationFolder);
            copier.Copy(progress);

            backup.UpdateHistory.Update();
        }
        public static void UpdateFile(FileBackup backup, UpdateProgress progress) {
            if (!File.Exists(backup.SourcePath)) return;

            progress.ProcessType = ProcessType.Copying;
            progress.CurrentItem = new ItemCopy(backup.SourcePath, ItemType.File);

            string fileName = GetHeadName(backup.SourcePath, backup.DestinationFolder);
            if (File.Exists(fileName)) {
                File.Delete(fileName);
            } else {
                Directory.CreateDirectory(Path.GetDirectoryName(fileName));
            }

            progress.Count++;
            File.Copy(backup.SourcePath, fileName);
            progress.Current++;
            progress.Percentage = progress.Current / (float)progress.Count;

            backup.UpdateHistory.Update();
        }

        public static void UpdateDirectories(List<DirectoryBackup> backups, UpdateProgress progress) {
            progress.Type = ItemType.Directory;
            progress.Count = backups.Count;

            foreach (var backup in backups) {
                if (!Directory.Exists(backup.SourcePath)) continue;

                string headDirectory = GetHeadName(backup.SourcePath, backup.DestinationFolder);
                if (Directory.Exists(headDirectory)) {
                    Directory.Delete(headDirectory, true);
                }
                Directory.CreateDirectory(headDirectory);
            }

            DirectoryBundleCopier.Copy(backups, progress);
        }
        public static void UpdateFiles(List<FileBackup> backups, UpdateProgress progress) {
            progress.ProcessType = ProcessType.Copying;

            progress.Count = backups.Count;
            foreach (var backup in backups) {
                if (!File.Exists(backup.SourcePath)) continue;

                progress.CurrentItem = new ItemCopy(backup.SourcePath, ItemType.File);

                string fileName = GetHeadName(backup.SourcePath, backup.DestinationFolder);
                if (File.Exists(fileName)) {
                    File.Delete(fileName);
                } else {
                    Directory.CreateDirectory(Path.GetDirectoryName(fileName));
                }

                File.Copy(backup.SourcePath, fileName);
                progress.Current++;
                progress.Percentage = progress.Current / (float)progress.Count;

                backup.UpdateHistory.Update();
            }
        }

        private static string GetHeadName(string sourcePath, string destinationPath){
            return $"{destinationPath}\\{Path.GetFileName(sourcePath)}";
        }
    }
}
