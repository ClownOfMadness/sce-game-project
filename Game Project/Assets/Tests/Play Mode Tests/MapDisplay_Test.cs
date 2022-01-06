using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NUnit.Framework;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;

public class MapDisplay_Test : MonoBehaviour
{
    Data_Player player = null;

    [OneTimeSetUp]
    public void loadGame()  //loading game to run play-time testing
    {
        SceneManager.LoadScene("Game", LoadSceneMode.Single);
    }

    //Check if the data saved.
    [UnityTest]
    public IEnumerator A_SaveData_test()
    {
        yield return new WaitForSeconds(3);

        Map_Display map = FindObjectOfType<Map_Display>();
        player = map.Save_Data();
    }
    //Check if the map saved.
    [UnityTest]
    public IEnumerator B_SaveMap_Test()
    {
        yield return new WaitForSeconds(1);
        Map_Display map = FindObjectOfType<Map_Display>();
        Map_Display.Save_Tile[,] Map = map.SaveMap();
        Assert.IsNotNull(Map);
    }
    //Check if the tile saved.
    [UnityTest]
    public IEnumerator C_Tile_Test()
    {
        yield return new WaitForSeconds(1);
        Map_Display map = FindObjectOfType<Map_Display>();
        Map_Display.Save_Tile tile = map.SaveTile(map.TownHall, 0, 0);
        Assert.IsNotNull(tile);
    }
    //Check if the building saved.
    [UnityTest]
    public IEnumerator D_TileData_Test()
    {
        yield return new WaitForSeconds(1);
        Map_Display map = FindObjectOfType<Map_Display>();
        Map_Display.Save_TileData tileData = map.SaveTileData(map.TownHall.GetComponent<Data_Tile>());
        Assert.IsNotNull(tileData);
    }
    //Check if the tile units saved.
    [UnityTest]
    public IEnumerator E_UnitList_Test()
    {
        yield return new WaitForSeconds(1);
        Map_Display map = FindObjectOfType<Map_Display>();
        List<List<Map_Display.Save_Unit>> units = map.SaveUnitList();
        Assert.IsNotNull(units); 
    }
    //Check if the enemies saved.
    public IEnumerator F_EnemyList_Test()
    {
        yield return new WaitForSeconds(1);
        Map_Display map = FindObjectOfType<Map_Display>();
        List<List<Map_Display.Save_Enemy>> enemies = map.SaveEnemyList();
        Assert.IsNotNull(enemies);
    }
    //Check if the player data loaded.
    [UnityTest]
    public IEnumerator G_LoadData_test()
    {
        yield return new WaitForSeconds(1);
        Map_Display map = FindObjectOfType<Map_Display>();
        player = map.Save_Data();
        Assert.IsNotNull(map.LoadData(player));
    }
}
