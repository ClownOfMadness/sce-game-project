using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class EnemiesStatus_test
{
    [Test]
    public void IsParent()
    {
        Screen_Parent screen_Parent = new Screen_Parent();
        Assert.IsTrue(!screen_Parent.ParentOptions);
    }

    [Test]
    public void Test_enemiesOn()
    {
        Game_Master game_Master = new Game_Master();
        Assert.IsFalse(game_Master.enemiesOff);
    }

    [Test]
    public void Test_MapSpawnEnemies()
    {
        Map_SpawnControl map_SpawnControl = new Map_SpawnControl();
        Assert.IsFalse(map_SpawnControl.EnemySpawn);
    }


    // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    // `yield return null;` to skip a frame.
    /*[UnityTest]
    public IEnumerator EnemiesStatus_testWithEnumeratorPasses()
    {
        // Use the Assert class to test conditions.
        // Use yield to skip a frame.
        yield return null;
    }*/
}
