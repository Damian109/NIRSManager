using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NIRSCore.FileOperations
{
    [Serializable]
    public sealed class FileUsersItem
    {
        public string Login { get; set; }
        public string Md5 { get; set; }

        public FileUsersItem() { }
    }
}
