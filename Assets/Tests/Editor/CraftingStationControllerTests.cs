using System.Reflection;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class ScanningStationControllerTests
    {
        ScanningStationController controller;
        GameObject stationGO;
        GameObject difficultyUI;
        GameObject highlight;
        GameObject playerGO;
        BoxCollider2D playerCollider;

        MethodInfo awakeMethod;
        MethodInfo onEnterMethod;
        MethodInfo onExitMethod;

        [SetUp]
        public void SetUp()
        {
            // Reset singleton
            typeof(ScanningStationController)
                .GetField("Instance", BindingFlags.Public | BindingFlags.Static)
                .SetValue(null, null);

            // Create the station and grab its private Unity‐message methods
            stationGO = new GameObject("Station");
            controller = stationGO.AddComponent<ScanningStationController>();

            awakeMethod   = typeof(ScanningStationController)
                                .GetMethod("Awake", BindingFlags.Instance | BindingFlags.NonPublic);
            onEnterMethod = typeof(ScanningStationController)
                                .GetMethod("OnTriggerEnter2D", BindingFlags.Instance | BindingFlags.NonPublic);
            onExitMethod  = typeof(ScanningStationController)
                                .GetMethod("OnTriggerExit2D", BindingFlags.Instance | BindingFlags.NonPublic);

            // Dummy UI & highlight
            difficultyUI = new GameObject("UI");
            difficultyUI.SetActive(false);
            highlight    = new GameObject("Highlight");
            highlight.SetActive(false);

            // Inject them into the private fields
            var t = typeof(ScanningStationController);
            t.GetField("difficultyUI", BindingFlags.NonPublic | BindingFlags.Instance)
             .SetValue(controller, difficultyUI);
            t.GetField("highlight",    BindingFlags.Public    | BindingFlags.Instance)
             .SetValue(controller, highlight);

            // Player GameObject with Collider
            playerGO      = new GameObject("Player");
            playerGO.tag  = "Player";
            playerCollider = playerGO.AddComponent<BoxCollider2D>();

            var realPC = playerGO.AddComponent<PlayerController>();
            t.GetField("player", BindingFlags.NonPublic | BindingFlags.Instance)
             .SetValue(controller, realPC);
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(stationGO);
            Object.DestroyImmediate(difficultyUI);
            Object.DestroyImmediate(highlight);
            Object.DestroyImmediate(playerGO);
        }

        [Test]
        public void Awake_SetsSingletonInstance()
        {
            awakeMethod.Invoke(controller, null);
            Assert.AreEqual(controller, ScanningStationController.Instance);
        }

        [Test]
        public void OnTriggerEnter2D_WithPlayerTag_ActivatesHighlight()
        {
            awakeMethod.Invoke(controller, null);
            onEnterMethod.Invoke(controller, new object[] { playerCollider });
            Assert.IsTrue(highlight.activeSelf);
        }

        [Test]
        public void OnTriggerExit2D_WithPlayerTag_DeactivatesHighlight()
        {
            highlight.SetActive(true);
            onExitMethod.Invoke(controller, new object[] { playerCollider });
            Assert.IsFalse(highlight.activeSelf);
        }

        [Test]
        public void Interact_ShowsDifficultyUI()
        {

            Assert.IsFalse(difficultyUI.activeSelf);
            controller.Interact();
            Assert.IsTrue(difficultyUI.activeSelf);
        }

        [Test]
        public void SendResult_LogsPassOrFail()
        {
            // Success case
            LogAssert.Expect(LogType.Log, "You Pass");
            controller.SendResult(true, 1);

            // Failure case
            LogAssert.Expect(LogType.Log, "You Fail");
            controller.SendResult(false, 1);
        }
    }
}
