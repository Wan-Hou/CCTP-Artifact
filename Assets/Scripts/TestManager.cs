using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TestManager : MonoBehaviour
{
    public static TestManager instance;

    [Header("Test Version Settings")]
    public bool use_visual_features = false;
    public bool use_CVD_mode = false;

    [Header("Version Text")]
    public TMP_Text versionText;

    [Header("CVD Filter")]
    public GameObject CVDFilter;

    [Header("Scene Indices")] // Set this to the appropriate scene index for your start scene.
    public int start     = 0; 
    public int game_CVND = 1;
    public int game_CVD  = 2;

    // Set the test mode based on the specified version (A, B, C, or D).
    public void SetTestMode(string version)
    {
        switch(version)
        {
            // Colour -> use_CVD_mode = false; Greyscale -> use_CVD_mode = true;
            // VF -> use_visual_features = true; No VF -> use_visual_features = false;
            case "A": 
            {
                // Version A: Colour & No VF:
                // baseline state to assess colour-based mechanics
                use_CVD_mode = false;
                use_visual_features = false;
                break;
            }
            case "B": 
            {
                // Version B: Colour + VF:
                // to evaluate pattern integration alongside colour
                use_CVD_mode = false;
                use_visual_features = true;
                break;
            }
            case "C": 
            {
                // Version C: Greyscale + No VF:
                // simulates severe CVD
                use_CVD_mode = true;
                use_visual_features = false;
                break;
            }
            case "D": 
            {
                // Version D: Greyscale + VF:
                // to evaluate pattern integration while simulates severe CVD
                use_CVD_mode = true;
                use_visual_features = true;
                break;
            }
            default:
            {
                Debug.LogError(
                    "Invalid test version specified." +
                    " Please choose A, B, C, or D.");
                break;
            }
        }

        CVDFilter.SetActive(use_CVD_mode);

    }

    public void SaveVersionToMemory(string version)
    {
        PlayerPrefs.SetString("Version", version);
        // PlayerPrefs.Save();
        Debug.Log("Version saved.");
        switch (version)
        {
            case "A":
            case "B":
            {
                SceneManager.LoadScene(game_CVND);
                break;
            }
            case "C":
            case "D":
            {
                SceneManager.LoadScene(game_CVD);
                break;
            }
            default:
            {
                Debug.LogError("Invalid test version specified.");
                break;
            }
        }
    }

    public void LoadVersionFromMemory()
    {
        if (PlayerPrefs.HasKey("Version"))
        {
            string version = PlayerPrefs.GetString("Version");
            SetTestMode(version);
            versionText.text = version;
            Debug.Log("Version loaded: " + version);
        }
        else
        {
            Debug.LogWarning("No version found in memory.");
            SceneManager.LoadScene(start);
        }
    }

    public void SetScene(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);

        if (SceneManager.GetActiveScene().buildIndex == start)
        {
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            LoadVersionFromMemory();
            Cursor.lockState = CursorLockMode.Locked;
        }
        
        //Debug.Log("Current Scene Build Index: " + SceneManager.GetActiveScene().buildIndex + ", Scene Name: " + SceneManager.GetActiveScene().name);
    }

    private void Update()
    {
        if (InputHandler.instance != null)
        {
            if (InputHandler.instance.player_r_triggered || 
                InputHandler.instance.ui_r_triggered)
            {
                //Debug.Log("Resetting the game to build index " + start);
                SceneManager.LoadScene(start);
            }
        }
    }
}
