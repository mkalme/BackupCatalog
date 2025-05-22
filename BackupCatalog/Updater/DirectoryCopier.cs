using System;
using System.Collections.Generic;
using System.IO;

namespace BackupCatalog {
    class DirectoryCopier {
        public DirectoryCopier(string source, string dest){
            SourceDirectoryPath = source;
            DestDirectoryPath = dest;
        }

        public void Copy(UpdateProgress progress) {
            //Discovering
            progress.ProcessType = ProcessType.Discovering;

            List<FileInfo> files = new List<FileInfo>();
            List<DirectoryInfo> directories = new List<DirectoryInfo>();
            Discover(new DirectoryInfo(_sourceDirectoryPath), files, directories, progress);

            //Copying
            progress.ProcessType = ProcessType.Copying;

            foreach (var directory in directories) {
                string newPath = GetSubPath(_sourceDirectoryPath, DestDirectoryPath, directory.FullName);
                Directory.CreateDirectory(newPath);
            }

            for (int i = 0; i < files.Count; i++) {
                string newPath = GetSubPath(_sourceDirectoryPath, DestDirectoryPath, files[i].FullName);
                File.Copy(files[i].FullName, newPath);

                progress.Current++;
                progress.Percentage = progress.Current / (float)progress.Count;
                progress.CurrentItem = new ItemCopy(files[i].FullName, ItemType.File);
            }
        }

        private static void Discover(DirectoryInfo diTop, List<FileInfo> files, List<DirectoryInfo> directories, UpdateProgress progress){
            directories.Add(diTop);
            foreach (var fi in diTop.EnumerateFiles()) {
                files.Add(fi);
                progress.Count++;
            }

            foreach (var di in diTop.EnumerateDirectories()) {
                Discover(di, files, directories, progress);
            }
        }
        private string GetSubPath(string sourcePath, string destinationPath, string subPath) {
            return $"{destinationPath}\\{Name}\\{subPath.Substring(sourcePath.Length, subPath.Length - sourcePath.Length)}";
        }

        public string Name { get; private set; }

        private string _sourceDirectoryPath;
        public string SourceDirectoryPath {
            get => _sourceDirectoryPath;
            set {
                _sourceDirectoryPath = value;
                Name = Path.GetFileName(value);
            }
        }
        
        public string DestDirectoryPath { get; set; }
    }
}
