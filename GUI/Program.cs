using System;
using System.Windows.Forms;

namespace GUI {
    static class Program {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            try {
                ApplicationInfo.Current = ApplicationInfo.FromLocalFile();
            } catch {
                DialogFactory.ShowShortErrorDialog("There was an error reading the backup catalog file.");
                return;
            }

            try {
                Application.Run(new Window(new BackupProfile(ApplicationInfo.Current.Group)));
            } catch (Exception e) {
                DialogFactory.ShowErrorDialog(e);
                ApplicationInfo.Current.Save();
            }
        }
    }
}
