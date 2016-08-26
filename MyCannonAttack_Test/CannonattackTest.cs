using NUnit.Framework;
using MyCannonAttack;
using System.Collections.Generic;
using System;

namespace MyCannonAttack_Test
{
    [TestFixture]
    public class CannonAttackTest
    {
        private static Cannon cannon;

        [OneTimeSetUp]
        public static void CannonTestsInitialize(){
            cannon = Cannon.GetInstance();
        }

        [SetUp]
        public void ResetCannonObject()
        {
            cannon.Reset();
        }

        /// <summary>
        /// Kanone vorhanden?
        /// </summary>
        [Test]
        public void TestCannonIDValid()
        {
            Cannon cannon = Cannon.GetInstance();
            Assert.IsNotNull(cannon.ID);
        }

        /// <summary>
        /// Es dard nur eine Kanone erzeugt werden - Singleton-Instanz
        /// </summary>
        [Test]
        public void TestCannonMultipleInstances()
        {
            Cannon cannon = Cannon.GetInstance();
            Cannon cannon2 = Cannon.GetInstance();
            Assert.IsTrue(cannon == cannon2);
        }

        /// <summary>
        /// Schusswinkel sollte nicht über 90° liegen
        /// </summary>
        [Test]
        public void TestCannonShootIncorrectAngle()
        {
            Cannon cannon = new Cannon();
            var shot = cannon.Shoot(95, 100);
            Assert.IsFalse(shot.Item1);
        }

        /// <summary>
        /// Überprüfe die Geschwindigkeit der Kugel
        /// </summary>
        [Test]
        public void TestCannonShootVelocityGreaterThanSpeedOfLight()
        {
            var shot = cannon.Shoot(45, 300000001);
            Assert.IsFalse(shot.Item1);
        }

        /// <summary>
        /// Wurde Target verfehlt? Distanz wird überprüft
        /// </summary>
        [Test]
        public void TestCannonShootMiss()
        {
            cannon.SetTarget(4000);
            var shot = cannon.Shoot(45, 350);
            Assert.IsTrue(shot.Item2 == "Missed cannonball landed at 12621 meters");
        }

        /// <summary>
        /// Wurde Target getroffen? Distanz wird überprüft
        /// </summary>
        [Test]
        public void TestCannonShootHit()
        {
            cannon.SetTarget(12621);
            var shot = cannon.Shoot(45, 350);
            Assert.IsTrue(shot.Item2 == "Hit - 1 Shot(s)");
        }

        [Test]
        public void TestCannonCountShots()
        {
            cannon.SetTarget(12621);
            var shot = cannon.Shoot(45, 350);
            Assert.IsTrue(shot.Item2 == "Hit - 1 Shot(s)", "Number of shots:" + cannon.Shots);
        }
    }
}
