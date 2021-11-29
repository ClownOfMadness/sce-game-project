using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class PathfindingTest
{
    [UnityTest]
    public IEnumerable TestMovement()
    {
        var gameObject = new GameObject();
        var camera = gameObject.AddComponent<PlayerControl>();
        yield return null;
    }
}
