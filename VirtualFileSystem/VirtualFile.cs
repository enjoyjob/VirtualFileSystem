using System;
using System.Collections.Generic;
using System.Text;

namespace VirtualFileSystem
{
    public class VirtualFile : IVirtualNode
    {
        public const string DEFAULT_FILE_NAME = "New File";

        public string Path { get; set; }

        public VirtualFile(string fileName)
        {
            Name =  string.IsNullOrWhiteSpace(fileName) ? 
                DEFAULT_FILE_NAME : fileName.Trim();
            VirtualStorage.Instance.Allocate(this);
        }

        public string Name
        {
            get;
            set;
        }
        public string Parent
        {
            get => throw new NotImplementedException();
            set => throw new NotImplementedException();
        }

        bool IVirtualNode.Delete()
        {
            throw new NotImplementedException();
        }
    }
}
