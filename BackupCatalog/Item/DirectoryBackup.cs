using System;
using System.IO;
using FastStream;

namespace BackupCatalog {
    public class DirectoryBackup : Item {
        private string _sourcePath = string.Empty;
        public string SourcePath {
            get => _sourcePath;
            set {
                if (value != _sourcePath) {
                    _sourcePath = value;
                    OnPropertyChange();
                }
            }
        }

        private string _destinationFolder = string.Empty;
        public string DestinationFolder {
            get => _destinationFolder;
            set {
                if (value != _destinationFolder) {
                    _destinationFolder = value;
                    OnPropertyChange();
                }
            }
        }

        public UpdateHistory UpdateHistory { get; set; } = new UpdateHistory();
        public DateTime UpdateDate => UpdateHistory.Last;

        public DirectoryBackup() { 
        }
        internal DirectoryBackup(Item item) : base(item){
        }
        public DirectoryBackup(string name) : base(name){
        }

        //Methods
        public override void Update(UpdateProgress progress){
            progress.State = ProgressState.Running;
            Updater.UpdateDirectory(this, progress);
            progress.State = ProgressState.Finished;
        }

        public void Update(DirectoryBackup backup) {
            base.Update(backup);

            _sourcePath = backup._sourcePath;
            _destinationFolder = backup._destinationFolder;

            OnPropertyChange();
        }
        public static implicit operator DirectoryBackup(string path) {
            DirectoryBackup backup = new DirectoryBackup(Path.GetFileName(path));
            backup.SourcePath = path;

            return backup;
        }

        //Serialize
        internal override void Serialize(FastBinaryWriter writer){
            base.Serialize(writer);

            writer.Write(_sourcePath);
            writer.Write(_destinationFolder);
            UpdateHistory.Serialize(writer);
        }

        //Deserialize
        internal new static DirectoryBackup Deserialize(FastBinaryReader reader){
            DirectoryBackup backup = new DirectoryBackup(Item.Deserialize(reader));

            backup._sourcePath = reader.ReadString();
            backup._destinationFolder = reader.ReadString();
            backup.UpdateHistory = UpdateHistory.Deserialize(reader);

            return backup;
        }

        //Default Methods
        public override bool Equals(Item item){
            if (!base.Equals(item)) return false;

            DirectoryBackup backup = item as DirectoryBackup;

            return _sourcePath == backup._sourcePath && _destinationFolder == backup._destinationFolder;
        }
        public override object Clone(){
            DirectoryBackup backup = new DirectoryBackup((Item)base.Clone());

            backup._sourcePath = _sourcePath;
            backup._destinationFolder = _destinationFolder;

            return backup;
        }
    }
}
