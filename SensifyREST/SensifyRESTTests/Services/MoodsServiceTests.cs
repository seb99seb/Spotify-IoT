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


        /// Test Initialize
        /// initialiserer en ny moodService før hver test. 
        [TestInitialize]
        public void BeforEachTest()
        {
            ms = new MoodsService();
        }


        /// tester de accepterede humør-retninger
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


        /// tester nogle IKKE accepterede humør-retninger
        [TestMethod()]
        [DataRow("sa")]
        [DataRow("neeeeuuuutraaaaaaalllllleees")]
        public void GetMoodTestFail(string mood)
        {
            // Act - i test initialize 

            // Arrange i lambdaudtryk i assert

            // Assert
            Assert.ThrowsException<ArgumentException>(() =>
            {
                ms.CheckMood(mood);
            });
        }

        /// Accepterede humør-inputs til at finde spillelisteID
        [TestMethod()]
        [DataRow("Happy")]
        [DataRow("Sad")]
        [DataRow("Neutral")]
        public void GetPlaylistIdSuccess(string mood)
        {
            //Act - i test initialize 

            //Arrange
            string id = ms.GetPlaylistId(mood);

            //Assert
            Assert.IsTrue(id.Length>0);
        }

        /// Ikke accepterede humør-inputs
        [TestMethod()]
        [DataRow("happy")]
        [DataRow("noooo")]
        public void GetPlaylistIdFail(string mood)
        {
            //Act - i test initialize 

            //Arrange - foregår i lambdaudtryk i assert

            //Assert
            Assert.ThrowsException<ArgumentException>(() =>
            {
                string id = ms.GetPlaylistId(mood);
            });
        }

        /// Metode til at teste humøt og spillelister
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