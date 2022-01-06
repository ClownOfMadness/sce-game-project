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
    public GameObject gamePlayUI;//Game play menue
    public Game_Master game_Master;
    public Save_Manager save_Manager;
    public Dropdown UIDesignDrop; //dropdown for UI design changing
    SpriteState tempState; //for swapping sprites
    [Header("---[UI]---")]
    public Button Hints;
    public Button Book;
    public Button Storage;
    public Button StoragePt2;
    public Image StorageWindow;
    public Image Hand;
    public Button Job;
    public Button Delete;

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
    //Storage Window
    public Sprite DSWindow;
    //Hand
    public Sprite DHand;
    //Job
    public Sprite Job1;
    public Sprite Job1Hover;
    //Delete
    public Sprite Delete1;
    public Sprite Delete1Hover;


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
    //Storage Sprite
    public Sprite GSWindow;
    //Hand
    public Sprite GHand;
    //Job
    public Sprite Job2;
    public Sprite Job2Hover;
    //Delete
    public Sprite Delete2;
    public Sprite Delete2Hover;

    private void Start()
    {
        UIDesignDrop.value = game_Master.windowLook;
        ChangeDesign();
    }

    void Update()
    {
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
        save_Manager.Save();
        SceneManager.LoadScene("Menu");
    }

    public void ChangeDesign()
    {
        if (UIDesignDrop.value == 1)
        {
            SetGoldenDesign();
            UIDesignDrop.value = 1;
        }
        else
        {
            SetDefaultDesign();
            UIDesignDrop.value = 0;
        }
        game_Master.windowLook = UIDesignDrop.value;
    }

    private void SetDefaultDesign()
    {
        //-------------------Hints-------------------
        tempState = Hints.spriteState;
        tempState.highlightedSprite = Hint1Hover;
        tempState.pressedSprite = Hint1Selected;
        Hints.spriteState = tempState;
        Hints.GetComponent<Image>().sprite = Hint1;

        //-------------------Book-------------------
        tempState = Book.spriteState;
        tempState.highlightedSprite = Book1Hover;
        tempState.pressedSprite = Book1Selected;
        Book.spriteState = tempState;
        Book.GetComponent<Image>().sprite = Book1;

        //-------------------Storage-------------------
        tempState = Storage.spriteState;
        tempState.highlightedSprite = Storage1Hover;
        tempState.pressedSprite = Storage1Selected;
        Storage.spriteState = tempState;
        Storage.GetComponent<Image>().sprite = Storage1;

        //-------------------Storage pt2-------------------
        tempState = StoragePt2.spriteState;
        tempState.highlightedSprite = StorageB1Selected;
        tempState.pressedSprite = StorageB1;
        StoragePt2.spriteState = tempState;
        StoragePt2.GetComponent<Image>().sprite = StorageB1;

        //-------------------Storage Window-------------------
        StorageWindow.sprite = DSWindow;

        //-------------------Hand-------------------
        Hand.sprite = DHand;

        //-------------------Job-------------------
        tempState = Job.spriteState;
        tempState.highlightedSprite = Job1Hover;
        Job.spriteState = tempState;
        Job.GetComponent<Image>().sprite = Job1;

        //-------------------Delete-------------------
        tempState = Delete.spriteState;
        tempState.highlightedSprite = Delete1Hover;
        Delete.spriteState = tempState;
        Delete.GetComponent<Image>().sprite = Delete1;

    }

    private void SetGoldenDesign()
    {
        //-------------------Hints-------------------
        tempState = Hints.spriteState;
        tempState.highlightedSprite = Hint2Hover;
        tempState.pressedSprite = Hint2Selected;
        Hints.spriteState = tempState;
        Hints.GetComponent<Image>().sprite = Hint2;

        //-------------------Book-------------------
        tempState = Book.spriteState;
        tempState.highlightedSprite = Book2Hover;
        tempState.pressedSprite = Book2Selected;
        Book.spriteState = tempState;
        Book.GetComponent<Image>().sprite = Book2;

        //-------------------Storage-------------------
        tempState = Storage.spriteState;
        tempState.highlightedSprite = Storage2Hover;
        tempState.pressedSprite = Storage2Selected;
        Storage.spriteState = tempState;
        Storage.GetComponent<Image>().sprite = Storage2;

        //-------------------Storage pt2-------------------
        tempState = StoragePt2.spriteState;
        tempState.highlightedSprite = StorageB2Selected;
        tempState.pressedSprite = StorageB2;
        StoragePt2.spriteState = tempState;
        StoragePt2.GetComponent<Image>().sprite = StorageB2;

        //-------------------Storage Window-------------------
        StorageWindow.sprite = GSWindow;

        //-------------------Hand-------------------
        Hand.sprite = GHand;

        //-------------------Job-------------------
        tempState = Job.spriteState;
        tempState.highlightedSprite = Job2Hover;
        Job.spriteState = tempState;
        Job.GetComponent<Image>().sprite = Job2;

        //-------------------Delete-------------------
        tempState = Delete.spriteState;
        tempState.highlightedSprite = Delete2Hover;
        Delete.spriteState = tempState;
        Delete.GetComponent<Image>().sprite = Delete2;
    }
}
