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

    public Game_Master GameMaster;
    public GameObject popUp;
    public TMP_Text popUptext;
    private int setTime = 5;


    private void Update()
    {
        if (!GameMaster.isVeteran && GameMaster.MapSpawn.DayCount >= daysForVeteran)
        {
            PopMessage("Acheivment unlocked: Veteran");
            GameMaster.isVeteran = true;
        }
        if (!GameMaster.isBuilder && GameMaster.buildingsCount >= buildingsForBuilder)
        {
            PopMessage("Acheivment unlocked: Builder");
            GameMaster.isBuilder = true;
        }
        if (!GameMaster.isCrafty && GameMaster.CardsCombined >= cardsForCrafty)
        {
            PopMessage("Acheivment unlocked: Craftsman");
            GameMaster.isCrafty = true;
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