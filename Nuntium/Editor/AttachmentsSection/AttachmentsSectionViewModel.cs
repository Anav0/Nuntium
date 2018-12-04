
using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.Drawing;
using System.IO;
using System.Windows;

namespace Nuntium
{
    public class AttachmentsSectionViewModel : BaseViewModel
    {
        public ObservableCollection<AttachFileViewModel> Items { get; set; }
        
        public AttachmentsSectionViewModel()
        {
            Items = new ObservableCollection<AttachFileViewModel>();
        }

        public void InsertFileFromDialog()
        {
            var dialog = new OpenFileDialog();
            dialog.Title = "Select file to attach";

            if (dialog.ShowDialog() == true)
            {
                foreach (var file in Items)
                {
                    if (file.FilePath == dialog.FileName)
                    {
                        MessageBox.Show("File was already added", "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                        return;
                    }
                }

                var fileVM = new AttachFileViewModel
                {
                    FilePath = dialog.FileName,
                    FileName = Path.GetFileName(dialog.FileName),
                    FileSize = new FileInfo(dialog.FileName).Length,
                    FileIcon = Icon.ExtractAssociatedIcon(dialog.FileName).ToImageSource(),
                };

                fileVM.OnDeleteAction += ((s, arg) =>
                {
                    if (!(s is AttachFileViewModel sdr))
                        return;

                    Items.Remove(sdr);
                });

                fileVM.OnFileClick += ((s, arg) =>
                {
                    if (!(s is AttachFileViewModel sdr))
                        return;

                    System.Diagnostics.Process.Start(sdr.FilePath);
                });

                Items.Add(fileVM);
            }
        }
    }
}
