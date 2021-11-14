using System;
using System.Collections.Generic;
using System.Text;

namespace VirtualFileSystem
{
    public interface IVirtualNode
    {
        public string Name { get; set; }
        public string Parent { get; set; }
        public string Path { get; }

        public bool Delete();
    }
}
