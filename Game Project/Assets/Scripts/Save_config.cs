using UnityEngine;
using UnityEngine.SceneManagement;

public class Save_config : MonoBehaviour
{
    const string volume = "volume";
    const string graphics = "graphics";
    const string resolution = "resolution";
    //const int FontSize = 0;
    //Game_Master GM;
    Menu_Options options;
    KeyBinding keyBinding;

    // The same old Awakening
    void Awake()
    {
        if (SceneManager.GetActiveScene().name == "Menu")
            options = this.GetComponent<Menu_Options>();
        else if(SceneManager.GetActiveScene().name == "Game")
            keyBinding = this.GetComponent<KeyBinding>();
    }
    // Start is called before the first frame update
    void Start()
    {
        LoadPrefs();
    }

    public void SavePrefs()
    {
        if (SceneManager.GetActiveScene().name == "Menu")
        {
            PlayerPrefs.SetFloat(volume, options.GetVolume());
            PlayerPrefs.SetInt(graphics, options.GetGraphics());
            PlayerPrefs.SetInt(resolution, options.GetResolution());
        }
        else if (SceneManager.GetActiveScene().name == "Game")
        {
            //-------------key bindings-----------------
            PlayerPrefs.SetString("Creative", keyBinding.GetKey("Creative"));
            PlayerPrefs.SetString("Storage", keyBinding.GetKey("Storage"));
            PlayerPrefs.SetString("Hints", keyBinding.GetKey("Hints"));
            PlayerPrefs.SetString("Jobs", keyBinding.GetKey("Jobs"));
            PlayerPrefs.SetString("MoveUp", keyBinding.GetKey("MoveUp"));
            PlayerPrefs.SetString("MoveDown", keyBinding.GetKey("MoveDown"));
            PlayerPrefs.SetString("MoveRight", keyBinding.GetKey("MoveRight"));
            PlayerPrefs.SetString("MoveLeft", keyBinding.GetKey("MoveLeft"));
            PlayerPrefs.SetString("Sprint", keyBinding.GetKey("Sprint"));
            //Debug.Log(PlayerPrefs.GetString("Hints","H"));
            //-------------------------------------
            PlayerPrefs.SetInt("ChangeFont", 0);
        }
        PlayerPrefs.Save();
    }

    public void LoadPrefs()
    {
        if (SceneManager.GetActiveScene().name == "Menu")
        {
            options.SetVolume(PlayerPrefs.GetFloat(volume, 0));
            options.SetQuality(PlayerPrefs.GetInt(graphics, 5));
            options.SetResolution(PlayerPrefs.GetInt(resolution, options.resolutions.Length - 1));
        }
        //keyBinding.SetKey((KeyCode)System.Enum.Parse(typeof(KeyCode) ,PlayerPrefs.GetString("Hints","H")));
    }
}
