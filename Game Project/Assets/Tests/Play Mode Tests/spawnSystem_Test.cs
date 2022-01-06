using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NUnit.Framework;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;

public class spawnSystem_Test : MonoBehaviour
{
    [OneTimeSetUp]
    public void loadGame()  //loading game to run play-time testing
    {
        SceneManager.LoadScene("Game", LoadSceneMode.Single);
    }

    //Check if the spaw recource succeded.
    [UnityTest]
    public IEnumerator SpawnResources_Test()
    {
        yield return new WaitForSeconds(1);

        Map_SpawnControl spawn = FindObjectOfType<Map_SpawnControl>();
        Assert.True(spawn.SpawnResources(0, 1));
    }
    //Check if new peasent spawned.
    [UnityTest]
    public IEnumerator CaluculateNewPeasents_Test()
    {
        yield return new WaitForSeconds(1); //wait till game 
        Map_SpawnControl spawn = FindObjectOfType<Map_SpawnControl>();

        spawn.MaxPeasentSpawn = 5;
        spawn.BuildingCapacity = 8;
        spawn.UnitTotal = 5;
        Assert.Equals(spawn.CaluculateNewPeasents(), 1);

        spawn.BuildingCapacity = 2;
        Assert.Equals(spawn.CaluculateNewPeasents(), 2);
    }

    //Check if 8 places has been found aroun the town hall.
    [UnityTest]
    public IEnumerator PeasentPosList_Test()
    {
        yield return new WaitForSeconds(1);

        Map_SpawnControl spawn = FindObjectOfType<Map_SpawnControl>(); 
        spawn.display = FindObjectOfType<Map_Display>();

        Assert.Equals(spawn.PeasentPosList().Count, 8);
    }
}
