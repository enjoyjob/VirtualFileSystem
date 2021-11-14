using System;
using System.Collections.Generic;
using System.Text;

namespace VirtualFileSystem
{
    public class VirtualFolder : IVirtualNode
    {
        public const string ROOT_FOLDER_NAME = "$";
        public const string FOLDER_SEPARATOR = @"\";
        public const string DEFAULT_FOLDER_NAME = "New Folder";

        public VirtualFolder(string fileName)
        {
            Name = string.IsNullOrWhiteSpace(fileName) ? 
                DEFAULT_FOLDER_NAME : fileName.Trim();

            if(Name == ROOT_FOLDER_NAME)
            {
                foreach (var item in VirtualStorage.Instance.Items)
                {
                    //RootFolder exist
                    if (item.Name == ROOT_FOLDER_NAME 
                        && item.Path == ROOT_FOLDER_NAME)
                    {
                        throw new Exception();
                    }
                }

                VirtualStorage.Instance.Allocate(this);
                Path = ROOT_FOLDER_NAME;
            }
            else
            {
                VirtualStorage.Instance.Allocate(this);
            }
        }

        // Create a file with given file name in current folder
        // If file name is not provided then default file name to "New File"
        // If current folder already have file with the same name, then add (n) as suffix to fileName
        // i.e. Create a file with "New File" as filename, but already exists a duplicated file then create file with "New File (1)" instead
        // If "New File (1)" was duplicated then create "New File (2)" instead
        public VirtualFile CreatFile(string fileName = null)
        {
            string base_name = string.IsNullOrWhiteSpace(fileName) ?
                DEFAULT_FOLDER_NAME : fileName.Trim();

            List<string> names = new List<string>();
            var items = GetDirectChild();
            foreach (var item in items)
            {
                names.Add(item.Name);
            }

            fileName = GetUniqName(base_name, names);
            var newFile = new VirtualFile(fileName);

            // add your code here
            newFile.Path = this.Path;

            return newFile;
        }

        // Create a folder with given file name in current folder
        // If folder name is not provided then default folder name to "New Folder"
        // If current folder already have folder with the same name, then add (n) as suffix to folderName
        // i.e. Create a folder with "New Folder" as folderName, but already exists a duplicated folder then create folder with "New Folder (1)" instead
        // If "New Folder (1)" was duplicated then create "New Folder (2)" instead
        public VirtualFolder CreateFolder(string folderName = null)
        {
            string base_name = string.IsNullOrEmpty(folderName) ? 
                DEFAULT_FOLDER_NAME : folderName.Trim();

            List<string> names = new List<string>();
            var items = GetDirectChild();
            foreach (var item in items)
            {
                names.Add(item.Name);
            }

            folderName = GetUniqName(base_name, names);
            var subFolder = new VirtualFolder(folderName);

            // add your code here
            subFolder.Path = $"{this.Path}{FOLDER_SEPARATOR}{folderName}";
            return subFolder;
        }

        // Create sub folder by a full path, will create sub folder if not exists, or reuse sub folder if already created
        // For example, call CreateFolderByPath("A\B\C\D"), current sub folder have "A" already created
        // Then do not create A but just use it, then create B under A, and Create C under B, and finally create D under C
        public VirtualFolder CreateFolderByPath(string fullPath)
        {
            // add your code here
            //"A\B\C\D" --> "A" "B" "C" "D"
            string[] items = fullPath.Split(FOLDER_SEPARATOR);
            if(items.Length > 0)
            {
                var folder = CreateFolderWhenAbsent(items[0]);
                if(folder != null)
                {
                    for (int i = 1; i < items.Length; i++)
                    {
                        folder = folder.CreateFolderWhenAbsent(items[i]);
                    }
                }

                return folder;
            }
            return null;
        }

        // Copy IVirtualNode to current folder, mean folder or file, if source is folder then should include all sub folders and files within source folder to current folder
        public IVirtualNode Copy(IVirtualNode source)
        {
            // add your code here
            return null;
        }

        // Cut IVirtualNode to current folder
        public IVirtualNode Cut(IVirtualNode source)
        {
            // add your code here
            return null;
        }

        public bool Delete()
        {
            // add your code here
            string this_path_child = $"{this.Path}{FOLDER_SEPARATOR}";
            VirtualStorage.Instance.Deallocate(this);

            List<IVirtualNode> child_items = new List<IVirtualNode>();
            foreach (var item in VirtualStorage.Instance.Items)
            {
                if (item.Path.StartsWith(this_path_child))
                {
                    child_items.Add(item);
                }
            }

            foreach(var item in child_items)
            {
                VirtualStorage.Instance.Deallocate(item);
            }
            return true;
        }

        // Return current folder contains a virtual node by name or not
        public IVirtualNode Get(string name)
        {
            // add your code here
            var items = GetDirectChild();
            foreach (var item in items)
            {
                if (name == item.Name)
                {
                    return item;
                }
            }
            return null;
        }
        public VirtualFolder GetFolder(string name)
        {
            // add your code here
            var items = GetDirectChild();
            foreach (var item in items)
            {
                if ((name == item.Name) && (item.GetType() == typeof(VirtualFolder)))
                {
                    return (VirtualFolder) item;
                }
            }
            return null;
        }

        private IEnumerable<IVirtualNode> GetDirectChild()
        {
            List<IVirtualNode> ret = new List<IVirtualNode>();
            foreach (var item in VirtualStorage.Instance.Items)
            {
                //this path: $1
                //direct child path: $1\ab , $1\cd , $1\ef ,...
                //child child  path: $1\ab\x1 , $1\ab\x1\x2 , $1\ab\x1\x2\x3 ,...
                
                if (item.Path.StartsWith(Path) && Path.Length == item.Path.LastIndexOf(FOLDER_SEPARATOR))
                {
                    ret.Add(item);
                }
            }
            return ret;
        }

        private string GetUniqName(string base_name, List<string> names)
        {
            string ret = base_name;
            if(names != null && names.Count > 0)
            {
                int i = 0;
                while (names.Contains(ret))
                {
                    ret = $"{base_name} ({++i})";

                }
            }
            return ret;
        }

        //create folder when not exist
        private VirtualFolder CreateFolderWhenAbsent(string folderName)
        {
            VirtualFolder ret = GetFolder(folderName);
          
            if (ret == null)
            {
                ret = CreateFolder(folderName);
            }
            return ret;
        }
        public string Name { get; set; }
        public string Parent { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string Path { get; private set; }
    }

    public class VirtualRootFolder : VirtualFolder
    {
        public VirtualRootFolder() : base("$")
        {
        }
    }
}
