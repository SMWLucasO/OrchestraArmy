using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class ExampleTest
{
    // A Test behaves as an ordinary method
    [Test]
    public void ExampleTestSimplePasses()
    {
        // Use the Assert class to test conditions
        Assert.IsTrue(true);
    }

    // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    // `yield return null;` to skip a frame.
    [UnityTest]
    public IEnumerator ExampleTestWithEnumeratorPasses()
    {
        // Use the Assert class to test conditions.
        // Use yield to skip a frame.
        for (var i = 0; i < 100; i++)
        {
            //every yield advances the game by 1 frame
            yield return null;
        }

        //after 100 frames assert this is asserted
        Assert.IsTrue(true);
    }
}
