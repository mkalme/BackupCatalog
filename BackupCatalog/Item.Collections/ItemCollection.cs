using System;
using System.Collections.Generic;

namespace BackupCatalog {
    public abstract class ItemCollection : Item {
        public abstract event EventHandler ContentChange;

        public ItemCollection() { 
        }
        public ItemCollection(Item item) : base(item) { 
        
        }
        public ItemCollection(string name) : base(name) { 
        }

        public abstract string GetNewItemName(string name, Type type);
        public abstract void AddItem(Item item);
        public abstract void RemoveItem(Item item);
        public abstract List<Item> GetItems();
        public abstract List<DirectoryBackup> GetAllDirectories();
        public abstract List<FileBackup> GetAllFiles();
        public abstract bool IsItemTypeAccepted(Type type);
    }
}
