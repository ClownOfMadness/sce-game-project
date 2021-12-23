using UnityEngine;

public class Save_config : MonoBehaviour
{
    const string volume = "volume";
    const string graphics = "graphics";
    const string resolution = "resolution";
    Menu_Options options;

    // The same old Awakening
    void Awake()
    {
        options = this.GetComponent<Menu_Options>();
    }
    // Start is called before the first frame update
    void Start()
    {
        LoadPrefs();
    }

    public void SavePrefs()
    {
        PlayerPrefs.SetFloat(volume, options.GetVolume());
        PlayerPrefs.SetInt(graphics, options.GetGraphics());
        PlayerPrefs.SetInt(resolution, options.GetResolution());
        PlayerPrefs.Save();
    }

    public void LoadPrefs()
    {
        options.SetVolume(PlayerPrefs.GetFloat(volume, 0));
        options.SetQuality(PlayerPrefs.GetInt(graphics, 5));
        options.SetResolution(PlayerPrefs.GetInt(resolution, options.resolutions.Length - 1));
    }
}
