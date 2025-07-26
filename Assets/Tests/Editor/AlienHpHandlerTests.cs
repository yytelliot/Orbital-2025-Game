using NUnit.Framework;
using UnityEngine;

public class AlienHpHandlerTests
{
    [Test]
    public void TakeDamage_Kills_WhenHpZeroOrLess()
    {
        // Setup singleton
        var controllerGO = new GameObject("TestPilotGameController");
        var controller = controllerGO.AddComponent<PilotGameController>();

        // You can set controller.difficultyMultiplier if needed
        controller.difficultyMultiplier = 1f;

        // Setup AlienHpHandler
        var go = new GameObject();
        var hpHandler = go.AddComponent<AlienHpHandler>();
        hpHandler.baseMaxHp = 10;

        // force maxHp, currentHp using reflection (because they're private)
        typeof(AlienHpHandler)
            .GetField("maxHp", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            .SetValue(hpHandler, 5);
        typeof(AlienHpHandler)
            .GetField("currentHp", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            .SetValue(hpHandler, 2);

        // Act
        hpHandler.TakeDamage(3);
    }
}