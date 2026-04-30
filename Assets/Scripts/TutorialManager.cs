using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public static TutorialManager instance;

    public GameObject hud;
    public List<GameObject> tutorialPanels;
    public int currentPanelIndex = 0;

    private InputHandler input;
    private bool pauseNavInput = false;
    private bool tutorialInputTriggered = false;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);
    }

    private void Start()
    {
        hud.SetActive(false);
        foreach (GameObject panel in tutorialPanels)
        {
            panel.SetActive(false);
        }
        instance.tutorialPanels[0].SetActive(true);
        input = GameObject.FindGameObjectWithTag("Manager").GetComponent<InputHandler>();
        if (input == null)
        {
            Debug.Log("InputHandler component not found on the manager object. Please add an InputHandler component.");
            input = InputHandler.instance; // Fallback to the singleton instance if not found on the player object
        }
        input.EnableUIInput();
    }

    void Update()
    {
        if (input.ui_navigation_input.x < 0 && !pauseNavInput)
        {
            if (currentPanelIndex > 0)
            {
                tutorialPanels[currentPanelIndex].SetActive(false);
                currentPanelIndex--;
                tutorialPanels[currentPanelIndex].SetActive(true);
                pauseNavInput = true;
            }
        }
        if (input.ui_navigation_input.x > 0 && !pauseNavInput)
        {
            if (currentPanelIndex < tutorialPanels.Count - 1)
            {
                tutorialPanels[currentPanelIndex].SetActive(false);
                currentPanelIndex++;
                tutorialPanels[currentPanelIndex].SetActive(true);
                pauseNavInput = true;
            }
            else
            {
                tutorialPanels[currentPanelIndex].SetActive(false);
                hud.SetActive(true);
                input.EnablePlayerInput();
            }
        }
        if (input.ui_navigation_input.x == 0)
        {
            pauseNavInput = false;
        }
        if (input.ui_exit_triggered)
        {
            tutorialPanels[currentPanelIndex].SetActive(false);
            hud.SetActive(true);
            input.EnablePlayerInput();
        }

        if (input.player_tutorial_triggered)
        {
            if (!tutorialInputTriggered)
            {
                if (tutorialPanels[currentPanelIndex].activeSelf)
                {
                    tutorialPanels[currentPanelIndex].SetActive(false);
                    hud.SetActive(true);
                    input.EnablePlayerInput();
                }
                else
                {
                    input.EnableUIInput();
                    tutorialPanels[currentPanelIndex].SetActive(true);
                    hud.SetActive(false);
                }

                tutorialInputTriggered = true;
            }
        }
        else tutorialInputTriggered = false;
    }

}
