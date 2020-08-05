﻿using BeatSaberModdingTools.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;

namespace BSMTTasks_UnitTests
{
    [TestClass]
    public class GetCommitInfo_Tests
    {
        public static readonly string DataFolder = Path.Combine("Data");
        public static readonly string OutputFolder = Path.Combine("Output", "GetCommitInfo");

#if !NCRUNCH
        [TestMethod]
        public void GetGitStatus_Test()
        {
            string directory = Environment.CurrentDirectory;
            GetCommitInfo task = new MockGetCommitHash(directory)
            {
                ProjectDir = directory
            };
            GitInfo status = task.GetGitStatus(directory);
            Assert.IsFalse(string.IsNullOrEmpty(status.Branch));
            Assert.IsFalse(string.IsNullOrEmpty(status.Modified));
            Assert.IsTrue(status.Modified == "Unmodified" || status.Modified == "Modified");
        }
        [TestMethod]
        public void TryGetCommitHash_Test()
        {
            string directory = Environment.CurrentDirectory;
            GetCommitInfo task = new MockGetCommitHash(directory)
            {
                ProjectDir = directory
            };
            bool success = task.TryGetGitCommit(directory, out string commitHash);
            Assert.IsTrue(success);
            Assert.IsTrue(commitHash.Length > 0);
        }
#endif
        [TestMethod]
        public void TryGetCommitManual_Test()
        {
            string directory = Path.Combine(DataFolder, "GitTests", "Test.git");
            string expectedBranch = "master";
            string expectedHash = "4197466ed7682542b4669e98fd962a3925ccaadf";
            string expectedUrl = @"https://github.com/Zingabopp/BeatSaberModdingTools.Tasks";
            Assert.IsTrue(GetCommitInfo.TryGetCommitManual(directory, out GitInfo gitInfo));
            Assert.AreEqual(expectedBranch, gitInfo.Branch);
            Assert.AreEqual(expectedHash, gitInfo.CommitHash);
            Assert.AreEqual(expectedUrl, gitInfo.OriginUrl);
        }
        #region Execute Tests
        [TestMethod]
        public void NoGit()
        {
            string directory = Path.Combine(DataFolder, "GitTests");
            string expectedBranch = "master";
            int hashLength = 7;
            string expectedHash = "4197466ed7682542b4669e98fd962a3925ccaadf".Substring(0, hashLength);
            string expectedUrl = @"https://github.com/Zingabopp/BeatSaberModdingTools.Tasks";
            GetCommitInfo task = new MockGetCommitHash("Test.git")
            {
                ProjectDir = directory,
                NoGit = true,
                HashLength = hashLength
            };
            Assert.IsTrue(task.Execute());
            Console.WriteLine($"Branch: {task.Branch}");
            Assert.AreEqual(expectedBranch, task.Branch);
            Console.WriteLine($"Hash: {task.CommitHash}");
            Assert.AreEqual(expectedHash, task.CommitHash);
            Assert.AreEqual(hashLength, task.CommitHash.Length);
            Assert.AreEqual(expectedUrl, task.OriginUrl);
        }
        [TestMethod]
        public void DefaultHashLength_Manual()
        {
            string directory = Path.Combine(DataFolder, "GitTests");
            string expectedBranch = "master";
            int hashLength = 7;
            string expectedHash = "4197466ed7682542b4669e98fd962a3925ccaadf".Substring(0, hashLength);
            GetCommitInfo task = new MockGetCommitHash("Test.git")
            {
                ProjectDir = directory,
                NoGit = true
            };
            Assert.IsTrue(task.Execute());
            Console.WriteLine($"Branch: {task.Branch}");
            Assert.AreEqual(expectedBranch, task.Branch);
            Console.WriteLine($"Hash: {task.CommitHash}");
            Assert.AreEqual(expectedHash, task.CommitHash);
            Assert.AreEqual(hashLength, task.CommitHash.Length);
        }
        #endregion
    }
}
