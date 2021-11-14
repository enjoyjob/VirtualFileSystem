using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using VirtualFileSystem;

namespace UnitTestProject
{
    [TestClass]
    public class VirtualFileUnitTest
    {
        [TestMethod]
        public void TestMethod_FileName_Should_Not_Empty()
        {
            VirtualFile vFile = new VirtualFile("");

            Assert.IsFalse(String.IsNullOrWhiteSpace(vFile.Name));
        }

        [TestMethod]
        public void TestMethod_Create_File_Should_Allocate_Storage()
        {
            VirtualFile vFile = new VirtualFile("New File");

            Assert.IsTrue(VirtualStorage.Instance.Contains(vFile));
        }
    }
}
