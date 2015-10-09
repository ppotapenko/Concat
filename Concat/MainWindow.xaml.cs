#region using

using System;
using System.Linq;
using System.Windows;
using Microsoft.WindowsAPICodePack.Dialogs;
using SimpleWPFProgressWindow;

#endregion

namespace Concat
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ButtonSelectFolder_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new CommonOpenFileDialog();
            dialog.IsFolderPicker = true;
            var result = dialog.ShowDialog();
            if (result == CommonFileDialogResult.Ok)
            {
                TextBoxDirPath.Text = dialog.FileName;
            }
        }

        private void ButtonAddToIgnore_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new CommonOpenFileDialog();
            var mainDir = TextBoxDirPath.Text;
            dialog.IsFolderPicker = true;
            dialog.InitialDirectory = mainDir;
            var result = dialog.ShowDialog();
            if (result == CommonFileDialogResult.Ok)
            {
                TextBoxIgnoreFolders.Text += dialog.FileName.Remove(0, mainDir.Length) + ";\n";
            }
        }

        private void ButtonGetResult_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new CommonSaveFileDialog();
            dialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
            dialog.DefaultFileName = "Result";
            dialog.AlwaysAppendDefaultExtension = true;
            dialog.DefaultExtension = "txt";
            dialog.AlwaysAppendDefaultExtension = true;
            dialog.Filters.Add(CommonFileDialogStandardFilters.TextFiles);
            var result = dialog.ShowDialog();
            if (result == CommonFileDialogResult.Ok)
            {
                var ignoreFolders = TextBoxIgnoreFolders.Text.Trim().Trim('\n').TrimEnd(';').Split(';').ToList();
                var progressWindow =
                    new ProgressWindow(new FileCounter(TextBoxDirPath.Text, TextBoxFilterExt.Text, ignoreFolders));
                //runs the progress operation upon window load
                progressWindow.ShowDialog();
            }
        }
    }
}
