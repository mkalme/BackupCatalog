using System;
using System.Threading;

namespace BackupCatalog {
    public class UpdateProgress {
        public int Current { get; set; } = 0;
        public int Count { get; set; } = 0;
        public float Percentage { get; internal set; }

        public ItemCopy CurrentItem { get; set; }
        public UpdateProgress CurrentDirectoryProgress { get; set; }

        private ProgressState _state = ProgressState.NotBegun;
        public ProgressState State {
            get => _state;
            set {
                if (_state != value) {
                    if (_state == value) return;
                    _state = value;

                    if (_state == ProgressState.Finished) {
                        OnFinished();
                    }
                }
            }
        }

        public ProcessType ProcessType { get; internal set; }
        public ItemType Type { get; internal set; } = ItemType.File;

        //Events

        public EventHandler Finished;
        public void OnFinished(){
            if (Finished != null)
                Finished.Invoke(this, EventArgs.Empty);
        }

        //Methods
        public void Reset() {
            Current = 0;
            Count = 0;
            Percentage = 0;

            Type = ItemType.File;
            CurrentItem = null;
            CurrentDirectoryProgress = null;
        }
    }

    public enum ProgressState {
        NotBegun,
        Running,
        Finished
    }
    public enum ProcessType {
        Discovering,
        Copying
    }

    public class ItemCopy {
        public string Path { get; set; }
        public ItemType Type { get; set; }

        public ItemCopy(string path, ItemType type) {
            Path = path;
            Type = type;
        }
    }
    public enum ItemType {
        File,
        Directory
    }
}
