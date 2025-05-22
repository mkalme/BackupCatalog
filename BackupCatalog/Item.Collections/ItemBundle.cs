using System;

namespace BackupCatalog {
    public abstract class ItemBundle : ItemCollection {
        public ItemBundle() { 
        }
        public ItemBundle(Item item) : base(item) { 
        }
        public ItemBundle(string name) : base(name) { 
        }

        public abstract DateTime GetLastUpdateDate();
    }
}
