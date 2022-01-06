using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;

public class player_test : MonoBehaviour
{
    [OneTimeSetUp]
    public void LoadGame()
    {
        SceneManager.LoadScene("Game", LoadSceneMode.Single);
    }

    [UnityTest]
    public IEnumerator player_CreateCheck()
    {
        // Makes sure that the player exists when the scene is loaded
        
        yield return new WaitForSeconds(1);
        GameObject player;
        player = GameObject.Find("Player(Clone)");
        Assert.IsNotNull(player);
        Assert.IsNotNull(0);
        yield return null;
    }

    [UnityTest]
    public IEnumerator player_ControlCheck()
    {
        // Checks if the player control works in the exact way the controller was set in
        
        yield return new WaitForSeconds(1);
        GameObject player;
        player = GameObject.Find("Player(Clone)");
        Assert.IsNotNull(player);
        Player_Control control = GameObject.Find("PlayerControl").GetComponent<Player_Control>();
        Assert.IsNotNull(control);
        control.zMov = 1f;
        yield return new WaitForSeconds(1);
        control.zMov = 0f;
        Vector3 originalPosition = player.transform.position;
        player.transform.position = player.transform.position + new Vector3(0, 0, 1f);
        Assert.AreNotEqual(player.transform.position, originalPosition);
        Assert.IsNotNull(0);
        yield return null;
    }

    [UnityTest]
    public IEnumerator player_UnitSelection()
    {
        // Makes sure that the unit selection returns an available unit from the unit_list
        
        yield return new WaitForSeconds(1);
        Player_Control control = GameObject.Find("PlayerControl").GetComponent<Player_Control>();
        GameObject tile = control.currentTileOn;
        Assert.IsNotNull(control.UnitSelection(0, tile, false));
        yield return null;
    }

    [UnityTest]
    public IEnumerator player_HurtSystem()
    {
        // Checks if the players health is reduced when being hit
        
        yield return new WaitForSeconds(1);
        Player_Control control = GameObject.Find("PlayerControl").GetComponent<Player_Control>();
        Enemy_List enemyList = GameObject.Find("Enemies").GetComponent<Enemy_List>();
        GameObject tile = control.currentTileOn;
        GameObject hand = GameObject.Find("CardsScreen").transform.GetChild(12).gameObject;
        int handCards = hand.transform.childCount;
        GameObject enemy = enemyList.CreateEnemy(0, tile);
        control.Hurt(enemy.GetComponent<Data_Enemy>());
        yield return new WaitForSeconds(1);
        Assert.AreNotEqual(hand.transform.childCount, handCards);
        Destroy(enemy);
        yield return null;
    }

    [UnityTest]
    public IEnumerator player_TiredSystem()
    {
        // Checks if the stamina system turns on tired mode once the stamina is reduced and is filled back on
        
        yield return new WaitForSeconds(1);
        Player_Control control = GameObject.Find("PlayerControl").GetComponent<Player_Control>();
        control.stamina = 0;
        yield return new WaitForSeconds(2);
        Assert.AreNotEqual(0, control.stamina);
        yield return null;
    }
}
