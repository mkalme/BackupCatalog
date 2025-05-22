using System;
using FastStream;

namespace BackupCatalog {
    public class Item : IEquatable<Item>, ICloneable {
        internal ItemCollection _parent;
        public ItemCollection Parent => _parent;

        private string _name = string.Empty;
        public string Name {
            get => _name;
            set {
                if (value != _name) {
                    _name = value;
                    OnPropertyChange();
                }
            }
        }

        private DateTime _creationDate = DateTime.Now;
        public DateTime CreationDate {
            get => _creationDate;
            set {
                if (value != _creationDate) {
                    _creationDate = value;
                    OnPropertyChange();
                }
            }
        }

        private DateTime _modificationDate = DateTime.Now;
        public DateTime ModificationDate {
            get => _modificationDate;
            set {
                if (value != _modificationDate) {
                    _modificationDate = value;
                    OnPropertyChange();
                }
            }
        }

        protected ItemState _state = ItemState.Existing;
        public ItemState State => _state;

        //Evvents
        public EventHandler PropertyChange;
        internal void OnPropertyChange() {
            if (PropertyChange != null)
                PropertyChange.Invoke(this, EventArgs.Empty);
        }

        public EventHandler Deleted;
        internal void Delete() {
            if (_state == ItemState.Deleted) return;
            _state = ItemState.Deleted;

            if (Deleted != null)
                Deleted.Invoke(this, EventArgs.Empty);
        }

        internal Item(string name) {
            _name = name;
        }
        internal Item(Item item) {
            _name = item._name;

            _creationDate = item._creationDate;
            _modificationDate = item._modificationDate;
        }

        public Item() { 
        }

        //Methods
        public virtual void Update(UpdateProgress progress){

        }
        public virtual void Update(Item item) {
            _name = item._name;
            _state = item._state;
        }

        //Serialize
        internal virtual void Serialize(FastBinaryWriter writer) {
            writer.Write(_name);

            writer.Write(_creationDate);
            writer.Write(_modificationDate);
        }
        internal virtual void SerializeID(FastBinaryWriter writer) {
            writer.Write(GetType().GetID());
        }

        //Deserialize
        internal static Item Deserialize(FastBinaryReader reader) {
            Item item = new Item();

            item._name = reader.ReadString();

            item._creationDate = reader.ReadDateTime();
            item._modificationDate = reader.ReadDateTime();

            return item;
        }

        //Default methods
        public virtual bool Equals(Item item) {
            if (item == null) return false;
            if (GetType() != item.GetType()) return false;

            return _name == item._name;
        }
        public virtual object Clone() {
            return new Item() {
                _name = _name,

                _creationDate = _creationDate,
                _modificationDate = _modificationDate,

                _state = _state
            };
        }
    }

    public enum ItemState {
        Existing,
        Deleted
    }
}
