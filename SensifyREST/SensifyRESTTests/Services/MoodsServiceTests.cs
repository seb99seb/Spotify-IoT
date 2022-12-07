using Microsoft.VisualStudio.TestTools.UnitTesting;
using SensifyREST.Models;
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
        private Moods m;
        private MoodsService ms;

        [TestInitialize]
        public void BeforEachTest()
        {
            m = new Moods();
            ms = new MoodsService(m);
        }


        [TestMethod()]
        [DataRow("happy")]
        [DataRow("sad")]
        [DataRow("neutral")]
        public void GetMoodTestOK(string mood)
        {
            // act
            string expectedMood = mood;
            m.Mood = mood;

            // Assert

            Assert.AreEqual(expectedMood, m.Mood);
        }

        [TestMethod()]
        [DataRow("sa")]
        [DataRow("neeeeuuuutraaaaaaalllllleees")]

        public void GetMoodTestFail(string mood)
        {
            // Act
            string expectedMood = mood;

            // Assert
            Assert.ThrowsException<ArgumentException>(() =>
            {
                m.Mood = mood;
            });
        }
        [TestMethod()]
        [DataRow("happy")]
        [DataRow("sad")]
        [DataRow("neutral")]
        public void GetPlaylistIdSuccess(string mood)
        {
            //Act

            //Arrange
            string id = ms.GetPlaylistId(mood);

            //Assert
            Assert.IsTrue(id.Length>0);
        }
        [TestMethod()]
        [DataRow("Happy")]
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
        [DataRow("happy", "happytest")]
        [DataRow("sad", "sadtest")]
        [DataRow("neutral", "neutraltest")]
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