using StarterAssets;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[System.Serializable]
public class TransformData
{
    public Vector3 position;
    public Quaternion rotation;
    public Vector3 scale;
}

[System.Serializable]
public class SaveData
{
    public string version;
    public TransformData player;
    public Vector3 playerRespawn;
    public TransformData cameraRoot;
    public Vector3Int colourIndex;
    public int currentColourIndex;
    public bool isCarryingObj;
    public TransformData interactable;
    public Vector3 interactableRespawn;
    public List<Obstacle> obstacles;
    public List<Vector3Int> obstacleKeys;
}

public class SaveManager : MonoBehaviour
{
    public static SaveManager instance;

    public SaveData saveData;

    public int start = 0;
    public Button continueButton;
    public Button buttonA;
    public Button buttonB;
    public Button buttonC;
    public Button buttonD;

    public void Save()
    {
        Transform player = GameObject.FindGameObjectWithTag("Player").transform;
        Transform cameraRoot = GameObject.FindGameObjectWithTag("CinemachineTarget").transform;

        saveData = new SaveData
        {
            version = PlayerPrefs.GetString("Version"),

            player = new TransformData{
                position = player.position,
                rotation = player.rotation,
                scale = player.localScale},

            playerRespawn = player.GetComponent<PlayerReset>().respawnPosition,

            cameraRoot = new TransformData{
                position = cameraRoot.position,
                rotation = cameraRoot.rotation,
                scale = cameraRoot.localScale},

            colourIndex = ColourControls.instance.colourIndex,
            currentColourIndex = ColourControls.instance.currentColourIndex
        };

        if (Camera.main != null)
            saveData.isCarryingObj = Camera.main.GetComponent<ObjectCarry>().isCarryingObj;
            

        if (!saveData.isCarryingObj)
        {
            Transform interactable = 
                GameObject.FindGameObjectWithTag("Interactable").transform;

            if (interactable.parent != null)
                interactable.SetParent(null);

            saveData.interactable = new TransformData{
                position = interactable.position,
                rotation = interactable.rotation,
                scale = interactable.localScale};

            saveData.interactableRespawn = interactable.GetComponent<PlayerReset>().respawnPosition;
        }
        else
        {
            GameObject carryObj = Camera.main.GetComponent<ObjectCarry>().carriedObject;
            carryObj.SetActive(true);
            saveData.interactableRespawn = carryObj.GetComponent<PlayerReset>().respawnPosition;
            carryObj.SetActive(false);
        }

        saveData.obstacles = FindObjectsByType<Obstacle>
            (FindObjectsSortMode.None).OrderBy(o => 
            {   if (int.TryParse(o.name, out int idx)) return idx;
                return int.MaxValue; }).ToList();
        saveData.obstacleKeys = saveData.obstacles.Select(o => o.colourKey).ToList();
    }

    public void Load()
    {
        Transform player = GameObject.FindGameObjectWithTag("Player").transform;
        player.GetComponent<PlayerController>().enabled = false;
        player.SetPositionAndRotation(saveData.player.position + Vector3.up, saveData.player.rotation);
        GameObject.FindGameObjectWithTag("CinemachineTarget").transform.SetPositionAndRotation
                (saveData.cameraRoot.position + Vector3.up, saveData.cameraRoot.rotation);
        player.GetComponent<PlayerController>().enabled = true;
        ColourControls.instance.colourIndex = saveData.colourIndex;
        ColourControls.instance.currentColourIndex = saveData.currentColourIndex;
        if (saveData.isCarryingObj)
        {
            if (Camera.main != null) Camera.main.GetComponent<ObjectCarry>().
                    PickUpNoRB(GameObject.FindGameObjectWithTag("Interactable"));
        }
        else GameObject.FindGameObjectWithTag("Interactable").transform.SetPositionAndRotation
                (saveData.interactable.position + Vector3.up, saveData.interactable.rotation);
        StartCoroutine(LoadObstacleKey());
    }

    public void SaveToMemory()
    {
        Save();
        string jsonData = JsonUtility.ToJson(saveData);
        //Debug.Log("Serialized SaveData: " + jsonData);

        PlayerPrefs.SetString("SaveData", jsonData);
        PlayerPrefs.Save();
        //Debug.Log("Game saved to memory.");
    }

    public void LoadFromMemory()
    {
        if (PlayerPrefs.HasKey("SaveData"))
        {
            saveData = JsonUtility.FromJson<SaveData>(PlayerPrefs.GetString("SaveData"));
            if (saveData.version == PlayerPrefs.GetString("Version"))
            {
                Load();
                //Debug.Log("Save data found. Game loaded.");
                return;
            }
        }

        //Debug.Log("No save data found or version mismatch. Starting new game.");
        ColourControls.instance.ObstacleActivationCheck();
    }

    public void ClearSaveData()
    {
        PlayerPrefs.DeleteKey("SaveData");
        //Debug.Log("Save data cleared from memory.");
    }

    public void ContinueGame()
    {
        PlayerPrefs.SetString("Version", saveData.version);
        TestManager.instance.GoToVersion(saveData.version);
    }

    IEnumerator LoadObstacleKey()
    {
        saveData.obstacles = FindObjectsByType<Obstacle>
            (FindObjectsSortMode.None).OrderBy(o =>
            {   if (int.TryParse(o.name, out int idx)) return idx;
                return int.MaxValue; }).ToList();
        bool colourAssigned = false;
        while (!colourAssigned)
        {
            foreach (Obstacle obstacle in saveData.obstacles)
            {
                colourAssigned = obstacle.colourAssigned;
            }
            yield return null;
        }
        for (int i = 0; i < saveData.obstacles.Count && i < saveData.obstacleKeys.Count; i++)
        {
            saveData.obstacles[i].SwapColour(saveData.obstacleKeys[i]);
        }
        ColourControls.instance.ObstacleActivationCheck();
        yield break;
    }

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else Destroy(gameObject);

        if (SceneManager.GetActiveScene().buildIndex == start)
        {
            if (PlayerPrefs.HasKey("SaveData"))
            {
                continueButton.transform.parent.gameObject.SetActive(true);
                saveData = JsonUtility.FromJson<SaveData>(PlayerPrefs.GetString("SaveData"));
                switch (saveData.version)
                {
                    case "A":
                    {
                        continueButton.colors = buttonA.colors;
                        continueButton.GetComponentInChildren<TMP_Text>().text = "Continue A";
                        break;
                    }
                    case "B":
                    {
                        continueButton.colors = buttonB.colors;
                        continueButton.GetComponentInChildren<TMP_Text>().text = "Continue B";
                        break;
                    }
                    case "C":
                    {
                        continueButton.colors = buttonC.colors;
                        continueButton.GetComponentInChildren<TMP_Text>().text = "Continue C";
                        break;
                    }
                    case "D":
                    {
                        continueButton.colors = buttonD.colors;
                        continueButton.GetComponentInChildren<TMP_Text>().text = "Continue D";
                        break;
                    }
                    default: break;
                }
                //Debug.Log("Save data found. Continue button enabled.");
            }
            else
            {
                continueButton.transform.parent.gameObject.SetActive(false);
                //Debug.Log("No save data found. Continue button disabled.");
            }
        }
        else
        {
            LoadFromMemory();
        }
    }
}
