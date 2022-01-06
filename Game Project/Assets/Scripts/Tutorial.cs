using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{
    public Menu_Main mainMenu;
    public GameObject[] background;
    public int index;

    void Start() //The tutorial setup
    {
        index = 0;
        mainMenu = FindObjectOfType<Menu_Main>();
    }


    void Update()
    {
        //making sure the index always stays within the index range
        if (index < 0)
            index = 0;

        if (index == 0) 
        {
            Time.timeScale = 0f;
            background[0].gameObject.SetActive(true);
        }

    }

    public void Next() //turning to the next page
    {
        if (index<11) {
            index += 1;
            background[index - 1].gameObject.SetActive(false);
            background[index].gameObject.SetActive(true);
        }
        else  //the last page and returning to the game 
        {
            this.gameObject.SetActive(false);
            background[index].gameObject.SetActive(false);
            background[0].gameObject.SetActive(true);
            index = 0;
            Time.timeScale = 1f;
        }
       //Debug.Log(index);
    }

    public void Previous() //turning to the previous page
    {
        if (index > 0) { 
        index -= 1;
        background[index + 1].gameObject.SetActive(false);
        background[index].gameObject.SetActive(true);
        }
        //Debug.Log(index);
    }

    public void Skip() //to skip the tutorial entirely
    {
            this.gameObject.SetActive(false);
            background[index].gameObject.SetActive(false);
            background[0].gameObject.SetActive(true);
            index = 0;
            Time.timeScale = 1f;
            //mainMenu.StartGame();
    }

    public void OpenTutorial() //the funcion to open the tutorial
    {
        this.gameObject.SetActive(true);
    }

}