using System;
using System.Collections.Generic;
using FastStream;

namespace BackupCatalog {
    public class UpdateHistory {
        private List<DateTime> _updateDates = new List<DateTime>();
        public IList<DateTime> UpdateDates => _updateDates.AsReadOnly();

        public DateTime Last {
            get {
                if (_updateDates.Count < 1) return new DateTime(0);

                return _updateDates[_updateDates.Count - 1];
            }
        }

        //Event
        public event EventHandler UpdateAdded;
        private void OnUpdateAdded() {
            if (UpdateAdded != null)
                UpdateAdded.Invoke(this, EventArgs.Empty);
        }

        //Methods
        public void Update() {
            _updateDates.Add(DateTime.Now);

            OnUpdateAdded();
        }

        //Serialize
        public void Serialize(FastBinaryWriter writer) {
            writer.Write(_updateDates.Count);

            foreach (var date in _updateDates) {
                writer.Write(date);
            }
        }

        //Deserialize
        public static UpdateHistory Deserialize(FastBinaryReader reader) {
            int count = reader.ReadInt32();

            UpdateHistory history = new UpdateHistory();
            while (count > 0) {
                history._updateDates.Add(reader.ReadDateTime());

                count--;
            }

            return history;
        }
    }
}
