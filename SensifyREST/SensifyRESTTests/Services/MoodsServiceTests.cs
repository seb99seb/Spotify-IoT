using Microsoft.VisualStudio.TestTools.UnitTesting;
using SensifyREST.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SensifyREST.Service.Tests
{
    [TestClass()]
    public class MoodsServiceTests
    {
        // shared instance field
        private MoodsService ms;

        [TestInitialize]
        public void BeforEachTest()
        {
            ms = new MoodsService();
        }

        [TestMethod()]
        [DataRow("Happy")]
        [DataRow("Sad")]
        [DataRow("Neutral")]
        public void GetMoodTestOK(string mood)
        {
            // act
            string actualMood = ms.CheckMood(mood);

            // Assert

            Assert.AreEqual(mood, actualMood);
        }

        [TestMethod()]
        [DataRow("sa")]
        [DataRow("neeeeuuuutraaaaaaalllllleees")]

        public void GetMoodTestFail(string mood)
        {
            // Act

            // Assert
            Assert.ThrowsException<ArgumentException>(() =>
            {
                ms.CheckMood(mood);
            });
        }
        [TestMethod()]
        [DataRow("Happy")]
        [DataRow("Sad")]
        [DataRow("Neutral")]
        public void GetPlaylistIdSuccess(string mood)
        {
            //Act

            //Arrange
            string id = ms.GetPlaylistId(mood);

            //Assert
            Assert.IsTrue(id.Length>0);
        }
        [TestMethod()]
        [DataRow("happy")]
        [DataRow("noooo")]
        public void GetPlaylistIdFail(string mood)
        {
            //Act

            //Arrange

            //Assert
            Assert.ThrowsException<ArgumentException>(() =>
            {
                string id = ms.GetPlaylistId(mood);
            });
        }
        [TestMethod()]
        [DataRow("Happy", "happytest")]
        [DataRow("Sad", "sadtest")]
        [DataRow("Neutral", "neutraltest")]
        public void SavePlaylistIdSuccess(string mood, string playlistId)
        {
            //Act

            //Arrange
            ms.SavePlaylistId(mood, playlistId);
            string id = ms.GetPlaylistId(mood);

            //Assert
            Assert.AreEqual(id, playlistId);
        }
    }
}