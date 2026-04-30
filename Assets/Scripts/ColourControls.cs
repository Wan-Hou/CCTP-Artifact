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
    private readonly float bufferTime = 0.5f;
    private Vector3 colourIndexBuffer;
    public int currentColourIndex;
    public Material activatedObstacleMaterial;
    public Material wallMaterial;

    [Header("VFX")]
    public GameObject redMatchVFX;
    public GameObject greenMatchVFX;
    public GameObject blueMatchVFX;
    public GameObject activateVFX;

    /*[Header("Keys")]
    public KeyCode reset        = KeyCode.R;
    public KeyCode redDownKey   = KeyCode.O;
    public KeyCode redUpKey     = KeyCode.P;
    public KeyCode greenDownKey = KeyCode.K;
    public KeyCode greenUpKey   = KeyCode.L;
    public KeyCode blueDownKey  = KeyCode.N;
    public KeyCode blueUpKey    = KeyCode.M;*/

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

    public void ChangeFilterColour()
    { 
        /*int redValue   = colourIndex[0] == 0 ? 0 : 64 * colourIndex[0] - 1;
        int greenValue = colourIndex[1] == 0 ? 0 : 64 * colourIndex[1] - 1;
        int blueValue  = colourIndex[2] == 0 ? 0 : 64 * colourIndex[2] - 1;

        UIFilter.color = new Color(redValue / 255f, greenValue / 255f, blueValue / 255f, UIFilter.color.a);*/

        ColourUIManager.instance.UIUpdate();
        ObstacleActivationCheck();
    }

    void FilterInput(int colourChannel, int direction)
    {
        if (colourIndexBuffer[colourChannel] == 0)
        {
            colourIndex[colourChannel] = Mathf.Clamp(colourIndex[colourChannel] + direction, 0, 4);
            ChangeFilterColour();
            colourIndexBuffer[colourChannel] = bufferTime;
        }
        else
        {
            colourIndexBuffer[colourChannel] =
                Mathf.Max(0, colourIndexBuffer[colourChannel] - Time.deltaTime);
        }
        //Debug.Log($"Colour Index: {colourChannel} | Buffer: {colourIndexBuffer}");
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
            FilterInput(currentColourIndex, -1);
        }

        if (inputHandler.player_increase_triggered)
        {
            FilterInput(currentColourIndex, 1);
        }

        /*if (Input.GetKeyDown(reset))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }*/

        if (inputHandler.player_red_minus_triggered)
        {
            FilterInput(0, -1);
        }

        if (inputHandler.player_red_plus_triggered)
        {
            FilterInput(0, 1);
        }

        if (inputHandler.player_green_minus_triggered)
        {
            FilterInput(1, -1);
        }

        if (inputHandler.player_green_plus_triggered)
        {
            FilterInput(1, 1);
        }

        if (inputHandler.player_blue_minus_triggered)
        {
            FilterInput(2, -1);
        }

        if (inputHandler.player_blue_plus_triggered)
        {
            FilterInput(2, 1);
        }

        if (!inputHandler.player_decrease_triggered && 
            !inputHandler.player_increase_triggered &&
            !inputHandler.player_red_minus_triggered &&
            !inputHandler.player_red_plus_triggered &&
            !inputHandler.player_green_minus_triggered &&
            !inputHandler.player_green_plus_triggered &&
            !inputHandler.player_blue_minus_triggered &&
            !inputHandler.player_blue_plus_triggered)
        {
            colourIndexBuffer = Vector3.zero;
        }
    }
}
