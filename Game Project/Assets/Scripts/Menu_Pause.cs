using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu_Pause : MonoBehaviour
{
    public static bool IsPaused = false;//Pause state
    public GameObject pauseMenuUI;//Pause Menu
    public GameObject optionsMenuUI;//Options Menu
    public GameObject KeyBindingUI;//Key Binding Menu
    public GameObject keyBindingOption; //KB menu
    public GameObject gamePlayUI;//Game play menue
    public Game_Master game_Master;
    public Dropdown UIDesignDrop; //dropdown for UI design changing
    SpriteState tempState; //for swapping sprites
    [Header("---[UI]---")]
    public Button Hints;
    public Button Book;
    public Button Storage;
    public Button StoragePt2;

    [Header("---[Default Sprites]---")]
    //Hints
    public Sprite Hint1;
    public Sprite Hint1Hover;
    public Sprite Hint1Selected;
    //Book
    public Sprite Book1;
    public Sprite Book1Hover;
    public Sprite Book1Selected;
    //StoragePart1
    public Sprite Storage1;
    public Sprite Storage1Hover;
    public Sprite Storage1Selected;
    //StoragePart2
    public Sprite StorageB1;
    public Sprite StorageB1Selected;


    [Header("---[Golden Sprites]---")]
    //Hints
    public Sprite Hint2;
    public Sprite Hint2Hover;
    public Sprite Hint2Selected;
    //Book
    public Sprite Book2;
    public Sprite Book2Hover;
    public Sprite Book2Selected;
    //StoragePart1
    public Sprite Storage2;
    public Sprite Storage2Hover;
    public Sprite Storage2Selected;
    //StoragePart2
    public Sprite StorageB2;
    public Sprite StorageB2Selected;


    public void Start()
    {
        //UIDesignDrop.value
        /* if (!game_Master.premiumUser)
         {
             keyBindingOption.SetActive(false);
         }
         else
         {
             keyBindingOption.SetActive(true);
         }*/
        //Hints.spriteState.
        //Testing
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {

            SetDefaultDesign();
        }

        if (Input.GetKeyDown(KeyCode.T))
        {

            SetGoldenDesign();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(IsPaused)
                Resume();
            else
                Pause();
        }
    }

    //Resume the game.
    public void Resume()
    {
        //Deactivate all panels when key pressed.
        if(optionsMenuUI.activeSelf)
            optionsMenuUI.SetActive(false);
        if (KeyBindingUI.activeSelf)
            KeyBindingUI.SetActive(false);

        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        IsPaused = false;
    }

    //Pause the game.
    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        IsPaused = true;
    }

    //Loading the main menu.
    public void LoadMenu()
    {
        IsPaused = false;
        Time.timeScale = 1f;
        SceneManager.LoadScene("Menu");
        Debug.Log("Loading menu...please wait");
    }

    public void ChangeDesign()
    {
        if (UIDesignDrop.value == 1)
        {
            SetGoldenDesign();
        }
        else
        {
            SetDefaultDesign();
        }
    }

    private void SetDefaultDesign()
    {
        //-------------------Hints-------------------
        tempState = Hints.spriteState;
        tempState.highlightedSprite = Hint1Hover;
        tempState.selectedSprite = Hint1Selected;
        tempState.pressedSprite = Hint1;
        Hints.spriteState = tempState;
        Hints.GetComponent<Image>().sprite = Hint1;

        //-------------------Book-------------------
        tempState = Book.spriteState;
        tempState.highlightedSprite = Book1Hover;
        tempState.selectedSprite = Book1Selected;
        tempState.pressedSprite = Book1;
        Book.spriteState = tempState;
        Book.GetComponent<Image>().sprite = Book1;

        //-------------------Storage-------------------
        tempState = Storage.spriteState;
        tempState.highlightedSprite = Storage1Hover;
        tempState.selectedSprite = Storage1Selected;
        tempState.pressedSprite = Storage1;
        Storage.spriteState = tempState;
        Storage.GetComponent<Image>().sprite = Storage1;

        //-------------------Storage pt2-------------------
        tempState = StoragePt2.spriteState;
        tempState.highlightedSprite = StorageB1Selected;
        tempState.pressedSprite = StorageB1;
        StoragePt2.spriteState = tempState;
        StoragePt2.GetComponent<Image>().sprite = StorageB1;
    }

    private void SetGoldenDesign()
    {
        //-------------------Hints-------------------
        tempState = Hints.spriteState;
        tempState.highlightedSprite = Hint2Hover;
        tempState.selectedSprite = Hint2Selected;
        tempState.pressedSprite = Hint2;
        Hints.spriteState = tempState;
        Hints.GetComponent<Image>().sprite = Hint2;

        //-------------------Book-------------------
        tempState = Book.spriteState;
        tempState.highlightedSprite = Book2Hover;
        tempState.selectedSprite = Book2Selected;
        tempState.pressedSprite = Book2;
        Book.spriteState = tempState;
        Book.GetComponent<Image>().sprite = Book2;

        //-------------------Storage-------------------
        tempState = Storage.spriteState;
        tempState.highlightedSprite = Storage2Hover;
        tempState.selectedSprite = Storage2Selected;
        tempState.pressedSprite = Storage2;
        Storage.spriteState = tempState;
        Storage.GetComponent<Image>().sprite = Storage2;

        //-------------------Storage pt2-------------------
        tempState = StoragePt2.spriteState;
        tempState.highlightedSprite = StorageB2Selected;
        tempState.pressedSprite = StorageB2;
        StoragePt2.spriteState = tempState;
        StoragePt2.GetComponent<Image>().sprite = StorageB2;
    }
}
