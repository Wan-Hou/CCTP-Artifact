using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.LowLevel;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum ColourChannel
{
    Red,
    Green,
    Blue
}

public class ColourControls : MonoBehaviour
{
    public static ColourControls instance;

    private InputHandler inputHandler;

    [Header("Settings")]
    [SerializeField] private Image UIFilter;

    [Header("Indexes")]
    public Vector3Int colourIndex;
    private float bufferTime = 0.5f;
    private Vector3 colourIndexBuffer;
    public int currentColourIndex;
    public Material activatedObstacleMaterial;
    public Material wallMaterial;

    [Header("Keys")]
    //public KeyCode reset        = KeyCode.R;
    public KeyCode redDownKey   = KeyCode.O;
    public KeyCode redUpKey     = KeyCode.P;
    public KeyCode greenDownKey = KeyCode.K;
    public KeyCode greenUpKey   = KeyCode.L;
    public KeyCode blueDownKey  = KeyCode.N;
    public KeyCode blueUpKey    = KeyCode.M;

    [Header("Lists")]
    public List<GameObject> resetList;
    public List<Obstacle> obstacles;

    public void ObstacleActivationCheck()
    {
        foreach (Obstacle obs in obstacles)
        {
            if (obs.CheckObstacleActivation()) obs.ActivateObstacle();
            else obs.DeactivateObstacle();
        }
    }

    void ChangeFilterColour()
    { 
        int redValue   = colourIndex[0] == 0 ? 0 : 64 * colourIndex[0] - 1;
        int greenValue = colourIndex[1] == 0 ? 0 : 64 * colourIndex[1] - 1;
        int blueValue  = colourIndex[2] == 0 ? 0 : 64 * colourIndex[2] - 1;

        UIFilter.color = new Color(redValue / 255f, greenValue / 255f, blueValue / 255f, UIFilter.color.a);

        ColourUIManager.instance.UIUpdate();
        ObstacleActivationCheck();
    }

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);
    }

    private void Start()
    {
        resetList.Add(GameObject.FindGameObjectWithTag("Player"));
        resetList.AddRange(GameObject.FindGameObjectsWithTag("Interactable"));
        resetList.AddRange(GameObject.FindGameObjectsWithTag("Obstacle"));
        obstacles.AddRange(FindObjectsByType<Obstacle>(FindObjectsSortMode.None));
        ColourUIManager.instance.UIUpdate();
        inputHandler = GameObject.FindGameObjectWithTag("Manager").GetComponent<InputHandler>();
        if (inputHandler == null)
        {
            Debug.Log("InputHandler component not found on the manager object. Please add an InputHandler component.");
            inputHandler = InputHandler.instance; // Fallback to the singleton instance if not found on the player object
        }
    }

    void Update()
    {
        if (inputHandler.player_scrollwheel_input.y < 0)
        {
            if (currentColourIndex < 2) currentColourIndex++;
            else currentColourIndex = 0;
            ColourUIManager.instance.UIUpdate();
        }

        if (inputHandler.player_scrollwheel_input.y > 0)
        {
            if (currentColourIndex > 0) currentColourIndex--;
            else currentColourIndex = 2;
            ColourUIManager.instance.UIUpdate();
        }

        if (inputHandler.player_decrease_triggered)
        {
            if (colourIndexBuffer[currentColourIndex] == 0)
            {
                colourIndex[currentColourIndex] = Mathf.Max(0, colourIndex[currentColourIndex] - 1);
                ChangeFilterColour();
                colourIndexBuffer = Vector3.zero;
                colourIndexBuffer[currentColourIndex] = bufferTime;
            }
            else
            {
                colourIndexBuffer[currentColourIndex] = 
                    Mathf.Max(0, colourIndexBuffer[currentColourIndex] - Time.deltaTime);
            }
        }

        if (inputHandler.player_increase_triggered)
        {
            if (colourIndexBuffer[currentColourIndex] == 0)
            {
                colourIndex[currentColourIndex] = Mathf.Min(4, colourIndex[currentColourIndex] + 1);
                ChangeFilterColour();
                colourIndexBuffer = Vector3.zero;
                colourIndexBuffer[currentColourIndex] = bufferTime;
            }
            else
            {
                colourIndexBuffer[currentColourIndex] =
                    Mathf.Max(0, colourIndexBuffer[currentColourIndex] - Time.deltaTime);
            }
        }

        if (inputHandler.player_decrease_triggered == false && inputHandler.player_increase_triggered == false)
        {
            colourIndexBuffer = Vector3.zero;
        }

        /*if (Input.GetKeyDown(reset))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }*/

        if (Input.GetKeyDown(redDownKey))
        {
            colourIndex[0] = Mathf.Max(0, colourIndex[0] - 1);
            ChangeFilterColour();
        }

        if (Input.GetKeyDown(redUpKey))
        {
            colourIndex[0] = Mathf.Min(4, colourIndex[0] + 1);
            ChangeFilterColour();
        }

        if (Input.GetKeyDown(greenDownKey))
        {
            colourIndex[1] = Mathf.Max(0, colourIndex[1] - 1);
            ChangeFilterColour();
        }

        if (Input.GetKeyDown(greenUpKey))
        {
            colourIndex[1] = Mathf.Min(4, colourIndex[1] + 1);
            ChangeFilterColour();
        }

        if (Input.GetKeyDown(blueDownKey))
        {
            colourIndex[2] = Mathf.Max(0, colourIndex[2] - 1);
            ChangeFilterColour();
        }

        if (Input.GetKeyDown(blueUpKey))
        {
            colourIndex[2] = Mathf.Min(4, colourIndex[2] + 1);
            ChangeFilterColour();
        }
    }
}
