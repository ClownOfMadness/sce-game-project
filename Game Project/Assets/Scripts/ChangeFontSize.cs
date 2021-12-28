using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeFontSize : MonoBehaviour
{
    public Screen_Cards screen_Cards;
    public Menu_Pause menu_Pause;
   public void DefaultSize() {  //looks absolutely horrible, I know
                                //but how else am I supposed to change each and every text individually? T_T
    
       screen_Cards.transform.GetChild(0).GetComponent<Text>().fontSize = 40;
       screen_Cards.transform.GetChild(7).GetChild(0).GetChild(0).GetComponent<Text>().fontSize = 20; //peasant
       //screen_Cards.transform.GetChild(7).GetChild(1).GetChild(0).GetComponent<Text>().fontSize = 19; //woodcutter
       screen_Cards.transform.GetChild(7).GetChild(2).GetChild(0).GetComponent<Text>().fontSize = 20; //spearman
       menu_Pause.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Text>().fontSize = 80; //resume
       menu_Pause.transform.GetChild(0).GetChild(1).GetChild(0).GetComponent<Text>().fontSize = 80; //save
       menu_Pause.transform.GetChild(0).GetChild(2).GetChild(0).GetComponent<Text>().fontSize = 80; //load 
       menu_Pause.transform.GetChild(0).GetChild(3).GetChild(0).GetComponent<Text>().fontSize = 80; //options 
       menu_Pause.transform.GetChild(0).GetChild(4).GetChild(0).GetComponent<Text>().fontSize = 80; //exit to menu
    }

    public void BiggerSize()
    {
        screen_Cards.transform.GetChild(0).GetComponent<Text>().fontSize = 59;
        screen_Cards.transform.GetChild(7).GetChild(0).GetChild(0).GetComponent<Text>().fontSize = 27; //peasant
        //screen_Cards.transform.GetChild(7).GetChild(1).GetChild(0).GetComponent<Text>().fontSize = 19; //woodcutter
        screen_Cards.transform.GetChild(7).GetChild(2).GetChild(0).GetComponent<Text>().fontSize = 22; //spearman
        menu_Pause.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Text>().fontSize = 120; //resume
        menu_Pause.transform.GetChild(0).GetChild(1).GetChild(0).GetComponent<Text>().fontSize = 120; //save
        menu_Pause.transform.GetChild(0).GetChild(2).GetChild(0).GetComponent<Text>().fontSize = 120; //load 
        menu_Pause.transform.GetChild(0).GetChild(3).GetChild(0).GetComponent<Text>().fontSize = 120; //options
        menu_Pause.transform.GetChild(0).GetChild(4).GetChild(0).GetComponent<Text>().fontSize = 120; //exit to menu
    }
}
