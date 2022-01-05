using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{
    public Menu_Main mainMenu;
    public GameObject[] background;
    public int index;

    void Start()
    {
        index = 0;
        mainMenu = FindObjectOfType<Menu_Main>();
    }


    void Update()
    {

        if (index < 0)
            index = 0;

        if (index == 0)
        {
            Time.timeScale = 0f;
            background[0].gameObject.SetActive(true);
        }

    }

    public void Next()
    {
        if (index<11) {
            index += 1;
            background[index - 1].gameObject.SetActive(false);
            background[index].gameObject.SetActive(true);
        }
        else 
        {
            this.gameObject.SetActive(false);
            background[index].gameObject.SetActive(false);
            background[0].gameObject.SetActive(true);
            index = 0;
            Time.timeScale = 1f;
            //mainMenu.StartGame();
        }
       //Debug.Log(index);
    }

    public void Previous()
    {
        if (index > 0) { 
        index -= 1;
        background[index + 1].gameObject.SetActive(false);
        background[index].gameObject.SetActive(true);
        }
        //Debug.Log(index);
    }

    public void Skip()
    {
            this.gameObject.SetActive(false);
            background[index].gameObject.SetActive(false);
            background[0].gameObject.SetActive(true);
            index = 0;
            Time.timeScale = 1f;
            //mainMenu.StartGame();
    }

    public void OpenTutorial()
    {
        this.gameObject.SetActive(true);
    }

}