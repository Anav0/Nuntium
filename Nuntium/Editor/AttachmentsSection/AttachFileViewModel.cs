
using System;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Nuntium
{
    public class AttachFileViewModel : BaseViewModel
    {
        #region Public properties

        public string FileName { get; set; }

        public string FilePath { get; set; }

        public long FileSize { get; set; }

        public ImageSource FileIcon { get; set; }

        #endregion

        public AttachFileViewModel()
        {
            DeleteFileCommand = new RelayCommand(DeleteFile);
            FileClickCommand = new RelayCommand(FileClick);
        }

        #region Public commands

        public ICommand DeleteFileCommand { get; set; }

        public ICommand FileClickCommand { get; set; }

        #endregion

        #region Events

        public event EventHandler OnDeleteAction;

        public event EventHandler OnFileClick;

        #endregion

        #region Event raisers

        public void RaiseOnDeleteAction() => OnDeleteAction?.Invoke(this, new EventArgs());

        public void RaiseOnFileClick() => OnFileClick?.Invoke(this, new EventArgs());

        #endregion

        #region Command methods

        private void DeleteFile() => RaiseOnDeleteAction();

        private void FileClick() => RaiseOnFileClick();

        #endregion
    }
}
