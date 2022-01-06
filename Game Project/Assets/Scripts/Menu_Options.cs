using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class Menu_Options : MonoBehaviour
{
    public Slider volume; //Volume slider
    public Dropdown graphics; //Graghics dropDown
    public AudioMixer mainMixer; //Control the volume
    public Dropdown resDrop; //Resolution dropdown
    public Resolution[] resolutions; //The resolutions available on the screen

    private void Awake()
    {
        resolutions = Screen.resolutions; //Get all the possible resolution to the specific screen
        List<string> resOpt = new List<string>(); //List of the resolution(because AddOptions require list)
        int CurrResIndex = 0;   //Chosen resolution on the dropBox

        //Create the resolutions list.
        for (int i = 0; i < resolutions.Length; i++)
        {
            resOpt.Add(resolutions[i].width + "x" + resolutions[i].height);

            if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
                CurrResIndex = i;
        }
        resDrop.ClearOptions(); //Clear the current options
        resDrop.AddOptions(resOpt); //Add the options that matching to the screen
        resDrop.value = CurrResIndex; //Set the dropBox value to current resolution on the screen
        resDrop.RefreshShownValue(); //Show the updated value of resolution
    }
    void Start()
    {
        ResetAll();
    }

    //Set UI to the correct values.
    private void ResetAll()
    {
        volume.value = GetVolume();
        graphics.value = GetGraphics();
        resDrop.value = GetResolution();
    }
    //Set the audio through the options menu.
    public void SetVolume(float volume)
    {
        mainMixer.SetFloat("volume", volume);
    }
    //Set the graphics quality through te options menu.
    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }
    //Set the screen resolution through te options menu.
    public void SetResolution(int resIndex)
    {
        Screen.SetResolution(resolutions[resIndex].width, resolutions[resIndex].height, Screen.fullScreen);
    }
    //Get the current audio.
    public float GetVolume()
    {
        float volume = 0;
        mainMixer.GetFloat("volume", out volume);
        return volume;
    }
    //Get the current graphics quality.
    public int GetGraphics()
    {
        int qualityIndex = 5;
        qualityIndex = QualitySettings.GetQualityLevel();
        return qualityIndex;
    }
    //Get the current resolution.
    public int GetResolution()
    {
        for (int i = 0; i < resolutions.Length; i++)
            if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
               return i;
        return 0;
    }

    public void SetKey(string K)
    {

    }
}
