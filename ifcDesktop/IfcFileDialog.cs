using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

namespace ifcDesktop
{
    class IfcFileDialog
    {
        static public string OpenFile(string Filter,string Title)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = Filter;
            dialog.Title = Title;

            var result = dialog.ShowDialog();

            if (result == DialogResult.Cancel) return null;

            Debug.WriteLine(dialog.FileName);
            return dialog.FileName;
        }

        static public string OpenFolder()
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();

            var result = dialog.ShowDialog();

            if (result == DialogResult.Cancel) return null;

            Debug.WriteLine(dialog.SelectedPath);
            return dialog.SelectedPath;
        }

        static public void throwEx(Exception ex)
        {
            MessageBox.Show(ex.Message);
        }
        
        static public void showDia(string text)
        {
            MessageBox.Show(text);
        }
        static public string SaveFile(string ext)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.DefaultExt = ext;

            var result = dialog.ShowDialog();
            
            if(result == DialogResult.OK) return dialog.FileName;
            return null;
        }
    }
}
