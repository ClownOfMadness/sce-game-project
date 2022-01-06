using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;

public class enemy_test : MonoBehaviour
{
    [OneTimeSetUp]
    public void LoadGame()
    {
        SceneManager.LoadScene("Game", LoadSceneMode.Single);
    }

    [UnityTest]
    public IEnumerator enemy_CreateEnemy()
    {
        yield return new WaitForSeconds(1);
        Enemy_List enemyList = GameObject.Find("Enemies").GetComponent<Enemy_List>();
        Player_Control control = GameObject.Find("PlayerControl").GetComponent<Player_Control>();
        GameObject tile = control.currentTileOn;
        GameObject enemy = enemyList.CreateEnemy(0, tile);
        GameObject enemies = enemyList.transform.GetChild(0).gameObject;
        yield return new WaitForSeconds(1);
        Assert.AreEqual(1, enemies.transform.childCount);
        Destroy(enemy);
        yield return new WaitForSeconds(1);
        yield return null;
    }

    [UnityTest]
    public IEnumerator enemy_HurtCheck()
    {
        yield return new WaitForSeconds(1);
        Enemy_List enemyList = GameObject.Find("Enemies").GetComponent<Enemy_List>();
        GameObject enemies = enemyList.transform.GetChild(0).gameObject;
        Unit_List unitList = GameObject.Find("Units").GetComponent<Unit_List>();
        GameObject spearmans = unitList.transform.GetChild(2).gameObject;
        unitList.SummonUnit(2, unitList.townhall, null, false);
        GameObject spearman = spearmans.transform.GetChild(Random.Range(0, spearmans.transform.childCount)).gameObject;
        Data_Unit dataUnit = spearman.GetComponent<Data_Unit>();
        spearman.transform.position = spearman.transform.position + new Vector3(20, 0, 20);
        GameObject tile = dataUnit.currentTileOn;
        yield return new WaitForSeconds(2);
        Assert.AreEqual(1, enemyList.transform.childCount);
        yield return null;
    }
}
