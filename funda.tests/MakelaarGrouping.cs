using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using funda.service;
using funda.razor;

namespace funda.tests
{
    public class MakelaarGrouping
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1()
        {
            var l = new List<Object1> {
                new Object1 { MakelaarId = 1, MakelaarNaam = "Test1" },
                new Object1 { MakelaarId = 1, MakelaarNaam = "Test1" },
                new Object1 { MakelaarId = 1, MakelaarNaam = "Test1" },
                new Object1 { MakelaarId = 2, MakelaarNaam = "Test2" },
                new Object1 { MakelaarId = 2, MakelaarNaam = "Test2" },
                new Object1 { MakelaarId = 3, MakelaarNaam = "Test3" },
            };

            var grouping = l.GroupByMakelaarId().ToList();
            Assert.AreEqual(grouping[0].MakelaarId, 1);
            Assert.AreEqual(grouping[0].Objects.Count, 3);
            Assert.AreEqual(grouping[1].MakelaarId, 2);
            Assert.AreEqual(grouping[1].Objects.Count, 2);
            Assert.AreEqual(grouping[2].MakelaarId, 3);
            Assert.AreEqual(grouping[2].Objects.Count, 1);
        }
    }
}