using System;
using System.Collections.Generic;
using BackupCatalog;

namespace GUI {
    public class SimpleProfile {
        public List<Item> Items { get; private set; }
        public Item SelectedItem { get; set; }

        //Event
        public event EventHandler Update;
        private void OnUpdate() {
            if (Update != null)
                Update.Invoke(this, EventArgs.Empty);
        }

        public SimpleProfile() {
            Items = new List<Item>();
        }
        public SimpleProfile(List<Item> items) {
            Items = items;
        }

        //Methods
        public void Add(Item item) {
            Items.Add(item);

            OnUpdate();
        }
        public void RemoveItem(Item item) {
            if (item == null) return;

            for (int i = 0; i < Items.Count; i++) {
                if (!Object.ReferenceEquals(Items[i], item)) continue;

                Items.RemoveAt(i);
                OnUpdate();
                return;
            }
        }
    }
}
