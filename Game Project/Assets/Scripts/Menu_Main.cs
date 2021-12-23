using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class Menu_Main : MonoBehaviour
{
    public GameObject LoadPanel;
    public GameObject SaveSlots;
    public TextMeshProUGUI Logo;
    TextMeshProUGUI loadText;

    private void Awake()
    {
        loadText = LoadPanel.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
    }

    public void NewGame()
    {
        LoadPanel.SetActive(true);
        SaveSlots.SetActive(false);

        StartCoroutine(LoadAsynchronic());
    }

    IEnumerator LoadAsynchronic()
    { 
        AsyncOperation asy = SceneManager.LoadSceneAsync("Game");
        GameObject loadScreen = LoadPanel.transform.GetChild(1).gameObject;
        Image img = loadScreen.GetComponent<Image>();
        SpriteRenderer spt = loadScreen.GetComponent<SpriteRenderer>();

        while (!asy.isDone)
        {
            for (int i = 1; i < 100; i++)
            {
                img.sprite = spt.sprite;
                img.transform.localScale = new Vector3(-2, 2, 2);
                if (!((i > 5 && i < 10) || (i > 40 && i < 55) || (i > 80 && i < 90)))
                {
                    loadText.text = string.Format("Loading...{0}%", i);
                    img.transform.localPosition = img.transform.localPosition + new Vector3(12, 0, 0);
                }
                    yield return new WaitForSeconds(0.07f);
                asy.allowSceneActivation = false;
            }
            LoadPanel.SetActive(false);
            asy.allowSceneActivation = true;
        }
        yield return null;
    }

    private void Update()
    {
        if (LoadPanel.activeSelf)
        {
            if (loadText.alpha > 0.5f)
                loadText.alpha -= 0.0005f;
            else loadText.alpha = 1f;
        }

        if (Logo.alpha < 1)
            Logo.alpha += 0.005f;
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
