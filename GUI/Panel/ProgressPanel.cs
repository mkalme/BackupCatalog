using System;
using System.IO;
using System.Windows.Forms;
using BackupCatalog;

namespace GUI {
    public partial class ProgressPanel : UserControl {
        public UpdateProgress Progress { get; set; }
        public string Message { get; set; }

        public ProgressPanel(){
            InitializeComponent();
        }

        public void DisplayPanel() {
            SetProgressPanelPercentage(Progress.Percentage);

            SetImage();

            if (Progress.CurrentItem != null) {
                NameLabel.Text = Path.GetFileName(Progress.CurrentItem.Path);
            }

            SetMessage();
            HeaderLabel.Text = Message;

            AmountLabel.Text = $"{Progress.Current} / {Progress.Count}";
        }
        private void SetProgressPanelPercentage(float percentage){
            ProgressContainerPanel.Width = (int)(ProgressContainer.Width * percentage);
        }

        private void SetImage() {
            PictureBox.Image = Progress.Type == ItemType.File ? Properties.Resources._64pxDocument : Properties.Resources._64pxDirectory;
        }

        private void SetMessage(){
            string header = "";

            switch (Progress.ProcessType) {
                case ProcessType.Discovering:
                    header = "Discovering";
                    break;
                case ProcessType.Copying:
                    header = "Copying";
                    break;
            }

            if (_headerText != header) {
                _headerText = header;
                Message = header;
            }

            if ((DateTime.Now - _updated).TotalMilliseconds >= 500) {
                _updated = DateTime.Now;
                _dotCount = _dotCount > 2 ? 0 : _dotCount + 1;
                Message = $"{_headerText}{new string('.', _dotCount)}";
            }
        }
        private DateTime _updated = DateTime.MinValue;
        private string _headerText = "";
        private int _dotCount = 0;
    }
}
