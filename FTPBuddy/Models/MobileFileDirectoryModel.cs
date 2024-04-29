using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTPBuddy.Models
{
    public class MobileFileDirectoryModel
    {
        private FileSystemInfo _directoryFileSystem;

        public FileSystemInfo DirectoryFileSystem
        {
            get { return _directoryFileSystem; }
            set { _directoryFileSystem = value; }
        }

        public bool IsDirectory
        {
            get { return _directoryFileSystem.Attributes.HasFlag(FileAttributes.Directory); }
        }

        public string GetIcon
        {
            get
            {
                if (IsDirectory)
                    return "folder.png";

                return "file.png";
            }
        }

        public string Name
        {
            get { return _directoryFileSystem.Name; }
        }

        public string FullName
        {
            get { return _directoryFileSystem.FullName; }
        }
    }
}
