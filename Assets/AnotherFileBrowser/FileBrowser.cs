//This script was downloaded, with permission, from https://github.com/SrejonKhan/AnotherFileBrowser
//A plugin was required because to access the file explorer without one would be too complex of a task for the scope of this project
//This is the only script in the project not written by me
//An understanding of the below code was required when referencing the file explorer in the TrackImporter script

#if UNITY_STANDALONE_WIN
using Ookii.Dialogs;
using System;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.IO;

namespace AnotherFileBrowser.Windows
{
    public class FileBrowser
    {
        [DllImport("user32.dll")]
        private static extern IntPtr GetActiveWindow();

        public FileBrowser() { }

        /// FileDialog for picking a single file
        public void OpenFileBrowser(BrowserProperties browserProperties, Action<string> filepath)
        {
            var ofd = new VistaOpenFileDialog();
            ofd.Multiselect = false;
            ofd.Title = browserProperties.title == null ? "Import custom track" : browserProperties.title;
            ofd.FileName = ValidateInitialDir(browserProperties.initialDir); // initial dir
            ofd.Filter = browserProperties.filter == null ? "All files (*.*)|*.*" : browserProperties.filter;
            ofd.FilterIndex = browserProperties.filterIndex + 1;
            ofd.RestoreDirectory = browserProperties.restoreDirectory;

            if (ofd.ShowDialog(new WindowWrapper(GetActiveWindow())) == DialogResult.OK)
            {
                filepath(ofd.FileName);
            }
        }

        private string ValidateInitialDir(string dir)
        {
            if (string.IsNullOrEmpty(dir) || string.IsNullOrWhiteSpace(dir))
                return "";

            if (!Directory.Exists(dir))
                return "";

            // add trailing slash to open directory perfectly
            if (dir[dir.Length - 1] != '/' && dir[dir.Length - 1] != '\\')
                return dir + "/";

            return dir;

        }
    }

    public class BrowserProperties
    {
        public string title; //Title of the Dialog
        public string initialDir; //Where dialog will be opened initially
        public string filter; //aka File Extension for filtering files
        public int filterIndex; //Index of filter, if there is multiple filter. Default is 0. 
        public bool restoreDirectory = true; //Restore to last return directory


        public BrowserProperties() { }
        public BrowserProperties(string title) { this.title = title; }
    }

    public class WindowWrapper : IWin32Window
    {
        public WindowWrapper(IntPtr handle)
        {
            _hwnd = handle;
        }

        public IntPtr Handle
        {
            get { return _hwnd; }
        }

        private IntPtr _hwnd;
    }
}
#endif