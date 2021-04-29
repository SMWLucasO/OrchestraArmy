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
        
        //create a new game object
        var go = new GameObject();
        //give it a rigid body
        go.AddComponent<Rigidbody>();
        
        //get it's position
        var originalPosition = go.transform.position.y;

        //wait till the next FixedUpdate fires
        yield return new WaitForFixedUpdate();

        //assert that the position no longer equals the original one
        Assert.AreNotEqual(originalPosition, go.transform.position.y);
    }
}
