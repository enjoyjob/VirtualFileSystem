using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using VirtualFileSystem;

namespace UnitTestProject
{
    [TestClass]
    public class VirtualFileUnitTest
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
