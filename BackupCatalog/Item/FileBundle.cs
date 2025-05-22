using System;
using System.Collections.Generic;
using System.Linq;
using FastStream;

namespace BackupCatalog {
    public class FileBundle : ItemBundle {
        private List<FileBundle> _bundles = new List<FileBundle>();
        public List<FileBundle> Bundles => _bundles.AsReadOnly().ToList();

        private List<FileBackup> _files = new List<FileBackup>();
        public List<FileBackup> Files => _files.AsReadOnly().ToList();

        public FileBundle() { 
        }
        internal FileBundle(Item item) : base(item){
        }
        public FileBundle(string name) : base(name){
        }

        //Event
        private event EventHandler _contentChange;
        public override event EventHandler ContentChange {
            add {
                _contentChange += value;
                foreach (var bundle in _bundles) {
                    bundle.PropertyChange += value;
                }
                foreach (var file in _files) {
                    file.PropertyChange += value;
                }
            }
            remove {
                _contentChange -= value;
                foreach (var bundle in _bundles) {
                    bundle.PropertyChange -= value;
                }
                foreach (var file in _files) {
                    file.PropertyChange -= value;
                }
            }
        }

        private void OnContentChange() {
            if (_contentChange != null)
                _contentChange.Invoke(this, EventArgs.Empty);
        }

        //Methods
        public void AddBundle(FileBundle bundle){
            _bundles.AddChild(bundle, this);
            OnContentChange();
        }
        public void RemoveBundle(FileBundle bundle){
            if (!_bundles.RemoveChild(bundle)) return;

            bundle.PropertyChange -= PropertyChange;
            bundle.Delete();
            OnContentChange();
        }

        public void AddFile(FileBackup file){
            _files.AddChild(file, this);
            OnContentChange();
        }
        public void RemoveFile(FileBackup file){
            if (!_files.RemoveChild(file)) return;

            file.PropertyChange -= PropertyChange;
            file.Delete();
            OnContentChange();
        }

        public override void Update(UpdateProgress progress){
            progress.State = ProgressState.Running;
            Updater.UpdateFiles(GetAllFiles(), progress);
            progress.State = ProgressState.Finished;
        }
        public void Update(FileBundle bundle){
            base.Update(bundle);

            _bundles = new List<FileBundle>(bundle._bundles);
            _files = new List<FileBackup>(bundle._files);

            OnPropertyChange();
        }

        //Abstract methods from item bundle class
        public override string GetNewItemName(string fileName, Type type){
            if (type == typeof(FileBundle)) return _bundles.GetNewName(fileName);

            return _files.GetNewName(fileName);
        }
        public override void AddItem(Item item){
            if (item.GetType() == typeof(FileBundle)) AddBundle(item as FileBundle);
            if (item.GetType() == typeof(FileBackup)) AddFile(item as FileBackup);
        }
        public override void RemoveItem(Item item){
            if (item.GetType() == typeof(FileBundle)) RemoveBundle(item as FileBundle);
            if (item.GetType() == typeof(FileBackup)) RemoveFile(item as FileBackup);
        }
        public override List<Item> GetItems(){
            List<Item> items = new List<Item>(_bundles);
            items.AddRange(_files);

            return items;
        }
        public override List<DirectoryBackup> GetAllDirectories(){
            return new List<DirectoryBackup>();
        }
        public override List<FileBackup> GetAllFiles(){
            List<FileBackup> files = new List<FileBackup>();
            foreach (var bundle in _bundles) {
                files.AddRange(bundle.GetAllFiles());
            }

            files.AddRange(_files);

            return files;
        }
        public override DateTime GetLastUpdateDate(){
            DateTime bundleDate = DateTime.MaxValue;
            foreach (var bundle in _bundles) {
                DateTime update = bundle.GetLastUpdateDate();
                if (update < bundleDate) bundleDate = update;
            }

            DateTime fileDate = DateTime.MaxValue;
            foreach (var file in _files) {
                DateTime update = file.UpdateDate;
                if (update < fileDate) fileDate = update;
            }

            return bundleDate < fileDate ? bundleDate.Convert() : fileDate.Convert();
        }
        public override bool IsItemTypeAccepted(Type type){
            return type == typeof(FileBundle) || type == typeof(FileBackup);
        }

        //Serialize
        internal override void Serialize(FastBinaryWriter writer){
            base.Serialize(writer);

            writer.Write(_bundles.Count);
            foreach (var bundle in _bundles) {
                bundle.Serialize(writer);
            }

            writer.Write(_files.Count);
            foreach (var directory in _files) {
                directory.Serialize(writer);
            }
        }

        //Deserialize
        internal new static FileBundle Deserialize(FastBinaryReader reader){
            FileBundle bundle = new FileBundle(Item.Deserialize(reader));

            int bundleCount = reader.ReadInt32();
            while (bundleCount > 0) {
                FileBundle f = Deserialize(reader);
                f._parent = bundle;
                bundle._bundles.Add(f);

                bundleCount--;
            }

            int fileCount = reader.ReadInt32();
            while (fileCount > 0) {
                FileBackup f = FileBackup.Deserialize(reader);
                f._parent = bundle;
                bundle._files.Add(f);

                fileCount--;
            }

            return bundle;
        }

        //Default Methods
        public override bool Equals(Item item){
            if (!base.Equals(item)) return false;

            FileBundle bundle = item as FileBundle;

            if (!_bundles.Compare(bundle._bundles)) return false;
            return _files.Compare(bundle._files);
        }
        public override object Clone(){
            FileBundle bundle = new FileBundle((Item)base.Clone());

            bundle._bundles = _bundles.CloneList();
            foreach (var b in bundle._bundles) {
                b._parent = bundle;
            }

            bundle._files = _files.CloneList();
            foreach (var file in bundle._files) {
                file._parent = bundle;
            }

            return bundle;
        }
    }
}
