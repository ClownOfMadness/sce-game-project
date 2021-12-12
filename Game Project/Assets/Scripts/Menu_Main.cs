using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu_Main : MonoBehaviour
{
    public GameObject LoadScreen;
    AsyncOperation AsyOpe = null;

    public void NewGame()
    {
        LoadScreen.SetActive(true);
        StartCoroutine(LoadAsynchronic());
    }

    IEnumerator LoadAsynchronic()
    {
        for (int i = 1; i < 40; i++)
        {
            LoadScreen.GetComponent<Image>().sprite = LoadScreen.GetComponent<SpriteRenderer>().sprite;
            yield return new WaitForSeconds(0.05f);
        }
        SceneManager.LoadSceneAsync("SampleScene");

        yield return null;
    }

    public void ExitGame()
    {
        Debug.Log("Exit");
#if UNITY_EDITOR
        // Application.Quit() does not work in the editor so
        // UnityEditor.EditorApplication.isPlaying need to be set to false to end the game
        UnityEditor.EditorApplication.isPlaying = false;
#else
         Application.Quit();
#endif
    }
}
