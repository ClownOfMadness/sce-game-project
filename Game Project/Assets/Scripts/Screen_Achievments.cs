using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Screen_Achievments : MonoBehaviour
{
    private int daysForVeteran = 2;
    private int buildingsForBuilder = 2;
    private int cardsForCrafty = 1;

    private bool isVeteran = false;
    private bool isBuilder = false;
    private bool isCrafty = false;
    public Game_Master GameMaster;
    public GameObject popUp;
    //public Animator animator;
    public TMP_Text popUptext;
    private float setTime = 500f;
    private float timeLeft;

    private float waitTime = 2.0f;
    private float timer = 0.0f;
    private float visualTime = 0.0f;
    private void SetTimer()
    {
        timer += Time.deltaTime;

        // Check if we have reached beyond 2 seconds.
        // Subtracting two is more accurate over time than resetting to zero.
        if (timer > waitTime)
        {
            visualTime = timer;

            // Remove the recorded 2 seconds.
            timer = timer - waitTime;
        }
    }
private void Update()
    {
        if (!isVeteran && GameMaster.MapSpawn.DayCount >= daysForVeteran)
        //if (!GameMaster.isVeteran && GameMaster.MapSpawn.DayCount >= daysForVeteran)
        {
            PopMessage("Acheivment unlocked: Veteran");
            //GameMaster.isVeteran = true;
            isVeteran = true;
        }
        if (!isBuilder && GameMaster.buildingsCount >= buildingsForBuilder)
        //if (!GameMaster.isBuilder && GameMaster.buildingsCount >= buildingsForBuilder)
        {
            PopMessage("Acheivment unlocked: Builder");
            //GameMaster.isBuilder = true;
            isBuilder = true;
        }
        if (!isCrafty && GameMaster.CardsCombined >= cardsForCrafty)
        //if (!GameMaster.isCrafty && GameMaster.CardsCombined >= cardsForCrafty)
        {
            PopMessage("Acheivment unlocked: Crafty");
            //GameMaster.isCrafty = true;
            isCrafty = true;
        }
    }

    public void PopMessage(string message)
    {
        Debug.Log(message);
    //    float alpha = 1;
    //    popUp.SetActive(true);
    //    Color color = popUp.GetComponent<Image>().color;
    //    popUptext.text = message;
    //    timeLeft = setTime;

    //    while (timeLeft >= 0)
    //    {
    //        color = new Color(176, 109, 109, alpha);
    //        alpha -= Time.deltaTime;
    //        timeLeft -= Time.deltaTime;
    //    }
    //    popUp.SetActive(false);
    }
}