using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using VirtualFileSystem;

namespace UnitTestProject
{
    [TestClass]
    public class VirtualFolderUnitTest
    {
        [TestMethod]
        public void TestMethod_FolderName_Should_Not_Empty()
        {
            VirtualFolder vFolder = new VirtualFolder("");

            Assert.IsFalse(String.IsNullOrWhiteSpace(vFolder.Name));
        }

        [TestMethod]
        public void TestMethod_Create_Folder_Should_Allocate_Storage()
        {
            VirtualFolder vFolder = new VirtualFolder("New File");

            Assert.IsTrue(VirtualStorage.Instance.Contains(vFolder));
        }

        [TestMethod]
        public void TestMethod_Create_Root_Folder_Multiple_Times_Throw_Exception()
        {
            VirtualRootFolder vFolder1 = new VirtualRootFolder();
            Assert.ThrowsException<System.Exception>(() => { VirtualRootFolder vFolder2 = new VirtualRootFolder(); });
        }

        [TestMethod]
        public void TestMethod_Root_Folder_Path()
        {
            VirtualRootFolder rootFolder = new VirtualRootFolder();
            Assert.IsTrue(rootFolder.Path == "$");
        }

        [TestMethod]
        public void TestMethod_Sub_Folder_Path()
        {
            VirtualRootFolder rootFolder = new VirtualRootFolder();
            var folder1 = rootFolder.CreateFolder("1");
            Assert.IsTrue(folder1.Path == @"$\1");

            var folder1A = folder1.CreateFolder("1A");
            Assert.IsTrue(folder1A.Path == @"$\1\1A");
        }

        [TestMethod]
        public void TestMethod_Create_Folder_By_Path()
        {
            VirtualRootFolder rootFolder = new VirtualRootFolder();
            var subFolderC = rootFolder.CreateFolderByPath(@"A\B\C");
            Assert.IsNotNull(subFolderC);

            // folder should be created
            var folderA = rootFolder.GetFolder("A");
            Assert.IsNotNull(folderA);

            var folderB = folderA.GetFolder("B");
            Assert.IsNotNull(folderB);

            var folderC = folderB.GetFolder("C");
            Assert.IsNotNull(folderC);
            Assert.ReferenceEquals(subFolderC, folderC);
        }

        [TestMethod]
        public void TestMethod_Cut_File_Should_Change_File_Path()
        {
            VirtualRootFolder rootFolder = new VirtualRootFolder();
            var folder1 = rootFolder.CreateFolder("1");

            // $\1\A
            var folder1A = folder1.CreateFolder("A");

            // $\1\B
            var folder1B = folder1.CreateFolder("B");

            // $\1\A\file1A_1.txt
            var file1A_1 = folder1A.CreatFile("file1A_1.txt");

            var nodeCount = VirtualStorage.Instance.Items.Count;

            // $\1\B\file1A_1.txt
            folder1B.Cut(file1A_1);

            Assert.IsTrue(VirtualStorage.Instance.Items.Count == nodeCount);
            Assert.IsTrue(folder1A.Get("file1A_1") == null);
            Assert.IsTrue(folder1B.Get("file1A_1") != null);

            // moved file path should be changed
            Assert.IsTrue(file1A_1.Path == @"$\1\B\file1A_1.txt");
        }

        [TestMethod]
        public void TestMethod_Delete_Folder_Should_Deallocate_Storage()
        {
            VirtualRootFolder rootFolder = new VirtualRootFolder();
            var folder1 = rootFolder.CreateFolder("folder1");
            folder1.Delete();

            // file should be cleaned up
            Assert.IsFalse(VirtualStorage.Instance.Contains(folder1));

            var nodeCount = VirtualStorage.Instance.Items.Count;

            var folder2 = rootFolder.CreateFolder("folder2");
            folder2.CreateFolder("A").CreateFolder("B").CreateFolder("C");
            folder2.CreateFolder("a").CreateFolder("b").CreateFolder("c");
            folder2.CreateFolder("1").CreateFolder("2").CreateFolder("3");
            folder2.Delete();

            // file should be cleaned up
            Assert.IsFalse(VirtualStorage.Instance.Contains(folder2));
            Assert.IsFalse(rootFolder.Get("folder2") == null);

            Assert.IsTrue(VirtualStorage.Instance.Items.Count == nodeCount);
        }

        [TestMethod]
        public void TestMethod_Cut_File_Should_Not_ReAllocating()
        {
            VirtualRootFolder rootFolder = new VirtualRootFolder();
            var folder1 = rootFolder.CreateFolder("1");
            var folder1A = folder1.CreateFolder("A");
            var folder1B = folder1.CreateFolder("B");
            var file1A_1 = folder1A.CreatFile("file1A_1.txt");

            var nodeCount = VirtualStorage.Instance.Items.Count;

            folder1B.Cut(file1A_1);

            Assert.IsTrue(VirtualStorage.Instance.Items.Count == nodeCount);
            Assert.IsTrue(folder1A.Get("file1A_1") == null);
            Assert.IsTrue(folder1B.Get("file1A_1") != null);
        }

        [TestMethod]
        public void TestMethod_Cut_Folder_From_Parent_Is_Not_Allowed()
        {
            VirtualRootFolder rootFolder = new VirtualRootFolder();
            var folder1 = rootFolder.CreateFolder("1");
            Random r = new Random();
            var subFolder = folder1.CreateFolder(r.Next(1, 10).ToString()).CreateFolder(r.Next(1, 10).ToString()).CreateFolder(r.Next(1, 10).ToString());
            // Cut parent is not allowed
            Assert.IsFalse(subFolder.Cut(folder1) == null);

            var subFolder2 = subFolder.CreateFolder(r.Next(1, 10).ToString()).CreateFolder(r.Next(1, 10).ToString()).CreateFolder(r.Next(1, 10).ToString());
            // Cut parent is not allowed
            Assert.IsFalse(subFolder2.Cut(subFolder) == null);

            // Cut myself is not allowed as well
            Assert.IsFalse(subFolder2.Cut(subFolder2) == null);
        }

        [TestMethod]
        public void TestMethod_Copy_Duplicated_File_Should_Rename()
        {
            VirtualRootFolder rootFolder = new VirtualRootFolder();
            var folder1 = rootFolder.CreateFolder("1");
            Random r = new Random();
            var file = folder1.CreatFile(r.Next(1, 10).ToString());

            var file2 = folder1.Copy(file);
            // file2 should be in a new name
            Assert.IsTrue(file2.Name != file.Name);
        }

        [TestMethod]
        public void TestMethod_Copy_Should_Copy_Whole_Structure()
        {
            VirtualRootFolder rootFolder = new VirtualRootFolder();

            var nodeCountStart = VirtualStorage.Instance.Items.Count;
            var folder1 = rootFolder.CreateFolder("1");

            var subFolder = folder1.CreateFolder(@"A1\B2");
            var subFolder2 = folder1.CreateFolder(@"A1\B2\C1");
            var subFolder3 = folder1.CreateFolder(@"A1\B2\C2\D1");

            var destFolder = folder1.CreateFolder(@"Dest");
            var nodeCount = VirtualStorage.Instance.Items.Count;
            // total 6 folder created:
            // $\1
            // $\1\A1
            // $\1\A1\B2
            // $\1\A1\B2\C1
            // $\1\A1\B2\C2
            // $\1\A1\B2\C2\D1
            Assert.IsTrue((nodeCount - nodeCountStart) == 6);

            var subFolderCopyed = destFolder.Copy(subFolder);

            Assert.IsTrue(folder1.GetFolder(@"Dest\A1\B2") != null);
            Assert.IsTrue(folder1.GetFolder(@"Dest\A1\B2\C2\D1") != null);

            // source kept
            Assert.IsTrue(folder1.GetFolder(@"A1\B2\C2\D1") != null);


            // total 5 folder created:
            // $\1\Dest\A1
            // $\1\Dest\B2
            // $\1\Dest\B2\C1
            // $\1\Dest\B2\C2
            // $\1\Dest\B2\C2\D1
            var nodeCount2 = VirtualStorage.Instance.Items.Count;
            Assert.IsTrue((nodeCount2 - nodeCount) == 5);
        }
    }
}
