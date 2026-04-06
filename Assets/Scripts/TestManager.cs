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

    [Header("Scene Indices")]
    public int start = 0; // Set this to the appropriate scene index for your start scene.
    public int game  = 1; // Set this to the appropriate scene index for your test scene.

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
        SceneManager.LoadScene(game);
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

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);

        if (SceneManager.GetActiveScene().buildIndex == game)
        {
            LoadVersionFromMemory();
        }
    }
}
