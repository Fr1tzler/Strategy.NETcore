using System;
using NUnit.Framework;
using SFML.System;

namespace strategy
{
	[TestFixture]
	public class tests
	{
		[Test]
		public void UnitDies()
		{
			var tank = new TankModel(new Vector2f(0, 0), true);
			tank.GetShot(50);
			Assert.AreEqual(true, tank.Alive);
			tank.GetShot(50);
			Assert.AreEqual(false, tank.Alive);
		}

		[Test]
		public void CheckCorrectPosition()
		{
			var scene = new SceneModel(50, 50);
			var a = new Vector2f(-100, 10);
			var b = new Vector2f(-10, -10);
			Assert.AreEqual(new Vector2f(0, 10), SceneModel.CorrectPosition(a));
			Assert.AreEqual(new Vector2f(0, 0), SceneModel.CorrectPosition(b));
		}

		[Test]
		public void UnitSees()
		{
			var tank = new TankModel(new Vector2f(0, 0), true);
			Assert.AreEqual(true, tank.PointVisible(99, 0));
			Assert.AreEqual(true, tank.PointVisible(0, 99));
			Assert.AreEqual(true, tank.PointVisible(70, 70));
			Assert.AreEqual(false, tank.PointVisible(71, 71));
			Assert.AreEqual(false, tank.PointVisible(101, 0));
			Assert.AreEqual(false, tank.PointVisible(0, 101));
		}
		
		[Test]
		public void UnitFireDistance()
		{
			var tank = new TankModel(new Vector2f(0, 0), true);
			Assert.AreEqual(true, tank.AbleToFire(new Vector2f(89, 0)));
			Assert.AreEqual(true, tank.AbleToFire(new Vector2f(0, 89)));
			Assert.AreEqual(true, tank.AbleToFire(new Vector2f(63, 63)));
			Assert.AreEqual(false, tank.AbleToFire(new Vector2f(65, 65)));
			Assert.AreEqual(false, tank.AbleToFire(new Vector2f(91, 0)));
			Assert.AreEqual(false, tank.AbleToFire(new Vector2f(0, 91)));
		}

		[Test]
		public void UnitReadyToFire()
		{
			var tank = new TankModel(new Vector2f(0, 0), true);
			var t = DateTime.Now;
			while ((DateTime.Now - t).TotalMilliseconds < tank.ReloadTime)
			{
				Assert.AreEqual(false, tank.ReadyToFire());
			}
			Assert.AreEqual(true, tank.ReadyToFire());
		}

		[Test]
		public void UnitMoves()
		{
			var tank = (UnitModel) new TankModel(new Vector2f(0, 0), true);
			var Destination = new Vector2f(0, 30);
			tank.SetDestination(Destination);
			for (var i = 0; i < 10; i++)
			{
				Assert.AreNotEqual(Destination, tank.Position);
				tank.Move();
			}
			Assert.AreEqual(Destination, tank.Position);
		}
	}
}