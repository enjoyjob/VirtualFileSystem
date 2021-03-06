using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using VirtualFileSystem;

namespace UnitTestProject
{
    [TestClass]
    public class UtilUnitTest
    {
        //Use ClassInitialize to run code before running the first test in the class 
        [ClassInitialize()]
        public static void ClassSetUp(TestContext testContext)
        {
        }

        // 
        //Use TestInitialize to run code before running each test 
        [TestInitialize()]
        public void TestSetup()
        {
        }

        // 
        //Use TestCleanup to run code after each test has run 
        [TestCleanup()]
        public void TestTeardown()
        {
            VirtualStorage.Instance.Init();
        }

        // 
        //Use ClassCleanup to run code after all tests in a class have run 
        [ClassCleanup()]
        public static void ClassTeardown()
        {
        }

        [TestMethod]
        public void TestMethod_GetPublicParentFolderPath()
        {
            // Return nearest parent folder for all virtual nodes
            // For example, suppose your nodes contains file/ folder pathes as following:
            // node 1: $\1\2\3\4\5
            // node 2: $\1\2\3\A\B\C
            // node 3: $\1\2\3
            // node 4: $\1\2\3\4\A
            // node 5: $\1\2\3\A\4
            // this method should
            // return "$\1\2\3" if node 3 is a folder
            // return "$\1\2" if node 3 is a file

            VirtualRootFolder rootFolder = new VirtualRootFolder();
            var folder1 = rootFolder.CreateFolder(@"1\2\3\4\5");
            var folder2 = rootFolder.CreateFolder(@"1\2\3\A\B\C");
            var folder3 = rootFolder.CreateFolder(@"1\2\3");
            var folder4 = rootFolder.CreateFolder(@"1\2\3\4\A");
            var folder5 = rootFolder.CreateFolder(@"1\2\3\A\4");
            var folders = new IVirtualNode[] { folder1, folder2, folder3, folder4, folder5 };

            var result = Utils.GetPublicParentFolderPath(folders);
            Assert.IsTrue(result == @"$\1\2\3");

            var folder6 = rootFolder.CreateFolder(@"1\2\33");
            //var file = folder3.CreatFile("3"); //Fix
            var file = rootFolder.CreateFolder(@"1\2").CreatFile("3");
            var file2 = folder6.CreatFile("33");
            folders = new IVirtualNode[] { folder1, folder2, folder3, folder4, folder5, file, file2 };
            result = Utils.GetPublicParentFolderPath(folders);
            Assert.IsTrue(result == @"$\1\2");
        }
    }
}
