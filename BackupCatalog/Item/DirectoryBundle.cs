using System;
using System.Collections.Generic;
using System.Linq;
using FastStream;

namespace BackupCatalog {
    public class DirectoryBundle : ItemBundle {
        private List<DirectoryBundle> _bundles = new List<DirectoryBundle>();
        public List<DirectoryBundle> Bundles => _bundles.AsReadOnly().ToList();

        private List<DirectoryBackup> _directories = new List<DirectoryBackup>();
        public List<DirectoryBackup> Directories => _directories.AsReadOnly().ToList();

        public DirectoryBundle() { 
        }
        internal DirectoryBundle(Item item) : base(item) {
        }
        public DirectoryBundle(string name) : base(name) { 
        }

        //Event
        private event EventHandler _contentChange;
        public override event EventHandler ContentChange {
            add {
                _contentChange += value;
                foreach (var bundle in _bundles) {
                    bundle.PropertyChange += value;
                }
                foreach (var file in _directories) {
                    file.PropertyChange += value;
                }
            }
            remove {
                _contentChange -= value;
                foreach (var bundle in _bundles) {
                    bundle.PropertyChange -= value;
                }
                foreach (var file in _directories) {
                    file.PropertyChange -= value;
                }
            }
        }

        private void OnContentChange(){
            if (_contentChange != null)
                _contentChange.Invoke(this, EventArgs.Empty);
        }

        //Methods
        public void AddBundle(DirectoryBundle bundle) {
            _bundles.AddChild(bundle, this);
            OnContentChange();
        }
        public void RemoveBundle(DirectoryBundle bundle) {
            if (!_bundles.RemoveChild(bundle)) return;

            bundle.PropertyChange -= PropertyChange;
            bundle.Delete();
            OnContentChange();
        }

        public void AddDirectory(DirectoryBackup directory){
            _directories.AddChild(directory, this);
            OnContentChange();
        }
        public void RemoveDirectory(DirectoryBackup directory){
            if (!_directories.RemoveChild(directory)) return;

            directory.PropertyChange -= PropertyChange;
            directory.Delete();
            OnContentChange();
        }

        public override void Update(UpdateProgress progress){
            progress.State = ProgressState.Running;
            Updater.UpdateDirectories(GetAllDirectories(), progress);
            progress.State = ProgressState.Finished;
        }
        public void Update(DirectoryBundle bundle){
            base.Update(bundle);

            _bundles = new List<DirectoryBundle>(bundle._bundles);
            _directories = new List<DirectoryBackup>(bundle._directories);

            OnPropertyChange();
        }

        //Abstract methods from item bundle class
        public override string GetNewItemName(string fileName, Type type){
            if (type == typeof(DirectoryBundle)) return _bundles.GetNewName(fileName);

            return _directories.GetNewName(fileName);
        }
        public override void AddItem(Item item){
            if (item.GetType() == typeof(DirectoryBundle)) AddBundle(item as DirectoryBundle);
            if (item.GetType() == typeof(DirectoryBackup)) AddDirectory(item as DirectoryBackup);
        }
        public override void RemoveItem(Item item){
            if (item.GetType() == typeof(DirectoryBundle)) RemoveBundle(item as DirectoryBundle);
            if (item.GetType() == typeof(DirectoryBackup)) RemoveDirectory(item as DirectoryBackup);
        }
        public override List<Item> GetItems(){
            List<Item> items = new List<Item>(_bundles);
            items.AddRange(_directories);

            return items;
        }
        public override List<DirectoryBackup> GetAllDirectories(){
            List<DirectoryBackup> directories = new List<DirectoryBackup>();
            foreach (var bundle in _bundles) {
                directories.AddRange(bundle.GetAllDirectories());
            }

            directories.AddRange(_directories);

            return directories;
        }
        public override List<FileBackup> GetAllFiles(){
            return new List<FileBackup>();
        }
        public override DateTime GetLastUpdateDate(){
            DateTime bundleDate = DateTime.MaxValue;
            foreach (var bundle in _bundles) {
                DateTime update = bundle.GetLastUpdateDate();
                if (update < bundleDate) bundleDate = update;
            }

            DateTime directoryDate = DateTime.MaxValue;
            foreach (var directory in _directories) {
                DateTime update = directory.UpdateDate;
                if (update < directoryDate) directoryDate = update;
            }

            return bundleDate < directoryDate ? bundleDate.Convert() : directoryDate.Convert();
        }
        public override bool IsItemTypeAccepted(Type type){
            return type == typeof(DirectoryBundle) || type == typeof(DirectoryBackup);
        }

        //Serialize
        internal override void Serialize(FastBinaryWriter writer){
            base.Serialize(writer);

            writer.Write(_bundles.Count);
            foreach (var bundle in _bundles) {
                bundle.Serialize(writer);
            }

            writer.Write(_directories.Count);
            foreach (var directory in _directories) {
                directory.Serialize(writer);
            }
        }

        //Deserialize
        internal new static DirectoryBundle Deserialize(FastBinaryReader reader) {
            DirectoryBundle bundle = new DirectoryBundle(Item.Deserialize(reader));

            int bundleCount = reader.ReadInt32();
            while (bundleCount > 0) {
                DirectoryBundle d = Deserialize(reader);
                d._parent = bundle;
                bundle._bundles.Add(d);

                bundleCount--;
            }

            int directoryCount = reader.ReadInt32();
            while (directoryCount > 0) {
                DirectoryBackup d = DirectoryBackup.Deserialize(reader);
                d._parent = bundle;
                bundle._directories.Add(d);
                
                directoryCount--;
            }

            return bundle;
        }

        //Default Methods
        public override bool Equals(Item item){
            if (!base.Equals(item)) return false;

            DirectoryBundle bundle = item as DirectoryBundle;

            if (!_bundles.Compare(bundle._bundles)) return false;
            return _directories.Compare(bundle._directories);
        }
        public override object Clone(){
            DirectoryBundle bundle = new DirectoryBundle((Item)base.Clone());

            bundle._bundles = _bundles.CloneList();
            foreach (var b in bundle._bundles) {
                b._parent = bundle;
            }
            
            bundle._directories = _directories.CloneList();
            foreach (var directory in bundle._directories) {
                directory._parent = bundle;
            }

            return bundle;
        }
    }
}
