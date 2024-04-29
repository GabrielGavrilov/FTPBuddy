using FluentFTP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTPBuddy.Models
{
    public class FtpFileDirectoryModel
    {
        private FtpListItem directoryListItem;

        public FtpListItem DirectoryListItem
        {
            get { return directoryListItem; }
            set { directoryListItem = value; }
        }

        public string GetIcon
        {
            get
            {
                if (IsFile)
                    return "file.png";

                return "folder.png";
            }
        }

        public bool IsFile
        {
            get { return directoryListItem.Type == FtpObjectType.File; }
        }

        public string Name
        {
            get { return directoryListItem.Name; }
        }

        public string FullName
        {
            get { return directoryListItem.FullName; }
        }

    }
}
