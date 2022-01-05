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
    public TMP_Text popUptext;
    private int setTime = 5;


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
            PopMessage("Acheivment unlocked: Craftsman");
            //GameMaster.isCrafty = true;
            isCrafty = true;
        }
    }

    public void PopMessage(string message)
    {
        Debug.Log(message);
        popUptext.text = message;
        Color color = popUp.GetComponent<Image>().color;
        popUp.SetActive(true);
        StartCoroutine(Pop());
    }

    private IEnumerator Pop()
    {
        yield return new WaitForSeconds(setTime);
        popUp.SetActive(false);
        yield return null;
    }
}