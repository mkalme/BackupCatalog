using System;
using System.Drawing;
using System.Media;
using BackupCatalog;

namespace GUI {
    public class DialogFactory {
        public static ProgressDialog ShowProgressDialog(UpdateProgress progress, Item item) {
            ProgressDialog dialog = new ProgressDialog(progress);

            dialog.Icon = item.GetIcon();
            
            dialog.ShowDialog();

            return dialog;
        }

        public static TextInputDialog ShowRenameItemDialog(Item item, ItemNameValidator validator) {
            TextInputDialog dialog = new TextInputDialog(item.Name, validator.ValidateName);
            dialog.Text = "Rename";
            dialog.Message = "Rename this item";
            dialog.Icon = item.GetIcon();
            dialog.Image = item.GetImage();

            dialog.ShowDialog();

            return dialog;
        }
        public static ConfirmationDialog ShowDeleteDialog(){
            ConfirmationDialog dialog = new ConfirmationDialog(ConfirmationDialogType.YesCancel);

            dialog.Text = "Delete";
            dialog.Message = "Are you sure you want to delete this item?";
            dialog.Image = Properties.Resources._64pxTrash;
            dialog.Sound = SystemSounds.Hand;
            dialog.Icon = Properties.Resources._64pxTrashIcon;
            dialog.YesButtonText = "Delete";

            dialog.ShowDialog();

            return dialog;
        }

        public static DirectoryDialog ShowEditDirectory(DirectoryBackup directory, ItemNameValidator validator) {
            DirectoryDialog dialog = new DirectoryDialog(directory, validator.ValidateName);
            dialog.ShowDialog();

            return dialog;
        }
        public static DirectoryBundleDialog ShowEditDirectoryBundle(DirectoryBundle bundle, ItemNameValidator validator){
            DirectoryBundleDialog dialog = new DirectoryBundleDialog(bundle, validator.ValidateName);
            dialog.ShowDialog();

            return dialog;
        }
        public static FileDialog ShowEditFile(FileBackup file, ItemNameValidator validator){
            FileDialog dialog = new FileDialog(file, validator.ValidateName);
            dialog.ShowDialog();

            return dialog;
        }
        public static FileBundleDialog ShowEditFileBundle(FileBundle bundle, ItemNameValidator validator){
            FileBundleDialog dialog = new FileBundleDialog(bundle, validator.ValidateName);
            dialog.ShowDialog();

            return dialog;
        }

        public static void ShowUpdateHistoryDialog(UpdateHistory history) {
            new UpdateHistoryDialog(history).ShowDialog();
        }

        public static void ShowShortErrorDialog(string input){
            MessageDialog dialog = new MessageDialog(input);

            dialog.Text = "Error";
            dialog.Sound = SystemSounds.Hand;
            dialog.Image = SystemIcons.Error.ToBitmap();
            dialog.Icon = SystemIcons.Error;
            dialog.TextBorderStyle = System.Windows.Forms.BorderStyle.None;
            dialog.Sizeable = false;

            dialog.ShowDialog();
        }
        public static void ShowErrorDialog(Exception e){
            MessageDialog dialog = new MessageDialog(e.ToString());

            dialog.Text = $"Error | {e.Message}";
            dialog.Sound = SystemSounds.Hand;
            dialog.Image = SystemIcons.Error.ToBitmap();
            dialog.Icon = SystemIcons.Error;

            dialog.ShowDialog();
        }
    }
}
