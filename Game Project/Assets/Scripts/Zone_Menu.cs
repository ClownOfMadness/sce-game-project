using UnityEngine;

//responsible for creating Menu prompt zone, extension of ZoneBehaviour
public class Zone_MenuOther : MonoBehaviour
{
    [HideInInspector] public int Size;              //Zone size
    public void ClearZone()
    {
        foreach (Transform cardObject in this.transform)
        {
            GameObject.Destroy(cardObject.gameObject);
        }
        this.gameObject.SetActive(false);
        Time.timeScale = 1f;    //resume game
    }
}

