
using System;

namespace Nuntium
{
    public class FilePathArgs : EventArgs
    {

        public string FilePath { get; set; }

        public FilePathArgs(string FilePath) => this.FilePath = FilePath;

    }
}
