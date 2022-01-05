using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class Menu_Main : MonoBehaviour
{
    public GameObject LoadPanel;
    public GameObject MenuMain;
    public GameObject GameMenu;

    public TextMeshProUGUI Logo;
    public GameObject blackScreen;
    public RawImage backGround;
    public float bGSpeed;
    TextMeshProUGUI loadText;

    //----------------------------Save System----------------------------//

    //[Per Save]//   
    public int charLook = 0;
    public bool bedtimeSet = false;
    public float bedtime = 0;
    public bool timeLimitSet = false;
    public float timeLimit = 0;
    public float timeLeft = 0;
    public int fontSize = 0;    //0=Normal, 1=Big
    public bool hintsOn = false;
    public int gameSpeed = 0;   //0=Normal, 1=Slow
    public bool enemiesOff = false;
    public int difficulty = 1;  //0=Easy, 1=Normal, 2=Hardcore
    public bool fogOff = false;
    public float TotalGameTime = 0;
    public int CardsCombined = 0;
    public int CardsDiscovered = 0;
    public int gameDays = 0;
    public int buildingsCount = 0;
    //[Per Save]// 

    //[Global save]//
    public int windowLook;
    public bool isVeteran;
    public bool isBuilder;
    public bool isCrafty;
    //[Global save]//

    //----------------------------Save System----------------------------//

    private void Awake()
    {
        loadText = LoadPanel.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
        //PlayerPrefs.SetInt("ChangeFont", 0); //*for testing font size*
        //PlayerPrefs.SetInt("GameSpeed", 0); //*for testing game speed*
    }

    public void NewGame()
    {
        LoadPanel.SetActive(true);
        GameMenu.SetActive(false);
        StartGame();
    }

    public void StartGame()
    {
        StartCoroutine(LoadAsynchronic());
    }

    IEnumerator LoadAsynchronic()
    {
        blackScreen.SetActive(true);
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
        backGround.uvRect = new Rect(Time.time * bGSpeed, 0, 0.8f, 0.8f);
        
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
