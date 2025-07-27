using System.Reflection;
using NUnit.Framework;
using UnityEngine;
using TMPro;

namespace Tests
{
    public class AmmoScrollerTests
    {
        AmmoScroller scroller;
        GameObject scrollerGO;
        GameObject textGO;
        TextMeshProUGUI tmpText;
        MethodInfo awakeMethod;

        [SetUp]
        public void SetUp()
        {
            // 1) Reset the static Instance
            typeof(AmmoScroller)
                .GetField("Instance", BindingFlags.Public | BindingFlags.Static)
                .SetValue(null, null);

            // 2) Create the scroller component
            scrollerGO = new GameObject("AmmoScroller");
            scroller = scrollerGO.AddComponent<AmmoScroller>();

            // 3) Grab its private Awake() via reflection
            awakeMethod = typeof(AmmoScroller)
                .GetMethod("Awake", BindingFlags.Instance | BindingFlags.NonPublic);

            // 4) Create a TMP_Text (TextMeshProUGUI) for statusText
            textGO = new GameObject("StatusText");
            tmpText = textGO.AddComponent<TextMeshProUGUI>();

            // 5) Inject it into the private field
            typeof(AmmoScroller)
                .GetField("statusText", BindingFlags.NonPublic | BindingFlags.Instance)
                .SetValue(scroller, tmpText);
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(scrollerGO);
            Object.DestroyImmediate(textGO);
        }

        [Test]
        public void Awake_SetsSingletonAndConvertsBeatTempo()
        {
            scroller.beatTempo = 120f;           // “120 beats per minute”
            awakeMethod.Invoke(scroller, null);  // now runs Awake()

            // Instance assigned
            Assert.AreEqual(scroller, AmmoScroller.Instance);

            // beatTempo ÷= 60 → 2 units per second
            Assert.AreEqual(2f, scroller.beatTempo);
        }

        [Test]
        public void SetScrolling_TogglesIsScrolling()
        {
            scroller.SetScrolling(true);
            Assert.IsTrue(scroller.IsScrolling(), "Should return true after SetScrolling(true)");

            scroller.SetScrolling(false);
            Assert.IsFalse(scroller.IsScrolling(), "Should return false after SetScrolling(false)");
        }

        [Test]
        public void AddScore_IncrementsCounterAndUpdatesText()
        {
            // initial state
            Assert.AreEqual(0, scroller.GetScore());
            Assert.IsNull(tmpText.text);

            scroller.AddScore();
            Assert.AreEqual(1, scroller.GetScore());
            Assert.AreEqual("Ammo: 1", tmpText.text);

            scroller.AddScore();
            Assert.AreEqual(2, scroller.GetScore());
            Assert.AreEqual("Ammo: 2", tmpText.text);
        }

        [Test]
        public void ResetScore_ResetsCounterToZero()
        {
            scroller.AddScore();
            scroller.AddScore();
            Assert.AreEqual(2, scroller.GetScore());

            scroller.ResetScore();
            Assert.AreEqual(0, scroller.GetScore());
        }
    }
}
