using System;
using System.Collections.Generic;
using System.Text;

namespace VirtualFileSystem
{
    public class Utils
    {
        public static string GetPublicParentFolderPath(IEnumerable<IVirtualNode> nodes)
        {
            // Return nearest parent folder for all virtual nodes
            // For example, suppose your nodes contains file/ folder pathes as following:
            // node 1: $\1\2\3\4\5
            // node 2: $\1\2\3\A\B\C
            // node 3: $\1\2\3
            // node 4: $\1\2\3\4\A
            // node 5: $\1\2\3\A\A
            // this method should
            // return "$\1\2\3" if node 3 is a folder
            // return "$\1\2" if node 3 is a file

            return "";
        }
    }
}
