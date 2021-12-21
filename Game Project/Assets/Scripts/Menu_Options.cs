using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Menu_Options : MonoBehaviour
{
    public TextMeshProUGUI Logo;
    float cntdnw = 5.0f;

    // Start is called before the first frame update
    private void Awake()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (cntdnw > 0 || Logo.alpha == 1)
        {
            cntdnw -= Time.deltaTime;
            Logo.alpha += 0.005f;
        }
        double b = System.Math.Round(cntdnw, 2);
        if (cntdnw < 0)
        {
            //Logo.SetActive(false);
        }
    }
}
