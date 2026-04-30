using StarterAssets;
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

    [Header("Level Skip")]
    public bool skipTriggered = false;
    public Transform playerSkip;
    public Transform interactableSkip;

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

    public void GoToVersion(string version)
    { 
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

    public void SaveVersionToMemory(string version)
    {
        PlayerPrefs.SetString("Version", version);
        SaveManager.instance.ClearSaveData();
        GoToVersion(version);
    }

    public void LoadVersionFromMemory()
    {
        if (PlayerPrefs.HasKey("Version"))
        {
            string version = PlayerPrefs.GetString("Version");
            SetTestMode(version);
            versionText.text = version;
            //Debug.Log("Version loaded: " + version);
        }
        else
        {
            Debug.LogWarning("No version found in memory.");
            SceneManager.LoadScene(start);
        }
    }

    public void SetScene(int index)
    {
        SceneManager.LoadScene(index);
    }

    public void OpenURL(string url)
    {
        if (!string.IsNullOrEmpty(url))
        {
            Application.OpenURL(url);
        }
    }

    public void SkipLevel()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        GameObject interactable = GameObject.FindGameObjectWithTag("Interactable");

        if (player != null)
        {
            player.GetComponent<PlayerController>().enabled = false;
            if (Camera.main != null)
            {
                if (Camera.main.GetComponent<ObjectCarry>().isCarryingObj)
                    Camera.main.GetComponent<ObjectCarry>().DropObjectNoRB();
                interactable = GameObject.FindGameObjectWithTag("Interactable");
            }
            player.transform.SetLocalPositionAndRotation(playerSkip.position + Vector3.up, playerSkip.rotation);
            player.GetComponent<PlayerController>().enabled = true;
            if (interactable.transform.parent != null)
                interactable.transform.SetParent(null);
            interactable.transform.localPosition = interactableSkip.position + Vector3.up;
            //Debug.Break();
            Debug.Log("Level skipped successfully.");
        }
        else
        {
            Debug.LogWarning("Player or Interactable not found. Cannot skip level.");
        }
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
            if (InputHandler.instance.player_reset_triggered || 
                InputHandler.instance.ui_reset_triggered || 
                InputHandler.instance.ui_cancel_triggered)
            {
                //Debug.Log("Resetting the game to build index " + start);
                SaveManager.instance.SaveToMemory();
                SceneManager.LoadScene(start);
            }

            if (InputHandler.instance.player_skip_triggered)
            {
                if (!skipTriggered)
                { 
                    SkipLevel(); 
                    skipTriggered = true;
                }
            }
            else skipTriggered = false;
        }
        else
        {
            if(Input.GetKeyDown(KeyCode.Escape))
            {
                Application.Quit();
            }
        }
    }
}
