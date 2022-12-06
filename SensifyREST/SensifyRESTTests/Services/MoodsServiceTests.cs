using Microsoft.VisualStudio.TestTools.UnitTesting;
using SensifyREST.models;
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
        public void GetMoodTestok(string mood)
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
        public void GetPlaylistIdSuccess()
        {
            //Act

            //Arrange
            int id = ms.GetPlaylistId();

            //Assert
            Assert.IsTrue(id>0);
        }
    }
}