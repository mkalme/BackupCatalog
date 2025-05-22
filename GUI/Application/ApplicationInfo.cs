using System;
using System.IO;
using System.Reflection;
using BackupCatalog;

namespace GUI{
    public class ApplicationInfo {
        public static readonly string DirectoryPath = $"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}\\Catalog";
        public static readonly string FilePath = $"{DirectoryPath}\\BackupCatalog.bc";

        public static ApplicationInfo Current;

        public Group Group { get; set; }

        public static ApplicationInfo FromLocalFile() {            
            if (!CreateRoot()) {
                ApplicationInfo createdInfo = new ApplicationInfo();
                createdInfo.Group = new Group("Root");
                createdInfo.Save();

                return createdInfo;
            }

            ApplicationInfo info = new ApplicationInfo();
            info.Group = Group.FromFile(FilePath);

            return info;
        }
        private static bool CreateRoot() {
            bool exists = true;
            
            if (!Directory.Exists(DirectoryPath)) {
                Directory.CreateDirectory(DirectoryPath);
                exists = false;
            }

            if (!File.Exists(FilePath)) {
                var file = File.Create(FilePath);
                file.Close();

                exists = false;
            }

            return exists;
        }

        public void Save() {
            Group.SaveToFile(FilePath);
        }
    }
}
