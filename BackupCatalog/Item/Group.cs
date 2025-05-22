using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FastStream;

namespace BackupCatalog {
    public class Group : ItemCollection, IEnumerable {
        private List<Item> _items = new List<Item>();
        public List<Item> Items => _items.AsReadOnly().ToList();

        public Group() { 
        }
        public Group(string name) : base(name) {
        }
        internal Group(Item item) : base(item) {

        }

        //Events
        private event EventHandler _contentChange;
        public override event EventHandler ContentChange {
            add {
                _contentChange += value;
                foreach (var bundle in _items) {
                    bundle.PropertyChange += value;
                }
            }
            remove {
                _contentChange -= value;
                foreach (var bundle in _items) {
                    bundle.PropertyChange -= value;
                }
            }
        }

        private void OnContentChange(){
            if (_contentChange != null)
                _contentChange.Invoke(this, EventArgs.Empty);
        }

        //Methods
        public void Add(Item item) {
            _items.AddChild(item, this);
            OnContentChange();
        }
        public void RemoveAt(int index) {
            Item item = _items[index];
            _items.RemoveAt(index);

            item._parent = null;
            item.PropertyChange -= PropertyChange;
            item.Delete();

            OnContentChange();
        }
        public void Remove(Item item) {
            if (!_items.RemoveChild(item)) return;

            item.PropertyChange -= PropertyChange;
            item.Delete();
            OnContentChange();
        }
        public void Clear() {
            if (_items.Count < 1) return;

            foreach (var item in _items) {
                item._parent = null;
                item.PropertyChange -= PropertyChange;
                item.Delete();
            }
            _items.Clear();

            OnContentChange();
        }

        public override void Update(UpdateProgress progress){
            progress.State = ProgressState.Running;

            Updater.UpdateDirectories(GetAllDirectories(), progress);
            progress.Reset();

            Updater.UpdateFiles(GetAllFiles(), progress);

            progress.State = ProgressState.Finished;
        }

        //Abstract methods from item collection class
        public override string GetNewItemName(string fileName, Type type){
            return _items.GetItemName(type, fileName);
        }
        public override void AddItem(Item item){
            Add(item);
        }
        public override void RemoveItem(Item item){
            Remove(item);
        }
        public override List<Item> GetItems(){
            return _items;
        }
        public override List<DirectoryBackup> GetAllDirectories(){
            List<DirectoryBackup> directories = new List<DirectoryBackup>();

            foreach (var item in _items) {
                if (item.GetType().IsSubclassOf(typeof(ItemCollection))) {
                    directories.AddRange(((ItemCollection)item).GetAllDirectories());
                } else if (item.GetType() == typeof(DirectoryBackup)) {
                    directories.Add((DirectoryBackup)item);
                }
            }

            return directories;
        }
        public override List<FileBackup> GetAllFiles(){
            List<FileBackup> files = new List<FileBackup>();

            foreach (var item in _items) {
                if (item.GetType().IsSubclassOf(typeof(ItemCollection))) {
                    files.AddRange(((ItemCollection)item).GetAllFiles());
                } else if (item.GetType() == typeof(FileBackup)) {
                    files.Add((FileBackup)item);
                }
            }

            return files;
        }
        public override bool IsItemTypeAccepted(Type type){
            return type.IsSubclassOf(typeof(Item));
        }

        //Serialize
        public byte[] ToBytes() {
            using (MemoryStream stream = new MemoryStream())
            using (FastBinaryWriter writer = new FastBinaryWriter(stream)) {
                Serialize(writer);

                return stream.ToArray();
            }
        }
        public void SaveToFile(string path) {
            using (MemoryStream stream = new MemoryStream())
            using (FastBinaryWriter writer = new FastBinaryWriter(stream)) {
                Serialize(writer);

                File.WriteAllBytes(path, stream.ToArray());
            }
        }
        public void WriteToStream(FastBinaryWriter writer) {
            Serialize(writer);
        }

        internal override void Serialize(FastBinaryWriter writer) {
            base.Serialize(writer);

            writer.Write(_items.Count);
            foreach (var item in _items) {
                item.SerializeID(writer);
                item.Serialize(writer);
            }
        }

        //Deserialize
        public static Group FromBytes(byte[] bytes) {
            return Deserialize(new FastBinaryReader(bytes));
        }
        public static Group FromFile(string path) {
            return Group.Deserialize(new FastBinaryReader(File.ReadAllBytes(path)));
        }

        public new static Group Deserialize(FastBinaryReader reader) {
            Group group = new Group(Item.Deserialize(reader));

            int count = reader.ReadInt32();
            while (count > 0) {
                byte id = reader.ReadByte();

                Item item = ItemHelper.GetItem(id.GetTypeFromID(), reader);
                item._parent = group;
                group._items.Add(item);

                count--;
            }

            return group;
        }

        //Default Methods
        public override bool Equals(Item item){
            if (!base.Equals(item)) return false;

            Group group = item as Group;

            return _items.Compare(group._items);
        }
        public override object Clone(){
            Group group = new Group((Item)base.Clone());

            group._items = _items.CloneList();
            foreach (var item in group._items) {
                item._parent = group;
            }

            return group;
        }
        public IEnumerator GetEnumerator() => _items.GetEnumerator();
    }
}
