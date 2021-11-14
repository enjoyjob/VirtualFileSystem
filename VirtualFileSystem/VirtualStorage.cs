using System;
using System.Collections.Generic;
using System.Text;

namespace VirtualFileSystem
{
    public sealed class VirtualStorage
    {
        private HashSet<IVirtualNode> items = new HashSet<IVirtualNode>();
        private VirtualStorage()
        {
        }

        private static VirtualStorage _instance = null;
        public static VirtualStorage Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new VirtualStorage();
                return _instance;
            }
        }

        // To mimic allocating disk for file/folder creation
        public void Allocate(IVirtualNode node)
        {
            System.Diagnostics.Debug.Assert(!Contains(node));
            if (items.Contains(node))
            {
                return;
            }
            items.Add(node);
        }

        // To mimic deallocating disk for file/folder deleting
        public void Deallocate(IVirtualNode node)
        {
            System.Diagnostics.Debug.Assert(Contains(node));
            if (!items.Contains(node))
            {
                return;
            }
            items.Remove(node);
        }

        public bool Contains(IVirtualNode node)
        {
            return items.Contains(node);
        }

        public HashSet<IVirtualNode> Items
        {
            get { return items; }
        }
    }
}
