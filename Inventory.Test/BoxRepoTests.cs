using System.Collections.Generic;
using ComicsInventory.DAL.Entities;
using ComicsInventory.DAL.Repositories.Interfaces;
using ComicsInventory.DAL.Repositories.Inventory;
using NUnit.Framework;

namespace Inventory.Test
{
    [TestFixture]
    public class BoxRepoTests
    {
        [Test]
        public void ShouldAddBoxes()
        {
            //SET UP
            IBoxRepo boxRepo = new BoxRepo();
            var b1 = new BoxStore {BoxID = 26, BoxName = "Test 1"};
            var b2 = new BoxStore {BoxID = 27, BoxName = "Test 2"};

            var boxes = new List<BoxStore> {b1, b2};

            //EXECUTE
            foreach (var box in boxes)
                boxRepo.InsertAndSubmit(box);

            //ASSERT
            var s1 = boxRepo.GetById(26);


            Assert.AreEqual(b1, s1);
        }
    }
}