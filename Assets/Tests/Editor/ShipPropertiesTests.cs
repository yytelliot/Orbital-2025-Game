using NUnit.Framework;
using UnityEngine;
using Game.Events;

public class ShipPropertiesTests
{
    
    private ShipProperties ship;

    [SetUp]
    public void Setup()
    {
        var go = new GameObject();
        ship = go.AddComponent<ShipProperties>();
        ship.updateUI = ScriptableObject.CreateInstance<GameEvent>();
        ship.onAmmoCountChange = ScriptableObject.CreateInstance<GameEvent>();
        ship.onShipHpChange = ScriptableObject.CreateInstance<GameEvent>();
        // set basic values
        ship.maxHp = 10;
        ship.currentHp = 10;
        ship.maxHpThresholds = 4;
        ship.currentHpThrehsolds = 4;
        ship.maxAmmoCount = 5;
        ship.currentAmmoCount = 5;
    }

    [TearDown]
    public void TearDown()
    {
        Object.DestroyImmediate(ship.gameObject);
    }

    [Test]
    public void AmmoIsFull_ReturnsTrue_WhenAmmoAtMax()
    {
        ship.currentAmmoCount = ship.maxAmmoCount;
        Assert.IsTrue(ship.AmmoIsFull());
    }

    [Test]
    public void AmmoIsFull_ReturnsFalse_WhenAmmoNotFull()
    {
        ship.currentAmmoCount = ship.maxAmmoCount - 1;
        Assert.IsFalse(ship.AmmoIsFull());
    }

    [Test]
    public void AmmoIsEmpty_ReturnsTrue_WhenAmmoZero()
    {
        ship.currentAmmoCount = 0;
        Assert.IsTrue(ship.AmmoIsEmpty());
    }

    [Test]
    public void AmmoIsEmpty_ReturnsFalse_WhenAmmoAboveZero()
    {
        ship.currentAmmoCount = 1;
        Assert.IsFalse(ship.AmmoIsEmpty());
    }


    [Test]
    public void DeductAmmo_ReturnsFalse_WhenAmmoZero()
    {
        ship.currentAmmoCount = 0;
        var result = ship.DeductAmmo();
        Assert.IsFalse(result);
    }

    [Test]
    public void DeductAmmo_WithAmount_ReturnsFalse_WhenNotEnoughAmmo()
    {
        ship.currentAmmoCount = 2;
        var result = ship.DeductAmmo(3);
        Assert.IsFalse(result);
    }

    [Test]
    public void AddHp_DoesNotAdd_WhenAtCap()
    {
        ship.currentHp = ship.maxHp * ship.currentHpThrehsolds / ship.maxHpThresholds;
        var result = ship.AddHp(5);
        Assert.IsFalse(result);
    }


    [Test]
    public void HpAtCap_True_WhenHpFull()
    {
        ship.currentHp = ship.maxHp * ship.currentHpThrehsolds / ship.maxHpThresholds;
        Assert.IsTrue(ship.HpAtCap());
    }

    [Test]
    public void HpAtCap_False_WhenHpNotFull()
    {
        ship.currentHp = 1;
        Assert.IsFalse(ship.HpAtCap());
    }

    [Test]
    public void HpAtCap_True_WhenHpZero()
    {
        ship.currentHp = 0;
        Assert.IsTrue(ship.HpAtCap());
    }

    [Test]
    public void ChangeHpThresholdBy_WithinBounds()
    {
        ship.currentHpThrehsolds = 2;
        ship.ChangeHpThresholdBy(1);
        Assert.AreEqual(3, ship.currentHpThrehsolds);
    }

    [Test]
    public void ChangeHpThresholdBy_ExceedsMax_ClampsToMax()
    {
        ship.currentHpThrehsolds = ship.maxHpThresholds - 1;
        ship.ChangeHpThresholdBy(2);
        Assert.AreEqual(ship.maxHpThresholds, ship.currentHpThrehsolds);
    }

    [Test]
    public void ChangeHpThresholdBy_BelowZero_ClampsToZero()
    {
        ship.currentHpThrehsolds = 1;
        ship.ChangeHpThresholdBy(-2);
        Assert.AreEqual(0, ship.currentHpThrehsolds);
    }
}
