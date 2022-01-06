using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;

public class pahtfind_test : MonoBehaviour
{
    [OneTimeSetUp]
    public void LoadGame()
    {
        SceneManager.LoadScene("Game", LoadSceneMode.Single);
    }

    [UnityTest]
    public IEnumerator unit_SummonUnit()
    {
        yield return new WaitForSeconds(1);
        Unit_List unitList = GameObject.Find("Units").GetComponent<Unit_List>();
        unitList.SummonUnit(1, unitList.townhall, null, false);
        unitList.SummonUnit(2, unitList.townhall, null, false);
        unitList.SummonUnit(3, unitList.townhall, null, false);
        GameObject woodcutters = unitList.transform.GetChild(1).gameObject;
        yield return new WaitForSeconds(1);
        Assert.AreEqual(1, woodcutters.transform.childCount);
        yield return null;
    }

    [UnityTest]
    public IEnumerator unit_HurtCheck()
    {
        yield return new WaitForSeconds(1);
        Unit_List unitList = GameObject.Find("Units").GetComponent<Unit_List>();
        GameObject peasants = unitList.transform.GetChild(0).gameObject;
        GameObject peasant = peasants.transform.GetChild(Random.Range(0, peasants.transform.childCount)).gameObject;
        Data_Unit dataUnit = peasant.GetComponent<Data_Unit>();
        Enemy_List enemyList = GameObject.Find("Enemies").GetComponent<Enemy_List>();
        GameObject tile = dataUnit.currentTileOn;
        int amount = peasants.transform.childCount;
        GameObject enemy = enemyList.CreateEnemy(0, tile);
        dataUnit.Hurt(10, enemy.GetComponent<Data_Enemy>());
        yield return new WaitForSeconds(2);
        Destroy(enemy);
        Assert.AreNotEqual(amount, peasants.transform.childCount);
        yield return null;
    }

    [UnityTest]
    public IEnumerator unit_TargetLocation()
    {
        yield return new WaitForSeconds(1);
        Unit_List unitList = GameObject.Find("Units").GetComponent<Unit_List>();
        GameObject peasants = unitList.transform.GetChild(0).gameObject;
        GameObject peasant = peasants.transform.GetChild(0).gameObject;
        Data_Unit dataUnit = peasant.GetComponent<Data_Unit>();
        GameObject map = GameObject.Find("Map Generator");
        GameObject tile = map.transform.GetChild(Random.Range(0, map.transform.childCount)).gameObject;
        dataUnit.UpdateTargetLocation(tile);
        yield return new WaitForSeconds(1);
        Assert.AreEqual(tile.transform.position, dataUnit.destinationTile);
        yield return null;
    }

    [UnityTest]
    public IEnumerator unit_TargetRemove()
    {
        yield return new WaitForSeconds(1);
        Unit_List unitList = GameObject.Find("Units").GetComponent<Unit_List>();
        GameObject peasants = unitList.transform.GetChild(0).gameObject;
        GameObject peasant = peasants.transform.GetChild(0).gameObject;
        Data_Unit dataUnit = peasant.GetComponent<Data_Unit>();
        GameObject map = GameObject.Find("Map Generator");
        GameObject tile = map.transform.GetChild(Random.Range(0, map.transform.childCount)).gameObject;
        dataUnit.UpdateTargetLocation(tile);
        yield return new WaitForSeconds(1);
        dataUnit.RemoveTargetLocation();
        yield return new WaitForSeconds(1);
        Assert.AreNotEqual(tile.transform.position, dataUnit.destinationTile);
        yield return null;
    }

    [UnityTest]
    public IEnumerator unit_BusyCheck()
    {
        yield return new WaitForSeconds(1);
        Unit_List unitList = GameObject.Find("Units").GetComponent<Unit_List>();
        GameObject peasants = unitList.transform.GetChild(0).gameObject;
        GameObject peasant = peasants.transform.GetChild(0).gameObject;
        Data_Unit dataUnit = peasant.GetComponent<Data_Unit>();
        GameObject map = GameObject.Find("Map Generator");
        GameObject tile = map.transform.GetChild(Random.Range(0, map.transform.childCount)).gameObject;
        dataUnit.UpdateTargetLocation(tile);
        yield return new WaitForSeconds(1);
        bool busy = dataUnit.busy;
        dataUnit.RemoveTargetLocation();
        yield return new WaitForSeconds(1);
        Assert.AreNotEqual(busy, dataUnit.busy);
        yield return null;
    }

    [UnityTest]
    public IEnumerator unit_JobDurability()
    {
        yield return new WaitForSeconds(1);
        Unit_List unitList = GameObject.Find("Units").GetComponent<Unit_List>();
        unitList.SummonUnit(1, unitList.townhall, null, false);
        GameObject woodcutters = unitList.transform.GetChild(1).gameObject;
        GameObject woodcutter = woodcutters.transform.GetChild(0).gameObject;
        Data_Unit dataUnit = woodcutter.GetComponent<Data_Unit>();
        woodcutter.transform.position = woodcutter.transform.position + new Vector3(10, 0, 0);
        int amount = woodcutters.transform.childCount;
        yield return new WaitForSeconds(1);
        dataUnit.durability = 0;
        yield return new WaitForSeconds(2);
        Assert.AreNotEqual(amount, woodcutters.transform.childCount);
        yield return null;
    }

    [UnityTest]
    public IEnumerator unit_RememberWork()
    {
        yield return new WaitForSeconds(1);
        Unit_List unitList = GameObject.Find("Units").GetComponent<Unit_List>();
        GameObject peasants = unitList.transform.GetChild(0).gameObject;
        GameObject peasant = peasants.transform.GetChild(0).gameObject;
        Data_Unit dataUnit = peasant.GetComponent<Data_Unit>();
        peasant.transform.position = unitList.townhall.transform.position + new Vector3(-22, 0, 0);
        GameObject tile = dataUnit.currentTileOn;
        dataUnit.UpdateTargetLocation(tile);
        yield return new WaitForSeconds(1);
        if (tile.GetComponent<Data_Tile>().CanWork(dataUnit) != -1)
        {
            Assert.AreEqual(dataUnit.working, true);
        }
        else
        {
            Assert.AreEqual(dataUnit.working, false);
        }
        dataUnit.RemoveTargetLocation();
        yield return null;
    }
}
