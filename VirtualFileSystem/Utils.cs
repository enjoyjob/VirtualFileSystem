using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

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

            int i = 0;
            List<string> unionList = new List<string>();
            List<string> curNodeParents = new List<string>();

            foreach ( var node in nodes)
            {
                if (i == 0)
                {
                    unionList = new List<string>(node.Path.Split(VirtualFolder.FOLDER_SEPARATOR));
                }
                else
                {
                    curNodeParents = new List<string>(node.Path.Split(VirtualFolder.FOLDER_SEPARATOR));
                    unionList = unionList.Intersect(curNodeParents).ToList();
                }
                i++;
            }
            string ret = string.Join(VirtualFolder.FOLDER_SEPARATOR, unionList);
            return ret;
        }
    }
}
