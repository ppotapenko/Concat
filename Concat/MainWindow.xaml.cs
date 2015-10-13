#region using

using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Microsoft.WindowsAPICodePack.Dialogs;

#endregion

namespace Concat
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
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
            dialog.Multiselect = true;
            var result = dialog.ShowDialog();
            if (result == CommonFileDialogResult.Ok)
            {
                foreach (var dirName in dialog.FileNames)
                {
                    TextBoxIgnoreFolders.AppendText(Environment.NewLine);
                    TextBoxIgnoreFolders.AppendText(dirName.Remove(0, mainDir.Length) + ";");
                }
                TextBoxIgnoreFolders.Text = TextBoxIgnoreFolders.Text.TrimStart(Environment.NewLine.ToCharArray());
            }
        }

        private void ButtonGetResult_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new CommonSaveFileDialog
            {
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory),
                DefaultFileName = new DirectoryInfo(TextBoxDirPath.Text).Name,
                AlwaysAppendDefaultExtension = true,
                DefaultExtension = "txt"
            };
            dialog.AlwaysAppendDefaultExtension = true;
            dialog.Filters.Add(CommonFileDialogStandardFilters.TextFiles);
            var result = dialog.ShowDialog();
            if (result == CommonFileDialogResult.Ok)
            {
                var ignoreFolders =
                    TextBoxIgnoreFolders.Text
                        .Trim()
                        .Replace("\r", String.Empty)
                        .Replace("\n", String.Empty)
                        .Replace(" ", String.Empty)
                        .TrimEnd(';')
                        .Split(';')
                        .ToList();

                var globalIgnoreFolders =
                    TextBoxGlobalIgnorFolders.Text
                        .Trim()
                        .Replace("\r", String.Empty)
                        .Replace("\n", String.Empty)
                        .Replace(" ", String.Empty)
                        .TrimEnd(',')
                        .Split(',')
                        .ToList();

                var fileCounter = new FileCounter(TextBoxDirPath.Text, TextBoxFilterExt.Text, ignoreFolders, globalIgnoreFolders);
                var progressWindow = new ProgressWindow(fileCounter, dialog.FileName, TextBoxFileTitle.Text)
                {
                    ButtonStart = {Visibility = Visibility.Hidden}
                };

                //runs the progress operation upon window load
                progressWindow.ShowDialog();
            }
        }
    }
}
